use rustler::NifStruct;
use rustler::NifUnitEnum;

#[derive(Debug, Clone, NifUnitEnum)]
pub enum GameUpdateType {
    KILLEVENT,
}

#[derive(Debug, Clone, NifStruct)]
#[module = "DarkWorldsServer.Engine.GameUpdate"]
pub struct GameUpdate {
    pub killer_player_id: u64,
    pub killed_player_id: u64,
    pub game_update_type: GameUpdateType,
}

impl GameUpdate {
    pub fn new_kill_update(killer_id: u64, killed_id: u64) -> Self {
        Self {
            killer_player_id: killer_id,
            killed_player_id: killed_id,
            game_update_type: GameUpdateType::KILLEVENT,
        }
    }
}
