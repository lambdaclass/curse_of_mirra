pub mod board;
pub mod character;
pub mod game;
pub mod player;
pub mod projectile;
pub mod skills;
pub mod time_utils;
use game::GameState;
use rustler::{Binary, Env, Term};
use std::collections::HashMap;

use crate::player::Player;
use crate::{board::GridResource, board::Tile, game::Direction, player::RelativePosition};

#[rustler::nif(schedule = "DirtyCpu")]
fn new_game(
    number_of_players: u64,
    board_width: usize,
    board_height: usize,
    build_walls: bool,
    characters_config: Vec<HashMap<Binary, Binary>>,
) -> Result<GameState, String> {
    let mut config: Vec<HashMap<String, String>> = vec![];
    for map in characters_config {
        let mut char: HashMap<String, String> = HashMap::new();
        for (key, val) in map {
            // A rustler binary derefs into [u8], see:
            // https://docs.rs/rustler/latest/rustler/types/binary/struct.Binary.html
            let key = String::from_utf8((*key).to_vec())
                .expect("Could not parse {key} into a Rust string!");
            let val = String::from_utf8((*val).to_vec())
                .expect("Could not parse {val} into a Rust string!");
            char.insert(key, val);
        }
        config.push(char);
    }
    GameState::new(
        number_of_players,
        board_width,
        board_height,
        build_walls,
        &config,
    )
}

#[rustler::nif(schedule = "DirtyCpu")]
fn move_player(game: GameState, player_id: u64, direction: Direction) -> GameState {
    let mut game_2 = game;
    game_2.move_player(player_id, direction);
    game_2
}

#[rustler::nif(schedule = "DirtyCpu")]
fn world_tick(game: GameState) -> GameState {
    let mut game_2 = game;
    game_2.world_tick().expect("Failed to tick world");
    game_2
}
#[rustler::nif(schedule = "DirtyCpu")]
fn get_grid(game: GameState) -> Vec<Vec<Tile>> {
    let grid = game.board.grid.resource.lock().unwrap();
    grid.clone()
}

#[rustler::nif(schedule = "DirtyCpu")]
fn get_non_empty(game: GameState) -> HashMap<(usize, usize), Tile> {
    let mut result: HashMap<(usize, usize), Tile> = HashMap::new();
    let grid = game.board.grid.resource.lock().unwrap();
    for (x, row) in grid.iter().enumerate() {
        for (y, e) in row.iter().enumerate() {
            match e {
                Tile::Empty => continue,
                _ => result.insert((x, y), (*e).clone()),
            };
        }
    }
    result
}

#[rustler::nif(schedule = "DirtyCpu")]
fn attack_player(
    game: GameState,
    attacking_player_id: u64,
    attack_direction: Direction,
) -> GameState {
    let mut game_2 = game;
    game_2.attack_player(attacking_player_id, attack_direction);
    game_2
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
fn disconnect(game: GameState, player_id: u64) -> Result<GameState, String> {
    let mut game_2 = game;
    game_2.disconnect(player_id)?;
    Ok(game_2)
}

#[rustler::nif(schedule = "DirtyCpu")]
fn new_round(game: GameState, players: Vec<Player>) -> GameState {
    let mut game_2 = game;
    game_2.new_round(players);
    game_2
}

#[rustler::nif(schedule = "DirtyCpu")]
fn move_with_joystick(
    game: GameState,
    player_id: u64,
    x: f64,
    y: f64,
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
pub fn auto_attack(
    game: GameState,
    player_id: u64,
    target_player_id: u64,
) -> Result<GameState, String> {
    let mut game = game;
    game.auto_attack(player_id, target_player_id)?;
    Ok(game)
}
#[rustler::nif(schedule = "DirtyCpu")]
fn spawn_player(game: GameState, player_id: u64) -> GameState {
    let mut game_2 = game;
    game_2.spawn_player(player_id);
    game_2
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
        attack_player,
        world_tick,
        disconnect,
        move_with_joystick,
        new_round,
        spawn_player,
        auto_attack,
        basic_attack,
        skill_1,
    ],
    load = load
);
