use crate::character::{Character, Effect};
use crate::time_utils::time_now;
use rustler::NifStruct;
use rustler::NifUnitEnum;

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
    pub first_skill_cooldown_left: u64,
    pub second_skill_cooldown_left: u64,
    pub third_skill_cooldown_left: u64,
    pub fourth_skill_cooldown_left: u64,
    // Timestamp when the cooldown started.
    pub basic_skill_cooldown_start: u64,
    pub first_skill_start: u64,
    pub second_skill_cooldown_start: u64,
    pub third_skill_start: u64,
    pub fourth_skill_start: u64,
    // This field is redundant given that
    // we have the Character filed, this his
    // hopefully temporary and to tell
    // the client which character is being used.
    pub character_name: String,
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
            first_skill_cooldown_left: 0,
            second_skill_cooldown_left: 0,
            third_skill_cooldown_left: 0,
            fourth_skill_cooldown_left: 0,
            basic_skill_cooldown_start: 0,
            first_skill_start: 0,
            second_skill_cooldown_start: 0,
            third_skill_start: 0,
            fourth_skill_start: 0,
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

        match self.character.status_effects.get(&Effect::Disarmed) {
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
        self.basic_skill_cooldown_left = (self.basic_skill_cooldown_start
            + self.character.cooldown_basic_skill())
        .checked_sub(now)
        .unwrap_or(0);
        self.first_skill_cooldown_left = (self.first_skill_start
            + self.character.cooldown_first_skill())
        .checked_sub(now)
        .unwrap_or(0);
        self.second_skill_cooldown_left = (self.second_skill_cooldown_start
            + self.character.cooldown_second_skill())
        .checked_sub(now)
        .unwrap_or(0);
        self.third_skill_cooldown_left = (self.third_skill_start
            + self.character.cooldown_third_skill())
        .checked_sub(now)
        .unwrap_or(0);
        self.fourth_skill_cooldown_left = (self.fourth_skill_start
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

#[derive(Debug, Clone, NifStruct, PartialEq)]
#[module = "DarkWorldsServer.Engine.RelativePosition"]
pub struct RelativePosition {
    pub x: i64,
    pub y: i64,
}

impl RelativePosition {
    pub fn new(x: i64, y: i64) -> Self {
        Self { x, y }
    }
}
