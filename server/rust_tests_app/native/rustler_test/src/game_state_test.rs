use crate::assert_result;
use crate::utils::{read_character_config, read_skills_config, TestResult};
use gamestate::board::{Grid, GridResource, Tile};
use gamestate::character::{Character, Name};
use gamestate::game::{Direction, GameState};
use gamestate::player::{Effect, EffectData, Player, Position};
use gamestate::time_utils;
use gamestate::utils::RelativePosition;
use std::collections::HashMap;

fn get_grid(game: &GameState) -> Grid {
    let grid = game.board.grid.resource.lock().unwrap();
    grid.clone()
}
fn speed1_character() -> Character {
    Character {
        base_speed: 1,
        ..Default::default()
    }
}
#[rustler::nif]
pub fn no_move_if_beyond_boundaries() -> TestResult {
    let mut expected_grid: Vec<Vec<Tile>>;
    let (grid_height, grid_width) = (100, 100);
    let mut selected_characters: HashMap<u64, Name> = HashMap::new();
    selected_characters.insert(1, Name::Muflus);
    let mut state = GameState::new(
        selected_characters,
        1,
        grid_height,
        grid_width,
        false,
        &read_character_config(),
        &read_skills_config(),
    )
    .unwrap();
    let mut player = state.players.get_mut(0).unwrap();
    player.character = Character {
        base_speed: 1,
        ..speed1_character()
    };
    let player_id = player.id;
    // Check UP boundary
    for i in 0..1000 {
        state.move_player(player_id, Direction::UP)?;
    }
    assert_result!(0, state.players.first().unwrap().position.x)?;

    // Check DOWN boundary
    for i in 0..1000 {
        state.move_player(player_id, Direction::DOWN)?;
    }
    assert_result!(99, state.players.first().unwrap().position.x)?;

    // Check RIGHT boundary
    for i in 0..1000 {
        state.move_player(player_id, Direction::RIGHT)?;
    }
    assert_result!(99, state.players.first().unwrap().position.y)?;

    // Check LEFT boundary
    for i in 0..1000 {
        state.move_player(player_id, Direction::LEFT)?;
    }
    assert_result!(0, state.players.first().unwrap().position.y)
}

#[rustler::nif]
fn no_move_if_occupied() -> TestResult {
    let mut selected_characters: HashMap<u64, Name> = HashMap::new();
    selected_characters.insert(1, Name::Muflus);
    selected_characters.insert(2, Name::Muflus);

    let mut state = GameState::new(
        selected_characters,
        2,
        2,
        2,
        false,
        &read_character_config(),
        &read_skills_config(),
    )
    .unwrap();
    let player1_id = 1;
    let player2_id = 2;
    let player1 = Player::new(player1_id, 100, Position::new(0, 0), speed1_character());
    let player2 = Player::new(player2_id, 100, Position::new(0, 1), speed1_character());
    state.players = vec![player1, player2];
    state.board.set_cell(0, 0, Tile::Player(player1_id))?;
    state.board.set_cell(0, 1, Tile::Player(player2_id))?;
    state.board.set_cell(1, 1, Tile::Empty)?;
    state.board.set_cell(1, 0, Tile::Empty)?;
    let expected_grid = get_grid(&state);
    state.move_player(player1_id, Direction::RIGHT)?;
    assert_result!(expected_grid, get_grid(&state))
}

#[rustler::nif]
fn no_move_if_wall() -> TestResult {
    let mut state = GameState::new(HashMap::new(), 1, 2, 2, false, &read_character_config(), &read_skills_config(),)?;
    let player1_id = 1;
    let player1 = Player::new(player1_id, 100, Position::new(0, 0), speed1_character());
    state.players = vec![player1];
    state.board.set_cell(0, 0, Tile::Player(player1_id))?;
    state.board.set_cell(0, 1, Tile::Wall)?;

    let expected_grid = get_grid(&state);
    state.move_player(player1_id, Direction::RIGHT)?;
    assert_result!(expected_grid, get_grid(&state))
}

