mod board;
mod game;
mod player;
mod time_utils;

use game::GameState;

use crate::game::Direction;

#[rustler::nif(schedule = "DirtyCpu")]
fn new_game(number_of_players: u64, board_width: usize, board_height: usize) -> GameState {
    GameState::new(number_of_players, board_width, board_height)
}

#[rustler::nif(schedule = "DirtyCpu")]
fn move_player(game: GameState, player_id: u64, direction: Direction) -> GameState {
    let mut game_2 = game;
    game_2.move_player(player_id, direction);
    game_2
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

/// `key_pressed` determines whether the input is a press event as opposed to a keyup event
#[rustler::nif(schedule = "DirtyCpu")]
fn attack_aoe(
    game: GameState,
    attacking_player_id: u64,
    direction: Direction,
    keypressed: bool,
) -> GameState {
    let mut game_2 = game;

    if keypressed {
        // create/advance projectile
        game_2.advance_or_create_projectile(attacking_player_id, direction);
    } else {
        // explode projectile
        let projectile = {
            let player = game_2
                .players
                .iter()
                .find(|x| x.id == attacking_player_id)
                .unwrap();

            &player.projectile.clone()
        };

        if let Some(proj) = projectile {
            game_2.explode_projectile(proj);

            game_2
                .players
                .iter_mut()
                .find(|x| x.id == attacking_player_id)
                .unwrap()
                .projectile = None;
        }
    }
    game_2
}

rustler::init!(
    "Elixir.DarkWorldsServer.Engine.Game",
    [new_game, move_player, attack_player, attack_aoe]
);
