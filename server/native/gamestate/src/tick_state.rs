use crate::character::{Character, Name};
use crate::game::{GameState, KillEvent};
use crate::player::{Effect, EffectData, Player, PlayerAction, Position, Status, StatusEffects};
use crate::projectile::{Projectile, ProjectileType, ProjectileStatus};
use crate::time_utils::{time_now, MillisTime};
use crate::utils::cmp_float;
use rustler::{NifStruct, NifTuple, NifUnitEnum};
use std::cell::RefCell;
use std::collections::HashMap;
use std::rc::Rc;
pub type MutablePlayers = Vec<MutablePlayer>;
pub struct MutablePlayer {
    inner: Rc<RefCell<Player>>,
}
impl From<Player> for MutablePlayer {
    fn from(player: Player) -> MutablePlayer {
        let player = Rc::new(RefCell::new(player.clone()));
        MutablePlayer { inner: player }
    }
}
impl From<MutablePlayer> for Player {
    fn from(player: MutablePlayer) -> Player {
        return player.into();
    }
}
impl Clone for MutablePlayer {
    fn clone(&self) -> MutablePlayer {
        return MutablePlayer{ inner: self.inner.clone() }
    }
}
impl MutablePlayer {
    // READ ONLY FUNCTIONS
    pub fn position(&self) -> Position {
        self.inner.borrow().position
    }
    pub fn status(&self) -> Status {
        self.inner.borrow().status.clone()
    }
    pub fn id(&self) -> u64 {
        self.inner.borrow().id
    }
    pub fn character(&self) -> Character {
        self.inner.borrow().character.clone()
    }
    pub fn is_under_effect(&self, effect: &Effect) -> bool {
        self.inner.borrow().effects.get(&effect).is_some()
    }

    pub fn fetch_effect(&self, effect: &Effect) -> Option<EffectData> {
        self.inner.borrow().effects.get(&effect).cloned()
    }
    pub fn speed(&self) -> u64 {
        self.inner.borrow().speed()
    }
    pub fn action(&self) -> PlayerAction {
        return self.inner.borrow().action.clone();
    }
    pub fn skill_3_damage(&self) -> u32 {
        self.inner.borrow().skill_3_damage()
    }
    pub fn is_dead(&self) -> bool {
        self.inner.borrow().is_dead()
    }
    pub fn kill_count(&self) -> u64 {
        self.inner.borrow().kill_count
    }
    pub fn is_alive(&self) -> bool {
        self.inner.borrow().is_alive()
    }
    // PLAYER MUTATING FUNCTIONS
    pub fn set_action(&self, action: &PlayerAction) {
        self.inner.borrow_mut().action = action.clone();
    }
    fn update_cooldowns(&self, now: &MillisTime) {
        self.inner.borrow_mut().update_cooldowns(*now);
    }
    fn update_effects_time_left(&self, now: &MillisTime) -> Result<Vec<Effect>, String> {
        self.inner.borrow_mut().update_effects_time_left(now)
    }
    pub fn modify_health(&self, hp_points: i64) {
        self.inner.borrow_mut().modify_health(hp_points);
    }
    /// World Tick Updates that should happen for every player,
    /// regardless of what the player is doing.
    pub fn world_tick_updates(&self, now: &MillisTime) -> Result<Vec<Effect>, String> {
        // Clean each player actions
        self.set_action(&PlayerAction::NOTHING);
        // Keep only (de)buffs that have
        // a non-zero amount of ticks left.
        self.update_cooldowns(&now);
        self.update_effects_time_left(&now)
    }
    pub fn disarm_by_projectile(
        &self,
        now: MillisTime,
        ends_at: MillisTime,
        projectile: &Projectile,
    ) {
        self.inner
            .borrow_mut()
            .disarm_by_projectile(now, ends_at, &projectile);
    }
    pub fn get_mirrored_player_id(&self) -> Option<u64> {
        self.inner.borrow_mut().get_mirrored_player_id()
    }
    pub fn increment_kill_count(&self, increment: u64) {
        self.inner.borrow_mut().kill_count += increment;
    }
}