#[rustler::nif]
fn movement() -> TestResult {
    let mut selected_characters: HashMap<u64, Name> = HashMap::new();
    selected_characters.insert(1, Name::Muflus);

    let mut state = GameState::new(
        selected_characters,
        0,
        2,
        2,
        false,
        &read_character_config(),
        &read_skills_config(),
    )
    .unwrap();
    let player_id = 1;
    let player1 = Player::new(player_id, 100, Position::new(0, 0), speed1_character());
    state.players = vec![player1];
    state.board.set_cell(0, 0, Tile::Player(player_id))?;

    state.move_player(player_id, Direction::RIGHT)?;
    assert_result!(
        vec![
            Tile::Empty,
            Tile::Player(player_id),
            Tile::Empty,
            Tile::Empty
        ],
        get_grid(&state)
    )?;

    state.move_player(player_id, Direction::DOWN)?;
    assert_result!(
        vec![
            Tile::Empty,
            Tile::Empty,
            Tile::Empty,
            Tile::Player(player_id)
        ],
        get_grid(&state)
    )?;

    state.move_player(player_id, Direction::LEFT)?;
    assert_result!(
        vec![
            Tile::Empty,
            Tile::Empty,
            Tile::Player(player_id),
            Tile::Empty
        ],
        get_grid(&state)
    )?;

    state.move_player(player_id, Direction::UP)?;
    assert_result!(
        vec![
            Tile::Player(player_id),
            Tile::Empty,
            Tile::Empty,
            Tile::Empty
        ],
        get_grid(&state)
    )
}

#[rustler::nif]
fn attacking() -> TestResult {
    let mut selected_characters: HashMap<u64, Name> = HashMap::new();
    selected_characters.insert(1, Name::Muflus);

    // FIXME: A 0 in new game is wrong!
    let mut state = GameState::new(
        selected_characters,
        0,
        20,
        20,
        false,
        &read_character_config(),
        &read_skills_config(),
    )
    .unwrap();
    let player_1_id = 1;
    let player_2_id = 2;
    let char: Character = speed1_character();
    let player1 = Player::new(player_1_id, 100, Position::new(0, 0), char.clone());
    let player2 = Player::new(player_2_id, 100, Position::new(0, 0), char.clone());
    state.players = vec![player1.clone(), player2];
    state.board.set_cell(0, 0, Tile::Player(player_1_id))?;
    state.board.set_cell(0, 1, Tile::Player(player_2_id))?;
    let cooldown = player1.character.cooldown_basic_skill();
    time_utils::sleep(cooldown);

    // Attack lands and damages player
    state
        .basic_attack(player_1_id, &RelativePosition::new(0f32, 1f32))
        .unwrap();
    assert_result!(100, state.players[0].health)?;
    assert_result!(100, state.players[1].health)?;

    // Attack does nothing because of cooldown
    state
        .basic_attack(player_1_id, &RelativePosition::new(0f32, 1f32))
        .unwrap();
    assert_result!(100, state.players[0].health)?;
    assert_result!(100, state.players[1].health)?;

    time_utils::sleep(cooldown);

    // Attack misses and does nothing
    state
        .basic_attack(player_1_id, &RelativePosition::new(0f32, 1f32))
        .unwrap();
    assert_result!(100, state.players[0].health)?;
    assert_result!(100, state.players[1].health)?;

    time_utils::sleep(cooldown);

    state.move_player(player_1_id, Direction::DOWN)?;

    // Attacking to the right now does nothing since the player moved down.
    state
        .basic_attack(player_1_id, &RelativePosition::new(0f32, 1f32))
        .unwrap();
    assert_result!(100, state.players[0].health)?;
    assert_result!(100, state.players[1].health)?;

    time_utils::sleep(cooldown);

    // Attacking to a non-existent position on the board does nothing.
    state
        .basic_attack(player_1_id, &RelativePosition::new(0f32, 1f32))
        .unwrap();
    assert_result!(100, state.players[0].health)?;
    assert_result!(100, state.players[1].health)
}

