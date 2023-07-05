pub mod board;
pub mod character;
pub mod game;
pub mod game_configuration;
pub mod player;
pub mod projectile;
pub mod skills;
pub mod time_utils;
pub mod utils;
use crate::player::Player;
use crate::{board::GridResource, board::Tile, game::Direction, utils::RelativePosition};
use game::GameState;
use rustler::{Binary, Env, Term};
use std::collections::HashMap;
use std::str::FromStr;

#[rustler::nif(schedule = "DirtyCpu")]
fn new_game(
    selected_players: HashMap<u64, String>,
    number_of_players: u64,
    board_width: usize,
    board_height: usize,
    build_walls: bool,
    raw_characters_config: Vec<HashMap<Binary, Binary>>,
    raw_skills_config: Vec<HashMap<Binary, Binary>>,
) -> Result<GameState, String> {
    let characters_config = game_configuration::config_binaries_to_strings(raw_characters_config);
    let skills_config = game_configuration::config_binaries_to_strings(raw_skills_config);

    let mut selected_characters: HashMap<u64, character::Name> =
        HashMap::<u64, character::Name>::new();

    for (player_id, name) in selected_players {
        let val = character::Name::from_str(&name)
            .map_err(|_| format!("Can't parse the character name {name}"))?;
        selected_characters.insert(player_id, val);
    }

    GameState::new(
        selected_characters,
        number_of_players,
        board_width,
        board_height,
        build_walls,
        &characters_config,
        &skills_config,
    )
}

#[rustler::nif(schedule = "DirtyCpu")]
fn move_player(game: GameState, player_id: u64, direction: Direction) -> Result<GameState, String> {
    let mut game = game;
    game.move_player(player_id, direction)?;
    Ok(game)
}

#[rustler::nif(schedule = "DirtyCpu")]
fn world_tick(game: GameState) -> GameState {
    let mut game_2 = game;
    game_2.world_tick().expect("Failed to tick world");
    game_2
}
#[rustler::nif(schedule = "DirtyCpu")]
fn get_grid(game: GameState) -> Vec<Tile> {
    let board = game.board;
    let grid = board.grid.resource.lock().unwrap();
    grid.clone()
}

#[rustler::nif(schedule = "DirtyCpu")]
fn get_non_empty(game: GameState) -> HashMap<(usize, usize), Tile> {
    let mut result: HashMap<(usize, usize), Tile> = HashMap::new();
    let grid: Vec<Tile> = game.board.grid.resource.lock().unwrap().to_vec();
    for (index, elem) in grid.into_iter().enumerate() {
        match elem {
            Tile::Empty => continue,
            _ => {
                let width = game.board.width;
                let x = (index / width) as usize;
                let y = (index % width) as usize;
                result.insert((x, y), (elem).clone())
            }
        };
    }
    result
}

#[rustler::nif(schedule = "DirtyCpu")]
fn skill_1(
    game: GameState,
    attacking_player_id: u64,
    attack_position: RelativePosition,
) -> Result<GameState, String> {
    let mut game_2 = game;
    game_2.skill_1(attacking_player_id, &attack_position)?;
    Ok(game_2)
}

#[rustler::nif(schedule = "DirtyCpu")]
fn skill_2(
    game: GameState,
    attacking_player_id: u64,
    attack_position: RelativePosition,
) -> Result<GameState, String> {
    let mut game_2 = game;
    game_2.skill_2(attacking_player_id, &attack_position)?;
    Ok(game_2)
}

#[rustler::nif(schedule = "DirtyCpu")]
fn skill_3(
    game: GameState,
    attacking_player_id: u64,
    attack_position: RelativePosition,
) -> Result<GameState, String> {
    let mut game_2 = game;
    game_2.skill_3(attacking_player_id, &attack_position)?;
    Ok(game_2)
}

#[rustler::nif(schedule = "DirtyCpu")]
fn skill_4(
    game: GameState,
    attacking_player_id: u64,
    attack_position: RelativePosition,
) -> Result<GameState, String> {
    let mut game_2 = game;
    game_2.skill_4(attacking_player_id, &attack_position)?;
    Ok(game_2)
}

#[rustler::nif(schedule = "DirtyCpu")]
fn disconnect(game: GameState, player_id: u64) -> Result<GameState, String> {
    let mut game_2 = game;
    game_2.disconnect(player_id)?;
    Ok(game_2)
}

#[rustler::nif(schedule = "DirtyCpu")]
fn new_round(game: GameState, players: Vec<Player>) -> Result<GameState, String> {
    let mut game = game;
    game.new_round(players)?;
    Ok(game)
}

#[rustler::nif(schedule = "DirtyCpu")]
fn move_with_joystick(
    game: GameState,
    player_id: u64,
    x: f32,
    y: f32,
) -> Result<GameState, String> {
    let mut game_2 = game;
    game_2.move_with_joystick(player_id, x, y)?;
    Ok(game_2)
}

#[rustler::nif(schedule = "DirtyCpu")]
fn basic_attack(
    game: GameState,
    player_id: u64,
    direction: RelativePosition,
) -> Result<GameState, String> {
    let mut game_2 = game;
    game_2.basic_attack(player_id, &direction)?;
    Ok(game_2)
}

#[rustler::nif(schedule = "DirtyCpu")]
fn spawn_player(game: GameState, player_id: u64) -> Result<GameState, String> {
    let mut game_2 = game;
    game_2.spawn_player(player_id);
    Ok(game_2)
}

pub fn load(env: Env, _: Term) -> bool {
    rustler::resource!(GridResource, env);
    true
}

#[cfg(feature = "init_engine")]
rustler::init!(
    "Elixir.DarkWorldsServer.Engine.Game",
    [
        new_game,
        move_player,
        get_grid,
        get_non_empty,
        world_tick,
        disconnect,
        move_with_joystick,
        new_round,
        spawn_player,
        basic_attack,
        skill_1,
        skill_2,
        skill_3,
        skill_4,
    ],
    load = load
);
