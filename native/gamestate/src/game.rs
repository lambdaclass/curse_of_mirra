use rand::{thread_rng, Rng};
use rustler::{NifStruct, NifUnitEnum};
use std::collections::HashSet;

use crate::board::Board;
use crate::player::{Player, Position, Projectile, Status};
use crate::time_utils::time_now;

const ATTACK_COOLDOWN: u64 = 1;

#[derive(NifStruct)]
#[module = "DarkWorldsServer.Engine.Game"]
pub struct GameState {
    pub players: Vec<Player>,
    pub board: Board,
}

#[derive(Clone, Debug, NifUnitEnum)]
pub enum Direction {
    UP,
    DOWN,
    LEFT,
    RIGHT,
}

impl GameState {
    pub fn new(number_of_players: u64, board_width: usize, board_height: usize) -> Self {
        let mut positions = HashSet::new();
        let players: Vec<Player> = (1..number_of_players + 1)
            .map(|player_id| {
                let new_position = generate_new_position(&mut positions, board_width, board_height);
                Player::new(player_id, 100, new_position)
            })
            .collect();

        let grid = vec![vec![0; board_height]; board_width];
        let mut board = Board::new(grid, board_width, board_height);

        for player in players.clone() {
            board.set_cell(player.position.x, player.position.y, player.id);
        }

        Self { players, board }
    }

    pub fn move_player(&mut self, player_id: u64, direction: Direction) {
        let player = self
            .players
            .iter_mut()
            .find(|player| player.id == player_id)
            .unwrap();

        if player.projectile.is_some() {
            return;
        }

        if matches!(player.status, Status::DEAD) {
            return;
        }

        let new_position = compute_adjacent_position(&direction, &player.position);
        if !is_valid_movement(&self.board, &new_position) {
            return;
        }

        // Remove the player from their previous position on the board
        self.board.set_cell(player.position.x, player.position.y, 0);

        player.position = new_position;
        self.board
            .set_cell(player.position.x, player.position.y, player.id);
    }

    pub fn attack_player(&mut self, attacking_player_id: u64, attack_direction: Direction) {
        let attacking_player = self
            .players
            .iter_mut()
            .find(|player| player.id == attacking_player_id)
            .unwrap();

        if matches!(attacking_player.status, Status::DEAD) {
            return;
        }

        let now = time_now();

        if (now - attacking_player.last_melee_attack) < ATTACK_COOLDOWN {
            return;
        }
        attacking_player.last_melee_attack = now;

        let target_position =
            compute_adjacent_position(&attack_direction, &attacking_player.position);
        let maybe_target_cell = self.board.get_cell(target_position.x, target_position.y);

        if maybe_target_cell.is_none() {
            return;
        }

        if let Some(target_player) = self
            .players
            .iter_mut()
            .find(|player| player.id == *maybe_target_cell.unwrap())
        {
            modify_health(target_player, -10);
            let player = target_player.clone();
            self.modify_cell_if_player_died(&player);
        }
    }

    // Go over each player, check if they are inside the area. If they are, damage them
    pub fn explode_projectile(&mut self, projectile: &Projectile) {
        for player in self.players.iter_mut() {
            let distance = distance_to_center_squared(player, &projectile.position);
            if distance <= 2.0 {
                modify_health(player, -15);
                // let player = player.clone();
                // self.modify_cell_if_player_died(&player);
            }
        }
    }

    pub fn advance_or_create_projectile(
        &mut self,
        attacking_player_id: u64,
        direction: Direction,
    ) {
        let player = self
            .players
            .iter_mut()
            .find(|x| x.id == attacking_player_id)
            .unwrap();

        let now = time_now();

        if let Some(projectile) = &mut player.projectile {
            // projectile moves 1 unit per ATTACK_COOLDOWN secs, for convenience
            if now - projectile.movement_cooldown < ATTACK_COOLDOWN {
                return;
            }

            projectile.position =
                compute_adjacent_position(&projectile.direction, &projectile.position);
            projectile.movement_cooldown = now;
        } else {
            // only create projectile if attack cooldown has passed
            if (now - player.last_melee_attack) > ATTACK_COOLDOWN {
                player.projectile = Some(Projectile::new(player.position.clone(), direction));
                player.last_melee_attack = now;
            }
        }
    }

