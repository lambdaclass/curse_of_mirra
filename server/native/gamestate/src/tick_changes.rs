use crate::effect_affected_players;
use crate::board::Board;
use crate::character::{Character, Name};
use crate::game::{Direction, GameState, KillEvent};
use crate::time_utils::{time_now, MillisTime};
use crate::utils::cmp_float;
use crate::{
    player::{Effect, EffectData, Player, PlayerAction, Position, Status},
    projectile::{Projectile, ProjectileStatus, ProjectileType},
};
use itertools::Itertools;
use rustler::NifStruct;
use std::cell::RefCell;
use std::collections::HashMap;
use std::ops::Deref;
use std::rc::Rc;
pub type MutablePlayers = Vec<MutablePlayer>;
#[derive(Debug)]
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
        player.inner.deref().borrow().clone()
    }
}
impl Clone for MutablePlayer {
    fn clone(&self) -> MutablePlayer {
        MutablePlayer {
            inner: self.inner.clone(),
        }
    }
}
impl MutablePlayer {
    // READ ONLY FUNCTIONS
    pub fn dash_dmg(&self) -> u32 {
        self.inner.borrow().skill_3_damage()
    }
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
    pub fn dash_effect(&self) -> Effect {
        self.inner.borrow().dash_effect()
    }
    // PLAYER MUTATING FUNCTIONS
    pub fn set_action(&self, action: &PlayerAction) {
        self.inner.borrow_mut().action = action.clone();
    }
    pub fn set_position(&self, pos: Position) {
        self.inner.borrow_mut().position = pos;
    }
    fn update_cooldowns(&self, now: &MillisTime) {
        self.inner.borrow_mut().update_cooldowns(*now);
    }
    fn update_effects_time_left(
        &self,
        now: &MillisTime,
    ) -> Result<Vec<(Effect, EffectData)>, String> {
        self.inner.borrow_mut().update_effects_time_left(now)
    }
    pub fn modify_health(&self, hp_points: i64) {
        self.inner.borrow_mut().modify_health(hp_points);
    }
    /// World Tick Updates that should happen for every player,
    /// regardless of what the player is doing.
    pub fn world_tick_updates(
        &self,
        now: &MillisTime,
    ) -> Result<Vec<(Effect, EffectData)>, String> {
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

/// Holds necessary data to update
/// the GameState on each tick.
#[derive(NifStruct)]
#[module = "DarkWorldsServer.Engine.WorldTickChanges"]
pub struct TickChanges {
    // TODO:
    // There's a simple performance gain here,
    // we can use arrays/vectors instead of hashmaps,
    // fully sized with the number of players.
    pub neon_crash_affected_players: HashMap<u64, (i64, Vec<u64>)>,
    pub leap_affected_players: HashMap<u64, (i64, Vec<u64>)>,
    pub uma_mirroring_affected_players: HashMap<u64, (i64, u64)>,
    pub projectile_affected_players: HashMap<u64, (i64, Vec<u64>)>,
    pub reference_time: MillisTime,
    pub tick_killed_events: Vec<KillEvent>,
    pub kill_count: Vec<u64>,
}
impl TickChanges {
    pub fn new() -> TickChanges {
        TickChanges {
            neon_crash_affected_players: HashMap::new(),
            leap_affected_players: HashMap::new(),
            uma_mirroring_affected_players: HashMap::new(),
            reference_time: time_now(),
            kill_count: vec![],
            tick_killed_events: vec![],
            projectile_affected_players: HashMap::new(),
        }
    }

    /// Apply the effects of a projectile on the given player,
    /// on this tick.
    fn apply_projectile_on_player(
        &mut self,
        attacking_player: &MutablePlayer,
        attacked_player: &MutablePlayer,
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
        // Remove this if we end up using MutablePlayer
        let mut players_vec: Vec<Player> = vec![];
        for player in players.iter() {
            players_vec.push(player.clone().into())
        }
        let affected_players: HashMap<u64, f64> = GameState::players_in_range(
            projectile.player_id,
            &players_vec,
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
                (&(*attacking_player)).into(),
                (&(*attacked_player)).into(),
                projectile,
            )?;
        }
        Ok(())
    }

    pub fn attack_mirrored_players(&mut self, players: &MutablePlayers) -> Result<(), String> {
        for (_player_id, (damage, attacked_player_id)) in
            self.uma_mirroring_affected_players.iter_mut()
        {
            let attacked_player = players
                .get((attacked_player_id.deref() - 1) as usize)
                .expect("Player with invalid ID aborting!!!");
            attacked_player.modify_health(-damage.deref())
        }

        Ok(())
    }

    pub fn world_tick_players_in_range(
        players: MutablePlayers,
        attacking_position: &Position,
        range: f64,
    ) -> Vec<u64> {
        let mut players_in_range: Vec<u64> = vec![];
        for player in players {
            if Position::distance_between(&(player.position()), attacking_position) <= range
                && !matches!(player.status(), Status::DEAD)
            {
                players_in_range.push(player.id());
            }
        }
        players_in_range
    }
    pub fn attack_players_with_effect(
        &mut self,
        effect: Effect,
        players: &mut MutablePlayers,
    ) -> Result<(), String> {
        let affected_players = match effect {
            Effect::NeonCrashing => &self.neon_crash_affected_players,
            Effect::Leaping => &self.leap_affected_players,
            _ => todo!("Attack with effect not implemented for {effect:?}"),
        };
        for (_player_id, (damage, attacked_players)) in affected_players.iter() {
            for target_player_id in attacked_players.iter() {
                let attacked_player = players
                    .get((target_player_id - 1) as usize)
                    .expect("Player with invalid ID aborting!!!");
                attacked_player.modify_health(-damage);
            }
        }
        self.attack_mirrored_players(players)?;
        Ok(())
    }
    /// Apply dashing effects from a player
    /// to the current tick changes.
    pub fn apply_dashing_effects(
        &mut self,
        player: &MutablePlayer,
        expired_effects: &[(Effect, EffectData)],
        players: &MutablePlayers,
        board: &Board,
        effect_to_apply: &Effect,
    ) -> Result<(), String> {
        let to_affect = effect_affected_players!(effect_to_apply, self);
        // If the effect is something active, we apply that.
        if let Some(effect) = player.fetch_effect(effect_to_apply) {
            effect.direction.map(|direction| -> Result<(), String> {
                GameState::move_with_dash(
                    player,
                    player.speed(),
                    player.id(),
                    players,
                    &board,
                    to_affect,
                    &direction,
                )?;
                Ok(())
            });
            return Ok(());
        }
        // If the effect is something that expires, like
        // muflus' leap, then we apply that.
        if let Some((_, data)) = expired_effects
            .iter()
            .find_or_first(|(effect, _data)| effect == effect_to_apply)
        {
            data.direction.map(|direction| -> Result<(), String> {
                player.set_action(&PlayerAction::EXECUTINGSKILL3);
                GameState::move_with_dash(
                    player,
                    player.speed(),
                    player.id(),
                    players,
                    &board,
                    to_affect,
                    &direction,
                )?;
                Ok(())
            });
            return Ok(());
        }
        Ok(())
    }
}
#[macro_export]
macro_rules! effect_affected_players {
    ($effect:expr, $self:ident) => {
        match $effect {
            Effect::NeonCrashing => &mut $self.neon_crash_affected_players,
            Effect::Leaping => &mut $self.leap_affected_players,
            _ => todo!("Missing container for this effect: {:?}", $effect),
        }
    };
}
