use crate::game::Direction;
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
    pub projectile: Option<Projectile>,
    /// Time of the last melee attack done by the player, measured in seconds.
    pub last_melee_attack: u64,
    pub status: Status,
}

#[derive(Debug, Clone, NifStruct)]
#[module = "DarkWorldsServer.Engine.Projectile"]
pub struct Projectile {
    pub position: Position,
    pub direction: Direction,
    /// Time of the last movement performed by the projectile, measured in seconds.
    pub movement_cooldown: u64,
}

#[derive(Debug, Clone, NifUnitEnum)]
pub enum Status {
    ALIVE,
    DEAD,
}

#[derive(Debug, Clone, NifStruct)]
#[module = "DarkWorldsServer.Engine.Position"]
pub struct Position {
    pub x: usize,
    pub y: usize,
}

impl Player {
    pub fn new(id: u64, health: i64, position: Position) -> Self {
        Self {
            id,
            health,
            position,
            projectile: None,
            last_melee_attack: time_now(),
            status: Status::ALIVE,
        }
    }
}

impl Projectile {
    pub fn new(position: Position, direction: Direction) -> Projectile {
        Projectile {
            position,
            direction,
            movement_cooldown: 0,
        }
    }
}

impl Position {
    pub fn new(x: usize, y: usize) -> Self {
        Self { x, y }
    }
}