    fn remove_dead_players(&mut self) {
        self.players.iter_mut().for_each(|player| {
            if matches!(player.status, Status::DEAD) {
                self.board.set_cell(player.position.x, player.position.y, 0);
            }
        })
    }

    fn modify_cell_if_player_died(&mut self, player: &Player) {
        if matches!(player.status, Status::DEAD) {
            self.board.set_cell(player.position.x, player.position.y, 0);
        }
    }
}

fn modify_health(player: &mut Player, hp_points: i64) {
    if matches!(player.status, Status::ALIVE) {
        player.health += hp_points;
        if player.health <= 0 {
            player.status = Status::DEAD;
        }
    }
}
/// Given a position and a direction, returns the position adjacent to it in that direction.
/// Example: If the arguments are Direction::RIGHT and (0, 0), returns (0, 1).
fn compute_adjacent_position(direction: &Direction, position: &Position) -> Position {
    let x = position.x;
    let y = position.y;

    match direction {
        Direction::UP => Position::new(x.wrapping_sub(1), y),
        Direction::DOWN => Position::new(x + 1, y),
        Direction::LEFT => Position::new(x, y.wrapping_sub(1)),
        Direction::RIGHT => Position::new(x, y + 1),
    }
}

fn is_valid_movement(board: &Board, new_position: &Position) -> bool {
    let cell = board.get_cell(new_position.x, new_position.y);
    if cell.is_none() {
        return false;
    }

    // Check if cell is not-occupied
    // This unwrap is safe since we checked for None in the line above.
    if *cell.unwrap() != 0 {
        return false;
    }

    true
}

fn distance_to_center_squared(player: &Player, center: &Position) -> f64 {
    let distance_squared =
        (player.position.x - center.x).pow(2) + (player.position.y - center.y).pow(2);
    distance_squared as f64
}

fn generate_new_position(
    positions: &mut HashSet<(usize, usize)>,
    board_width: usize,
    board_height: usize,
) -> Position {
    let rng = &mut thread_rng();
    let mut x_coordinate: usize = rng.gen_range(0..board_width);
    let mut y_coordinate: usize = rng.gen_range(0..board_height);

    while positions.contains(&(x_coordinate, y_coordinate)) {
        x_coordinate = rng.gen_range(0..board_width);
        y_coordinate = rng.gen_range(0..board_height);
    }

    positions.insert((x_coordinate, y_coordinate));
    Position::new(x_coordinate, y_coordinate)
}

#[cfg(test)]
mod tests {
    use crate::game::ATTACK_COOLDOWN;
    use crate::player::Player;
    use crate::player::Position;
    use crate::time_utils;

    use super::Direction;
    use super::GameState;

    #[test]
    fn no_move_if_beyond_boundaries() {
        let mut expected_grid: Vec<Vec<u64>>;
        let mut state = GameState::new(1, 2, 2);
        let player_id = state.players.first().unwrap().id;

        // Check UP boundary
        state.move_player(player_id, Direction::UP);
        assert_eq!(0, state.players.first().unwrap().position.x);

        expected_grid = state.board.grid.clone();
        state.move_player(player_id, Direction::UP);
        assert_eq!(expected_grid, state.board.grid);

        // Check DOWN boundary
        state.move_player(player_id, Direction::DOWN);
        assert_eq!(1, state.players.first().unwrap().position.x);

        expected_grid = state.board.grid.clone();
        state.move_player(player_id, Direction::DOWN);
        assert_eq!(expected_grid, state.board.grid);

        // Check RIGHT boundary
        state.move_player(player_id, Direction::RIGHT);
        assert_eq!(1, state.players.first().unwrap().position.y);

        expected_grid = state.board.grid.clone();
        state.move_player(player_id, Direction::RIGHT);
        assert_eq!(expected_grid, state.board.grid);

        // Check LEFT boundary
        state.move_player(player_id, Direction::LEFT);
        assert_eq!(0, state.players.first().unwrap().position.y);

        expected_grid = state.board.grid.clone();
        state.move_player(player_id, Direction::LEFT);
        assert_eq!(expected_grid, state.board.grid);
    }

