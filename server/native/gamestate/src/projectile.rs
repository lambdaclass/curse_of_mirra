use crate::character::TicksLeft;
use crate::player::Position;
use rustler::NifStruct;
use rustler::NifUnitEnum;

#[derive(Debug, Clone, NifStruct)]
#[module = "DarkWorldsServer.Engine.Projectile"]
pub struct Projectile {
    pub id: u64,
    pub position: Position,
    pub direction: JoystickValues,
    pub speed: u32,
    pub range: u32,
    pub player_id: u64,
    pub damage: u32,
    pub remaining_ticks: TicksLeft,
    pub projectile_type: ProjectileType,
    pub status: ProjectileStatus,
}

#[derive(Debug, Clone, NifUnitEnum)]
pub enum ProjectileType {
    BULLET,
}

#[derive(Debug, Clone, NifUnitEnum, PartialEq)]
pub enum ProjectileStatus {
    ACTIVE,
    EXPLODED,
}

#[derive(Debug, Clone, NifStruct, PartialEq)]
#[module = "DarkWorldsServer.Engine.JoystickValues"]
pub struct JoystickValues {
    pub x: f64,
    pub y: f64,
}

impl JoystickValues {
    pub fn new(x: f64, y: f64) -> Self {
        Self { x, y }
    }
}

impl Projectile {
    pub fn new(
        id: u64,
        position: Position,
        direction: JoystickValues,
        speed: u32,
        range: u32,
        player_id: u64,
        damage: u32,
        remaining_ticks: TicksLeft,
        projectile_type: ProjectileType,
        status: ProjectileStatus,
    ) -> Self {
        Self {
            id,
            position,
            direction,
            speed,
            range,
            player_id,
            damage,
            remaining_ticks,
            projectile_type,
            status,
        }
    }
}
