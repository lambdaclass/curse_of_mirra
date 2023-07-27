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
}

#[derive(Debug, Clone, NifUnitEnum, PartialEq)]
pub enum DecoyStatus {
    DECOYALIVE,
    DECOYDEAD,
    DECOYRESPAWNED,
}

impl Decoy {
    pub fn new(
        id: u64,
        position: Position,
        health: i64,
        owner: u64,
        status: DecoyStatus,
    ) -> Self {
        Self {
            id,
            position,
            health,
            owner,
            status,
        }
    }
    pub fn modify_health(self: &mut Self, hp_points: i64) {
        if matches!(self.status, DecoyStatus::DECOYALIVE) {
            self.health = self.health.saturating_sub(hp_points);
            if self.health <= 0 {
                self.status = DecoyStatus::DECOYDEAD;
            }
        }
    }
}