    #[test]
    fn no_move_if_occupied() {
        let mut state = GameState::new(2, 2, 2);
        let player1_id = 1;
        let player2_id = 2;
        let player1 = Player::new(player1_id, 100, Position::new(0, 0));
        let player2 = Player::new(player2_id, 100, Position::new(0, 1));
        state.players = vec![player1, player2];
        state.board.set_cell(0, 0, player1_id);
        state.board.set_cell(0, 1, player2_id);
        state.board.set_cell(1, 1, 0);
        state.board.set_cell(1, 0, 0);

        let expected_grid = state.board.grid.clone();
        state.move_player(player1_id, Direction::RIGHT);
        assert_eq!(expected_grid, state.board.grid);
    }

    #[test]
    fn movement() {
        let mut state = GameState::new(0, 2, 2);
        let player_id = 1;
        let player1 = Player::new(player_id, 100, Position::new(0, 0));
        state.players = vec![player1];
        state.board.set_cell(0, 0, player_id);

        state.move_player(player_id, Direction::RIGHT);
        assert_eq!(vec![vec![0, player_id], vec![0, 0]], state.board.grid);

        state.move_player(player_id, Direction::DOWN);
        assert_eq!(vec![vec![0, 0], vec![0, player_id]], state.board.grid);

        state.move_player(player_id, Direction::LEFT);
        assert_eq!(vec![vec![0, 0], vec![player_id, 0]], state.board.grid);

        state.move_player(player_id, Direction::UP);
        assert_eq!(vec![vec![player_id, 0], vec![0, 0]], state.board.grid);
    }

    #[test]
    fn attacking() {
        let mut state = GameState::new(0, 2, 2);
        let player_1_id = 1;
        let player_2_id = 2;
        let player1 = Player::new(player_1_id, 100, Position::new(0, 0));
        let player2 = Player::new(player_2_id, 100, Position::new(0, 0));
        state.players = vec![player1, player2];
        state.board.set_cell(0, 0, player_1_id);
        state.board.set_cell(0, 1, player_2_id);

        time_utils::sleep(ATTACK_COOLDOWN);

        // Attack lands and damages player
        state.attack_player(player_1_id, Direction::RIGHT);
        assert_eq!(100, state.players[0].health);
        assert_eq!(90, state.players[1].health);

        // Attack does nothing because of cooldown
        state.attack_player(player_1_id, Direction::RIGHT);
        assert_eq!(100, state.players[0].health);
        assert_eq!(90, state.players[1].health);

        time_utils::sleep(ATTACK_COOLDOWN);

        // Attack misses and does nothing
        state.attack_player(player_1_id, Direction::DOWN);
        assert_eq!(100, state.players[0].health);
        assert_eq!(90, state.players[1].health);

        time_utils::sleep(ATTACK_COOLDOWN);

        state.move_player(player_1_id, Direction::DOWN);

        // Attacking to the right now does nothing since the player moved down.
        state.attack_player(player_1_id, Direction::RIGHT);
        assert_eq!(100, state.players[0].health);
        assert_eq!(90, state.players[1].health);

        time_utils::sleep(ATTACK_COOLDOWN);

        // Attacking to a non-existent position on the board does nothing.
        state.attack_player(player_1_id, Direction::LEFT);
        assert_eq!(100, state.players[0].health);
        assert_eq!(90, state.players[1].health);
    }
}