#[derive(NifStruct)]
#[module = "DarkWorldsServer.Engine.WorldTickState"]
pub struct TickState {
    // TODO:
    // There's a simple performance gain here,
    // we can use arrays/vectors instead of hashmaps,
    // fully sized with the number of players.
    pub neon_crash_affected_players: HashMap<u64, (i64, Vec<u64>)>,
    pub leap_affected_players: HashMap<u64, (i64, Vec<u64>)>,
    pub uma_mirroring_affected_players: HashMap<u64, (i64, u64)>,
    pub reference_time: MillisTime,
    pub tick_killed_events: Vec<KillEvent>,
    pub kill_count: Vec<u64>,
}
impl TickState {
    pub fn new() -> TickState {
        TickState {
            neon_crash_affected_players: HashMap::new(),
            leap_affected_players: HashMap::new(),
            uma_mirroring_affected_players: HashMap::new(),
            reference_time: time_now(),
            kill_count: vec![],
            tick_killed_events: vec![],
        }
    }

    /// Apply the effects of a projectile on the given player,
    /// on this tick.
    fn apply_projectile_on_player(
        &mut self,
        attacking_player: MutablePlayer,
        attacked_player: MutablePlayer,
        projectile: &mut Projectile,
    ) -> Result<(), String> {
        match projectile.projectile_type {
            ProjectileType::DISARMINGBULLET => {
                let duration = MillisTime { high: 0, low: 5000 };
                let ends_at = crate::time_utils::add_millis(self.reference_time, duration);
                attacked_player.disarm_by_projectile(duration, ends_at, &projectile);
            }
            ProjectileType::BULLET => {
                attacked_player.modify_health(-(projectile.damage as i64));
                attacked_player.get_mirrored_player_id().map(|mirrored_id| {
                    self.uma_mirroring_affected_players.insert(
                        attacked_player.id(),
                        ((projectile.damage as i64) / 2, mirrored_id),
                    )
                });
                projectile.last_attacked_player_id = attacked_player.id();
            }
        }
        if attacked_player.is_dead() {
            self.tick_killed_events.push(KillEvent {
                kill_by: projectile.player_id,
                killed: attacked_player.id(),
            });
            attacking_player.increment_kill_count(1);
        }
        Ok(())
    }

    /// Apply a projectile to this tick
    pub fn active_projectile_update(
        &mut self,
        players: &MutablePlayers,
        projectile: &mut Projectile,
    ) -> Result<(), String> {
        let affected_players: HashMap<u64, f64> = GameState::players_in_projectile_movement(
            projectile.player_id,
            &(players.clone()).into_iter().map(Into::into).collect(),
            projectile.prev_position,
            projectile.position,
        )
        .into_iter()
        .filter(|&(id, _distance)| {
            id != projectile.player_id && id != projectile.last_attacked_player_id
        })
        .collect();

        if affected_players.len() > 0 && !projectile.pierce {
            projectile.status = ProjectileStatus::EXPLODED;
        }

        // Seems like the current logic is to count
        // kill_counts by one, right?
        // let mut kill_count = 0;

        // A projectile should attack only one player per tick
        if affected_players.len() > 0 {
            // If there are more than one players affected by the projectile
            // find the nearest one
            let (attacked_player_id, _) = affected_players
                .into_iter()
                .min_by(|(_, player_dist_1), (_, player_dist_2)| {
                    cmp_float(*player_dist_1, *player_dist_2)
                })
                .ok_or("No player found for projectile attack!")?;

            let attacking_player = players
                .get((projectile.player_id - 1) as usize)
                .ok_or("Non valid ID")?;
            let attacked_player = players
                .get((attacked_player_id - 1) as usize)
                .ok_or("Non valid ID")?;

            self.apply_projectile_on_player(
                attacking_player.clone(),
                attacked_player.clone(),
                projectile,
            )?;
        }
        Ok(())
    }
}
