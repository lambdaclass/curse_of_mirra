use crate::player::Position;
use rustler::NifStruct;
use rustler::NifUnitEnum;

pub type TicksLeft = u64;

#[derive(Debug, Clone, NifStruct)]
#[module = "DarkWorldsServer.Engine.Decoy"]
pub struct Decoy {
    pub id: u64,
    pub position: Position,
    pub health: i64,
    pub owner: u64,
    pub status: DecoyStatus,
    pub should_respawn: bool,
}

#[derive(Debug, Clone, NifUnitEnum, PartialEq)]
pub enum DecoyStatus {
    DECOYALIVE,
    DECOYDEAD,
    DECOYRESPAWNED,
    DECOYTOEXPLODE,
}

impl Decoy {
    pub fn new(
        id: u64,
        position: Position,
        health: i64,
        owner: u64,
        status: DecoyStatus,
        should_respawn: bool,
    ) -> Self {
        Self {
            id,
            position,
            health,
            owner,
            status,
            should_respawn,
        }
    }
    pub fn modify_health(self: &mut Self, hp_points: i64) {
        if matches!(self.status, DecoyStatus::DECOYALIVE) {
            self.health = self.health.saturating_add(hp_points);
            if self.health <= 0 {
                self.status = DecoyStatus::DECOYTOEXPLODE;
            }
        }
        if matches!(self.status, DecoyStatus::DECOYRESPAWNED) {
            self.health = self.health.saturating_add(hp_points);
            if self.health <= 0 {
                self.status = DecoyStatus::DECOYTOEXPLODE;
                self.should_respawn = false;
            }
        }
    }
    pub fn is_alive(self: &Self) -> bool {
        matches!(self.status, DecoyStatus::DECOYALIVE) || matches!(self.status, DecoyStatus::DECOYRESPAWNED)
    }
}
