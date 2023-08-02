use crate::character::{Character, Name};
use crate::game::{GameState, KillEvent};
use crate::player::{Effect, EffectData, Player, PlayerAction, Position, Status, StatusEffects};
use crate::projectile::Projectile;
use crate::time_utils::{time_now, MillisTime};
use rustler::{NifStruct, NifTuple, NifUnitEnum};
use std::cell::RefCell;
use std::collections::HashMap;
use std::rc::Rc;
pub type MutablePlayers = [MutablePlayer];
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
impl MutablePlayer {
    // READ ONLY FUNCTIONS
    pub fn position(&self) -> Position {
        self.inner.borrow().position
    }
    pub fn status(&self) -> Status {
        self.inner.borrow().status.clone()
    }
    pub fn id(&self) -> u64 {
        self.inner.borrow().id.clone()
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
}
