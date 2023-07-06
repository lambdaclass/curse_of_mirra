use crate::character::{Character, Effect, Name, StatusEffects, TicksLeft};
use crate::skills::{Basic as BasicSkill, FirstActive};
use crate::time_utils::time_now;
use rand::Rng;
use rustler::NifStruct;
use rustler::NifUnitEnum;
use std::collections::HashMap;

/*
    Note: To track cooldowns we are storing the last system time when the ability/attack
    was used. This is not ideal, because system time is unreliable, but storing an `Instant`
    as a field on players does not work because it can't be encoded between Elixir and Rust.
*/

#[derive(Debug, Clone, NifStruct)]
#[module = "DarkWorldsServer.Engine.Player"]
pub struct Player {
    pub id: u64,
    pub health: i64,
    pub position: Position,
    /// Time of the last melee attack done by the player, measured in seconds.
    pub last_melee_attack: u64,
    pub status: Status,
    pub character: Character,
    pub action: PlayerAction,
    pub aoe_position: Position,
    pub kill_count: u64,
    pub death_count: u64,
    // How many seconds are left until the
    // cooldown is over.
    pub basic_skill_cooldown_left: u64,
    pub skill_1_cooldown_left: u64,
    pub skill_2_cooldown_left: u64,
    pub skill_3_cooldown_left: u64,
    pub skill_4_cooldown_left: u64,
    // Timestamp when the cooldown started.
    pub basic_skill_started_at: u64,
    pub skill_1_started_at: u64,
    pub skill_2_started_at: u64,
    pub skill_3_started_at: u64,
    pub skill_4_started_at: u64,
    // This field is redundant given that
    // we have the Character filed, this his
    // hopefully temporary and to tell
    // the client which character is being used.
    pub character_name: String,
    pub effects: StatusEffects,
}

#[derive(Debug, Clone, NifUnitEnum)]
pub enum Status {
    ALIVE,
    DEAD,
    DISCONNECTED,
}

#[derive(Debug, Clone, NifUnitEnum)]
pub enum PlayerAction {
    NOTHING,
    ATTACKING,
    ATTACKINGAOE,
    EXECUTINGSKILL1,
    EXECUTINGSKILL2,
    EXECUTINGSKILL3,
    EXECUTINGSKILL4,
    TELEPORTING,
}

#[derive(Debug, Copy, Clone, NifStruct, PartialEq)]
#[module = "DarkWorldsServer.Engine.Position"]
pub struct Position {
    pub x: usize,
    pub y: usize,
}

impl Player {
    pub fn new(id: u64, health: i64, position: Position, character: Character) -> Self {
        Self {
            id,
            health,
            position,
            last_melee_attack: time_now(),
            status: Status::ALIVE,
            character_name: character.name.to_string(),
            character,
            action: PlayerAction::NOTHING,
            aoe_position: Position::new(0, 0),
            kill_count: 0,
            death_count: 0,
            basic_skill_cooldown_left: 0,
            skill_1_cooldown_left: 0,
            skill_2_cooldown_left: 0,
            skill_3_cooldown_left: 0,
            skill_4_cooldown_left: 0,
            basic_skill_started_at: 0,
            skill_1_started_at: 0,
            skill_2_started_at: 0,
            skill_3_started_at: 0,
            skill_4_started_at: 0,
            effects: HashMap::new(),
        }
    }
    pub fn modify_health(self: &mut Self, hp_points: i64) {
        if matches!(self.status, Status::ALIVE) {
            self.health = self.health.saturating_add(hp_points);
            if self.health <= 0 {
                self.status = Status::DEAD;
                self.death_count += 1;
            }
        }
    }
    pub fn add_kills(self: &mut Self, kills: u64) {
        self.kill_count += kills;
    }

    pub fn basic_skill_damage(&self) -> u32 {
        let mut damage = self.character.attack_dmg_basic_skill();
        match self.character.skill_basic {
            BasicSkill::Bash => {
                if self.has_active_effect(&Effect::Raged) {
                    damage += 10_u32;
                }
                return damage;
            }
            _ => damage,
        }
    }
    pub fn skill_1_damage(&self) -> u32 {
        let mut damage = self.character.attack_dmg_first_active();
        match self.character.skill_active_first {
            FirstActive::BarrelRoll => {
                if self.has_active_effect(&Effect::Raged) {
                    damage += 10_u32;
                }
                return damage;
            }
            _ => damage,
        }
    }
    pub fn skill_2_damage(&mut self) -> u32 {
        return self.character.attack_dmg_second_active();
    }

    #[inline]
    pub fn add_effect(&mut self, e: Effect, tl: TicksLeft) {
        if !self.effects.contains_key(&e) {
            match self.character.name {
                Name::Muflus => {
                    if !(self.muflus_partial_immunity(&e)) {
                        self.effects.insert(e, tl);
                    }
                }
                _ => {
                    self.effects.insert(e.clone(), tl);
                }
            }
        }
    }

    #[inline]
    pub fn speed(&self) -> u64 {
        if self.has_active_effect(&Effect::Petrified) {
            return 0;
        }
        if self.has_active_effect(&Effect::Raged) {
            return ((self.character.base_speed as f64) * 1.5).ceil() as u64;
        }
        return self.character.base_speed;
    }

    fn muflus_partial_immunity(&self, effect_to_apply: &Effect) -> bool {
        effect_to_apply.is_crowd_control()
            && self.has_active_effect(&Effect::Raged)
            && Self::chance_check(0.3)
    }

    fn chance_check(chance: f64) -> bool {
        let mut rng = rand::thread_rng();
        let random: f64 = rng.gen();
        return random <= chance;
    }

    #[allow(unused_variables)]
    pub fn has_active_effect(&self, e: &Effect) -> bool {
        let effect = self.effects.get(e);
        matches!(effect, Some(1..=u64::MAX))
    }

    ///
    /// returns whether the player can do an attack, based on:
    ///
    /// - the player's status
    /// - the character's cooldown
    /// - the character's effects
    ///
    pub fn can_attack(self: &Self, cooldown_left: u64) -> bool {
        if matches!(self.status, Status::DEAD) {
            return false;
        }

        if cooldown_left > 0 {
            return false;
        }

        match self.effects.get(&Effect::Disarmed) {
            Some((1_u64..=u64::MAX)) => false,
            None | Some(0) => true,
        }
    }

    // TODO:
    // I think cooldown duration should be measured
    // in ticks instead of seconds to ensure
    // some kind of consistency.
    pub fn update_cooldowns(&mut self) {
        let now = time_now();
        // Time left of a cooldown = (start + left) - now
        // if (start) - left < now simply reset
        // the value as 0.
        self.basic_skill_cooldown_left = (self.basic_skill_started_at
            + self.character.cooldown_basic_skill())
        .checked_sub(now)
        .unwrap_or(0);

        self.skill_1_cooldown_left = (self.skill_1_started_at
            + self.character.cooldown_first_skill())
        .checked_sub(now)
        .unwrap_or(0);

        self.skill_2_cooldown_left = (self.skill_2_started_at
            + self.character.cooldown_second_skill())
        .checked_sub(now)
        .unwrap_or(0);

        self.skill_3_cooldown_left = (self.skill_3_started_at
            + self.character.cooldown_third_skill())
        .checked_sub(now)
        .unwrap_or(0);

        self.skill_4_cooldown_left = (self.skill_4_started_at
            + self.character.cooldown_fourth_skill())
        .checked_sub(now)
        .unwrap_or(0);
    }
}

impl Position {
    pub fn new(x: usize, y: usize) -> Self {
        Self { x, y }
    }
}
