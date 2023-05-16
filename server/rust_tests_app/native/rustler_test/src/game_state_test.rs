use crate::assert_result;
use crate::utils::TestResult;
use gamestate::board::GridResource;
use gamestate::board::Tile;
use gamestate::game::MELEE_ATTACK_COOLDOWN;
use gamestate::game::{Direction, GameState};
use gamestate::player::Player;
use gamestate::player::Position;
use gamestate::time_utils;
fn get_grid(game: &GameState) -> Vec<Vec<Tile>> {
    let grid = game.board.grid.resource.lock().unwrap();
    grid.clone()
}
#[rustler::nif]
pub fn no_move_if_beyond_boundaries() -> TestResult {
    let mut expected_grid: Vec<Vec<Tile>>;
    let (grid_height, grid_width) = (100, 100);
    let mut state = GameState::new(1, grid_height, grid_width, false);
    let player_id = state.players.first().unwrap().id;
    // Check UP boundary
    for i in 0..1000 {
        state.move_player(player_id, Direction::UP, 1);
    }
    assert_result!(0, state.players.first().unwrap().position.x)?;

    // Check DOWN boundary
    for i in 0..1000 {
        state.move_player(player_id, Direction::DOWN, 1);
    }
    assert_result!(99, state.players.first().unwrap().position.x)?;

    // Check RIGHT boundary
    for i in 0..1000 {
        state.move_player(player_id, Direction::RIGHT, 1);
    }
    assert_result!(99, state.players.first().unwrap().position.y)?;

    // Check LEFT boundary
    for i in 0..1000 {
        state.move_player(player_id, Direction::LEFT, 1);
    }
    assert_result!(0, state.players.first().unwrap().position.y)
}

#[rustler::nif]
fn no_move_if_occupied() -> TestResult {
    let mut state = GameState::new(2, 2, 2, false);
    let player1_id = 1;
    let player2_id = 2;
    let player1 = Player::new(player1_id, 100, Position::new(0, 0));
    let player2 = Player::new(player2_id, 100, Position::new(0, 1));
    state.players = vec![player1, player2];
    state.board.set_cell(0, 0, Tile::Player(player1_id));
    state.board.set_cell(0, 1, Tile::Player(player2_id));
    state.board.set_cell(1, 1, Tile::Empty);
    state.board.set_cell(1, 0, Tile::Empty);
    let expected_grid = get_grid(&state);
    state.move_player(player1_id, Direction::RIGHT, 1);
    assert_result!(expected_grid, get_grid(&state))
}

#[rustler::nif]
fn no_move_if_wall() -> TestResult {
    let mut state = GameState::new(1, 2, 2, false);
    let player1_id = 1;
    let player1 = Player::new(player1_id, 100, Position::new(0, 0));
    state.players = vec![player1];
    state.board.set_cell(0, 0, Tile::Player(player1_id));
    state.board.set_cell(0, 1, Tile::Wall);

    let expected_grid = get_grid(&state);
    state.move_player(player1_id, Direction::RIGHT, 1);
    assert_result!(expected_grid, get_grid(&state))
}

#[rustler::nif]
fn movement() -> TestResult {
    let mut state = GameState::new(0, 2, 2, false);
    let player_id = 1;
    let player1 = Player::new(player_id, 100, Position::new(0, 0));
    state.players = vec![player1];
    state.board.set_cell(0, 0, Tile::Player(player_id));

    state.move_player(player_id, Direction::RIGHT, 1);
    assert_result!(
        vec![
            vec![Tile::Empty, Tile::Player(player_id)],
            vec![Tile::Empty, Tile::Empty]
        ],
        get_grid(&state)
    )?;

    state.move_player(player_id, Direction::DOWN, 1);
    assert_result!(
        vec![
            vec![Tile::Empty, Tile::Empty],
            vec![Tile::Empty, Tile::Player(player_id)]
        ],
        get_grid(&state)
    )?;

    state.move_player(player_id, Direction::LEFT, 1);
    assert_result!(
        vec![
            vec![Tile::Empty, Tile::Empty],
            vec![Tile::Player(player_id), Tile::Empty]
        ],
        get_grid(&state)
    )?;

    state.move_player(player_id, Direction::UP, 1);
    assert_result!(
        vec![
            vec![Tile::Player(player_id), Tile::Empty],
            vec![Tile::Empty, Tile::Empty]
        ],
        get_grid(&state)
    )
}

#[rustler::nif]
fn attacking() -> TestResult {
    let mut state = GameState::new(0, 2, 2, false);
    let player_1_id = 1;
    let player_2_id = 2;
    let player1 = Player::new(player_1_id, 100, Position::new(0, 0));
    let player2 = Player::new(player_2_id, 100, Position::new(0, 0));
    state.players = vec![player1, player2];
    state.board.set_cell(0, 0, Tile::Player(player_1_id));
    state.board.set_cell(0, 1, Tile::Player(player_2_id));

    time_utils::sleep(MELEE_ATTACK_COOLDOWN);

    // Attack lands and damages player
    state.attack_player(player_1_id, Direction::RIGHT);
    assert_result!(100, state.players[0].health)?;
    assert_result!(90, state.players[1].health)?;

    // Attack does nothing because of cooldown
    state.attack_player(player_1_id, Direction::RIGHT);
    assert_result!(100, state.players[0].health)?;
    assert_result!(90, state.players[1].health)?;

    time_utils::sleep(MELEE_ATTACK_COOLDOWN);

    // Attack misses and does nothing
    state.attack_player(player_1_id, Direction::DOWN);
    assert_result!(100, state.players[0].health)?;
    assert_result!(90, state.players[1].health)?;

    time_utils::sleep(MELEE_ATTACK_COOLDOWN);

    state.move_player(player_1_id, Direction::DOWN, 1);

    // Attacking to the right now does nothing since the player moved down.
    state.attack_player(player_1_id, Direction::RIGHT);
    assert_result!(100, state.players[0].health)?;
    assert_result!(90, state.players[1].health)?;

    time_utils::sleep(MELEE_ATTACK_COOLDOWN);

    // Attacking to a non-existent position on the board does nothing.
    state.attack_player(player_1_id, Direction::LEFT);
    assert_result!(100, state.players[0].health)?;
    assert_result!(90, state.players[1].health)
}