#[rustler::nif]
pub fn cant_move_if_petrified() -> TestResult {
    let now = time_utils::time_now();
    let mut selected_characters: HashMap<u64, Name> = HashMap::new();
    selected_characters.insert(1, Name::Muflus);
    let mut expected_grid: Vec<Vec<Tile>>;
    let (grid_height, grid_width) = (100, 100);
    let mut state = GameState::new(
        selected_characters,
        1,
        grid_height,
        grid_width,
        false,
        &read_character_config(),
        &read_skills_config(),
    )
    .unwrap();
    let spawn_point = Position { x: 50, y: 50 };
    let base_speed = 1;
    state.players[0].position = spawn_point.clone();
    (state.players[0]).character = Character {
        base_speed,
        ..Default::default()
    };
    state.players[0].effects.insert(Effect::Petrified, EffectData{
        time_left: time_utils::MillisTime {
            high: 0,
            low: 2000
        },
        ends_at: time_utils::add_millis(now, time_utils::MillisTime {
            high: 0,
            low: 2000
        }), direction: None, position: None});
    let player_id = 1;
    let mut player = state.get_player(player_id)?;

    // Sleep 1 seconds and update status, the character should not be able to move.
    time_utils::sleep(time_utils::u128_to_millis(1000));
    // std::thread::sleep(std::time::Duration::from_secs(1));
    state.world_tick()?;

    // Try to move 10 times, the player/character should not move.
    for i in 0..10 {
        state.move_player(player_id, Direction::DOWN)?;
        state.move_player(player_id, Direction::LEFT)?;
        state.move_player(player_id, Direction::UP)?;
        state.move_player(player_id, Direction::RIGHT)?;
        assert_result!(spawn_point.x, player.position.x)?;
    }

    // Sleep 2 seconds and update status, now the character should be able to move.
    // std::thread::sleep(std::time::Duration::from_secs(2));
    time_utils::sleep(time_utils::u128_to_millis(2000));
    state.world_tick()?;

    state.move_player(player_id, Direction::DOWN)?;
    player = state.get_player(player_id)?;
    assert_result!(player.speed(), base_speed)?;
    assert_result!(spawn_point.x + 1, player.position.x)?;
    assert_result!(spawn_point.y, player.position.y)
}

#[rustler::nif]
pub fn cant_attack_if_disarmed() -> TestResult {
    let now = time_utils::time_now();
    let mut selected_characters: HashMap<u64, Name> = HashMap::new();
    selected_characters.insert(1, Name::Muflus);
    let mut state = GameState::new(
        selected_characters,
        1,
        20,
        20,
        false,
        &read_character_config(),
        &read_skills_config(),
    )
    .unwrap();
    let player_1_id = 1;
    let player_2_id = 2;
    let base_speed = 1;
    let disarmed_char = Character {
        base_speed,
        ..Default::default()
    };
    let char: Character = speed1_character();
    let mut player1 = Player::new(player_1_id, 100, Position::new(0, 0), disarmed_char.clone());
    player1.effects.insert(Effect::Disarmed, EffectData{
        time_left: time_utils::MillisTime {
            high: 0,
            low: 2000
        },
        ends_at: time_utils::add_millis(now, time_utils::MillisTime {
            high: 0,
            low: 2000
        }), direction: None, position: None});
    let player2 = Player::new(player_2_id, 100, Position::new(0, 0), char.clone());
    state.players = vec![player1.clone(), player2.clone()];
    state.board.set_cell(0, 0, Tile::Player(player_1_id))?;
    state.board.set_cell(10, 10, Tile::Player(player_2_id))?;
    let player1_cooldown = player1.character.cooldown_basic_skill();
    let player2_cooldown = player2.character.cooldown_basic_skill();
    // make sure both abilities are off cooldown
    time_utils::sleep(player1_cooldown);
    time_utils::sleep(player2_cooldown);

    // Player 1 can't attack since it is in a Disarmed state
    // Player 2 can attack
    state
        .basic_attack(player_1_id, &RelativePosition::new(0f32, 1f32))
        .unwrap();
    state
        .basic_attack(player_2_id, &RelativePosition::new(0f32, 1f32))
        .unwrap();
    assert_result!(100, state.players[0].health)?;
    assert_result!(100, state.players[1].health)?;

    // Wait for the Disarmed state to finish
    for _ in 0..10 {
        state.world_tick().unwrap();
    }

    // make sure both abilities are off cooldown
    time_utils::sleep(player1_cooldown);
    time_utils::sleep(player2_cooldown);

    // Now Player 1 should be able to attack
    state
        .basic_attack(player_1_id, &RelativePosition::new(0f32, 1f32))
        .unwrap();
    state
        .basic_attack(player_2_id, &RelativePosition::new(0f32, 1f32))
        .unwrap();
    assert_result!(100, state.players[1].health)
}
