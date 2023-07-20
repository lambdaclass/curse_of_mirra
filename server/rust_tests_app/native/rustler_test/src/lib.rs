#[allow(dead_code, unused)]
#[deny(unused_must_use)]
pub mod game_state_test;
pub mod utils;
rustler::init!(
    "Elixir.TestNIFs",
    [
        game_state_test::no_move_if_beyond_boundaries,
        game_state_test::attacking,
        game_state_test::cant_move_if_petrified,
        game_state_test::cant_attack_if_disarmed
    ],
    load = gamestate::load
);
