use rand::{thread_rng, Rng};
use rustler::{NifStruct, NifUnitEnum};
use std::collections::HashSet;

use crate::board::{Board, Tile};
use crate::player::{Player, Position, Status};
use crate::time_utils::time_now;

pub const MELEE_ATTACK_COOLDOWN: u64 = 1;

#[derive(NifStruct)]
#[module = "DarkWorldsServer.Engine.Game"]
pub struct GameState {
    pub players: Vec<Player>,
    pub board: Board,
}

#[derive(Debug, NifUnitEnum)]
pub enum Direction {
    UP,
    DOWN,
    LEFT,
    RIGHT,
}

impl GameState {
    pub fn new(
        number_of_players: u64,
        board_width: usize,
        board_height: usize,
        build_walls: bool,
    ) -> Self {
        let mut positions = HashSet::new();
        let players: Vec<Player> = (1..number_of_players + 1)
            .map(|player_id| {
                let new_position = generate_new_position(&mut positions, board_width, board_height);
                Player::new(player_id, 100, new_position)
            })
            .collect();

        let mut board = Board::new(board_width, board_height);

        for player in players.clone() {
            board.set_cell(
                player.position.x,
                player.position.y,
                Tile::Player(player.id),
            );
        }

        // We generate 10 random walls if walls is true
        if build_walls {
            for _ in 1..=10 {
                let rng = &mut thread_rng();
                let row_idx: usize = rng.gen_range(0..board_width);
                let col_idx: usize = rng.gen_range(0..board_height);
                if let Some(Tile::Empty) = board.get_cell(row_idx, col_idx) {
                    board.set_cell(row_idx, col_idx, Tile::Wall);
                }
            }
        }

        Self { players, board }
    }

    pub fn move_player(self: &mut Self, player_id: u64, direction: Direction, speed: usize) {
        let player = self
            .players
            .iter_mut()
            .find(|player| player.id == player_id)
            .unwrap();

        if matches!(player.status, Status::DEAD) {
            return;
        }

        let new_position = compute_adjacent_position_n_tiles(&direction, &player.position, speed);
        if !is_valid_movement(&self.board, &new_position) {
            return;
        }

        // Remove the player from their previous position on the board
        self.board
            .set_cell(player.position.x, player.position.y, Tile::Empty);

        player.position = new_position;
        self.board.set_cell(
            player.position.x,
            player.position.y,
            Tile::Player(player.id),
        );
    }

    pub fn attack_player(self: &mut Self, attacking_player_id: u64, attack_direction: Direction) {
        let attacking_player = self
            .players
            .iter_mut()
            .find(|player| player.id == attacking_player_id)
            .unwrap();
        if matches!(attacking_player.status, Status::DEAD) {
            return;
        }

        let now = time_now();

        if (now - attacking_player.last_melee_attack) < MELEE_ATTACK_COOLDOWN {
            return;
        }
        attacking_player.last_melee_attack = now;

        let target_position =
            compute_adjacent_position_n_tiles(&attack_direction, &attacking_player.position, 1);
        let maybe_target_cell = self.board.get_cell(target_position.x, target_position.y);

        // If the cell is not on range, or the attacking player is on the receiving end
        // of the attack, do nothing.
        if maybe_target_cell.is_none() || attacking_player.position == target_position {
            return;
        }

        if let Some(target_player) = self.players.iter_mut().find(|player| {
            let tile = maybe_target_cell.clone().unwrap();
            match tile {
                Tile::Player(tile_player_id) if tile_player_id == player.id => true,
                _ => false,
            }
        }) {
            target_player.modify_health(-10);
            let player = target_player.clone();
            self.modify_cell_if_player_died(&player);
        }
    }

    // Go over each player, check if they are inside the circle. If they are, damage them according
    // to their distance to the center.
    pub fn attack_aoe(self: &mut Self, attacking_player_id: u64, center_of_attack: &Position) {
        for player in self.players.iter_mut() {
            if player.id == attacking_player_id {
                continue;
            }

            let distance = distance_to_center(player, center_of_attack);
            if distance < 3.0 {
                let damage = (((3.0 - distance) / 3.0) * 10.0) as i64;
                player.modify_health(-damage);
            }
        }
    }

    pub fn disconnect(self: &mut Self, player_id: u64) -> Result<(), String> {
        if let Some(player) = self.players.get_mut((player_id - 1) as usize) {
            player.status = Status::DISCONNECTED;
            Ok(())
        } else {
            Err(format!("Player not found with id: {}", player_id))
        }
    }

    fn remove_dead_players(self: &mut Self) {
        self.players.iter_mut().for_each(|player| {
            if matches!(player.status, Status::DEAD) {
                self.board
                    .set_cell(player.position.x, player.position.y, Tile::Empty);
            }
        })
    }

    fn modify_cell_if_player_died(self: &mut Self, player: &Player) {
        if matches!(player.status, Status::DEAD) {
            self.board
                .set_cell(player.position.x, player.position.y, Tile::Empty);
        }
    }
}

/// Given a position and a direction, returns the position adjacent to it `n` tiles
/// in that direction
/// Example: If the arguments are Direction::RIGHT, (0, 0) and 2, returns (0, 2).
fn compute_adjacent_position_n_tiles(
    direction: &Direction,
    position: &Position,
    n: usize,
) -> Position {
    let x = position.x;
    let y = position.y;

    // Avoid overflow with saturated ops.
    match direction {
        Direction::UP => Position::new(x.wrapping_sub(n), y),
        Direction::DOWN => Position::new(x + n, y),
        Direction::LEFT => Position::new(x, y.wrapping_sub(n)),
        Direction::RIGHT => Position::new(x, y + n),
    }
}

fn is_valid_movement(board: &Board, new_position: &Position) -> bool {
    let cell = board.get_cell(new_position.x, new_position.y);
    if cell.is_none() {
        return false;
    }

    // Check if cell is not-occupied
    // This unwrap is safe since we checked for None in the line above.
    if let Tile::Empty = cell.unwrap() {
        true
    } else {
        false
    }
}

fn distance_to_center(player: &Player, center: &Position) -> f64 {
    let distance_squared =
        (player.position.x - center.x).pow(2) + (player.position.y - center.y).pow(2);
    (distance_squared as f64).sqrt()
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
