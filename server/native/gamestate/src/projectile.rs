use crate::character::TicksLeft;
use crate::game;
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
    DISARMINGBULLET,
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
    pub fn move_or_explode_if_out_of_board(&mut self, board_height: usize, board_width: usize) {
        self.position = game::new_entity_position(
            board_height,
            board_width,
            self.direction.x,
            self.direction.y,
            self.position,
            self.speed as i64,
        );
        let Position { x, y } = self.position;
        // The projectile shouldn't move beyond the board limits,
        // but just in case, lets compare it with greater than or eq.
        let outside_height_range = x == 0 || x >= (board_height - 1);
        let outside_width_range = y == 0 || y >= (board_width - 1);
        let has_to_explode = outside_height_range || outside_width_range;
        if has_to_explode {
            self.status = ProjectileStatus::EXPLODED;
        }
    }
}
