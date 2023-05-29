use rand::{thread_rng, Rng};
use rustler::{NifStruct, NifUnitEnum};
use std::collections::HashSet;

use crate::board::{Board, Tile};
use crate::character::Character;
use crate::player::{Player, PlayerAction, Position, RelativePosition, Status};
use crate::skills::{BasicSkill, Class};
use crate::time_utils::time_now;
use std::cmp::{max, min};

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
        let characters = [
            Default::default(),
            Character {
                class: Class::Guardian,
                basic_skill: BasicSkill::Bash,
                speed: 3,
                name: "Guardian".to_string(),
            },
        ];
        let players: Vec<Player> = (1..number_of_players + 1)
            .map(|player_id| {
                let new_position = generate_new_position(&mut positions, board_width, board_height);
                Player::new(
                    player_id,
                    100,
                    new_position,
                    characters[(player_id % 2) as usize].clone(),
                )
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

    pub fn move_player(self: &mut Self, player_id: u64, direction: Direction) {
        let player = self
            .players
            .iter_mut()
            .find(|player| player.id == player_id)
            .unwrap();

        if matches!(player.status, Status::DEAD) {
            return;
        }

        let mut new_position = compute_adjacent_position_n_tiles(
            &direction,
            &player.position,
            player.character.speed as usize,
        );

        // These changes are done so that if the player is moving into one of the map's borders
        // but is not already on the edge, they move to the edge. In simpler terms, if the player is
        // trying to move from (0, 1) to the left, this ensures that new_position is (0, 0) instead of
        // something invalid like (0, -1).
        new_position.x = min(new_position.x, self.board.height - 1);
        new_position.x = max(new_position.x, 0);
        new_position.y = min(new_position.y, self.board.width - 1);
        new_position.y = max(new_position.y, 0);

        // let tile_to_move_to = tile_to_move_to(&self.board, &player.position, &new_position);

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

    // Takes the raw value from Unity's joystick
    // and calculates the resulting position on the grid.
    // The joystick values are 2 floating point numbers,
    // x and y which are translated to the character's delta
    // into a certain grid direction. A conversion with rounding
    // to the nearest integer is done to obtain the grid coordinates.
    // The (x,y) input value becomes:
    // (-1*rounded_nearest_integer(y), round_nearest_integer(x)).
    // Eg: If the input is (-0.7069376, 0.7072759) (a left-upper joystick movement) the movement on the grid
    // becomes (-1, -1). Because the input is a left-upper joystick movement
    pub fn move_with_joystick(
        self: &mut Self,
        player_id: u64,
        x: f64,
        y: f64,
    ) -> Result<(), String> {
        let player = Self::get_player_mut(&mut self.players, player_id)?;
        if matches!(player.status, Status::DEAD) {
            return Ok(());
        }
        let Position { x: old_x, y: old_y } = player.position;
        let speed = player.character.speed as i64;
        let (x_grid_delta, y_grid_delta) = Self::joystick_axis_to_grid_coords(x, y);
        let mut new_position = Position {
            x: (old_x as i64 + (x_grid_delta * speed)) as usize,
            y: (old_y as i64 + (y_grid_delta * speed)) as usize,
        };

        new_position.x = min(new_position.x, self.board.height - 1);
        new_position.x = max(new_position.x, 0);
        new_position.y = min(new_position.y, self.board.width - 1);
        new_position.y = max(new_position.y, 0);
        self.board
            .set_cell(player.position.x, player.position.y, Tile::Empty);

        player.position = new_position;

        self.board.set_cell(
            player.position.x,
            player.position.y,
            Tile::Player(player.id),
        );
        Ok(())
    }
    // Maps joystick axis (which is like a common x-y axis) to
    // matrix coords.
    fn joystick_axis_to_grid_coords(joystick_x: f64, joystick_y: f64) -> (i64, i64) {
        let grid_x = -(joystick_y.round() as i64);
        let grid_y = joystick_x.round() as i64;
        return (grid_x, grid_y);
    }
    fn get_player_mut(players: &mut Vec<Player>, player_id: u64) -> Result<&mut Player, String> {
        players
            .get_mut((player_id - 1) as usize)
            .ok_or(format!("Given id ({player_id}) is not valid"))
    }
    fn get_player(self: &Self, player_id: u64) -> Result<Player, String> {
        self.players
            .get((player_id - 1) as usize)
            .ok_or(format!("Given id ({player_id}) is not valid"))
            .cloned()
    }
    pub fn attack_player(self: &mut Self, attacking_player_id: u64, attack_direction: Direction) {
        let attacking_player = self
            .players
            .iter_mut()
            .find(|player| player.id == attacking_player_id)
            .unwrap();
        let attack_dmg = attacking_player.character.attack_dmg() as i64;

        let cooldown = attacking_player.character.cooldown();

        if matches!(attacking_player.status, Status::DEAD) {
            return;
        }

        let now = time_now();

        if (now - attacking_player.last_melee_attack) < cooldown {
            return;
        }
        attacking_player.action = PlayerAction::ATTACKING;

        attacking_player.last_melee_attack = now;

        let (top_left, bottom_right) =
            compute_attack_initial_positions(&(attack_direction), &(attacking_player.position));

        let mut affected_players: Vec<u64> = self.players_in_range(top_left, bottom_right);

        for target_player_id in affected_players.iter_mut() {
            // FIXME: This is not ok, we should save referencies to the Game Players this is redundant
            let attacked_player = self
                .players
                .iter_mut()
                .find(|player| player.id == *target_player_id && player.id != attacking_player_id);

            match attacked_player {
                Some(ap) => {
                    ap.modify_health(-attack_dmg);
                    let player = ap.clone();
                    self.modify_cell_if_player_died(&player);
                }
                _ => continue,
            }
        }
    }

    // Return all player_id inside an area
    pub fn players_in_range(
        self: &mut Self,
        top_left: Position,
        bottom_right: Position,
    ) -> Vec<u64> {
        let mut players: Vec<u64> = vec![];
        for fil in top_left.x..=bottom_right.x {
            for col in top_left.y..=bottom_right.y {
                let cell = self.board.get_cell(fil, col);
                if cell.is_none() {
                    continue;
                }
                match cell.unwrap() {
                    Tile::Player(player_id) => {
                        players.push(player_id);
                    }
                    _ => continue,
                }
            }
        }
        players
    }

    pub fn attack_aoe(
        self: &mut Self,
        attacking_player_id: u64,
        attack_position: &RelativePosition,
    ) {
        let attacking_player = self
            .players
            .iter_mut()
            .find(|player| player.id == attacking_player_id)
            .unwrap();
        attacking_player.action = PlayerAction::ATTACKINGAOE;

        let cooldown = attacking_player.character.cooldown();

        if matches!(attacking_player.status, Status::DEAD) {
            return;
        }

        let now = time_now();

        if (now - attacking_player.last_melee_attack) < cooldown {
            return;
        }

        let (center, top_left, bottom_right) =
            compute_attack_aoe_initial_positions(&(attacking_player.position), attack_position);
        attacking_player.last_melee_attack = now;
        attacking_player.aoe_position = center;

        let mut affected_players: Vec<u64> = self.players_in_range(top_left, bottom_right);

        for target_player_id in affected_players.iter_mut() {
            // FIXME: This is not ok, we should save referencies to the Game Players this is redundant
            let attacked_player = self
                .players
                .iter_mut()
                .find(|player| player.id == *target_player_id && player.id != attacking_player_id);

            match attacked_player {
                Some(ap) => {
                    ap.modify_health(-10);
                    let player = ap.clone();
                    self.modify_cell_if_player_died(&player);
                }
                _ => continue,
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

    pub fn clean_players_actions(self: &mut Self) {
        self.players.iter_mut().for_each(|player| {
            player.action = PlayerAction::NOTHING;
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
        Direction::UP => Position::new(x.saturating_sub(n), y),
        Direction::DOWN => Position::new(x + n, y),
        Direction::LEFT => Position::new(x, y.saturating_sub(n)),
        Direction::RIGHT => Position::new(x, y + n),
    }
}

fn compute_attack_initial_positions(
    direction: &Direction,
    position: &Position,
) -> (Position, Position) {
    let x = position.x;
    let y = position.y;

    match direction {
        Direction::UP => (
            Position::new(x.saturating_sub(20), y.saturating_sub(20)),
            Position::new(x.saturating_sub(1), y + 20),
        ),
        Direction::DOWN => (
            Position::new(x + 1, y.saturating_sub(20)),
            Position::new(x + 20, y + 20),
        ),
        Direction::LEFT => (
            Position::new(x.saturating_sub(20), y.saturating_sub(20)),
            Position::new(x + 20, y.saturating_sub(1)),
        ),
        Direction::RIGHT => (
            Position::new(x.saturating_sub(20), y + 1),
            Position::new(x + 20, y + 20),
        ),
    }
}

fn compute_attack_aoe_initial_positions(
    player_position: &Position,
    attack_position: &RelativePosition,
) -> (Position, Position, Position) {
    let modifier = 120_f64;

    let x = (player_position.x as f64 + modifier * (attack_position.x as f64) / 100_f64) as usize;
    let y = (player_position.y as f64 + modifier * (attack_position.y as f64) / 100_f64) as usize;

    (
        Position::new(x, y),
        Position::new(x.saturating_sub(25), y.saturating_sub(25)),
        Position::new(x + 25, y + 25),
    )
}

/// TODO: update documentation
/// Checks if the given movement from `old_position` to `new_position` is valid.
/// The way we do it is separated into cases but the idea is always the same:
/// First of all check that we are not trying to move away from the board.
/// Then go through the tiles that are between the new_position and the old_position
/// and ensure that each one of them is empty. If that's not the case, the movement is
/// invalid; otherwise it's valid.
/// The cases that we separate the check into are the following:
/// - Movement is in the Y direction. This is divided into two other cases:
///     - Movement increases the Y coordinate (new_position.y > old_position.y).
///     - Movement decreases the Y coordinate (new_position.y < old_position.y).
/// - Movement is in the X direction. This is also divided into two cases:
///     - Movement increases the X coordinate (new_position.x > old_position.x).
///     - Movement decreases the X coordinate (new_position.x < old_position.x).
// fn tile_to_move_to(board: &Board, old_position: &Position, new_position: &Position) -> Position {
//     let mut number_of_cells_to_move = 0;

//     if new_position.x == old_position.x {
//         if new_position.y > old_position.y {
//             for i in 1..(new_position.y - old_position.y) + 1 {
//                 let cell = board.get_cell(old_position.x, old_position.y + i);

//                 match cell {
//                     Some(Tile::Empty) => {
//                         number_of_cells_to_move += 1;
//                         continue;
//                     }
//                     None => continue,
//                     Some(_) => {
//                         return Position {
//                             x: old_position.x,
//                             y: old_position.y + number_of_cells_to_move,
//                         };
//                     }
//                 }
//             }
//             return Position {
//                 x: old_position.x,
//                 y: old_position.y + number_of_cells_to_move,
//             };
//         } else {
//             for i in 1..(old_position.y - new_position.y) + 1 {
//                 let cell = board.get_cell(old_position.x, old_position.y - i);

//                 match cell {
//                     Some(Tile::Empty) => {
//                         number_of_cells_to_move += 1;
//                         continue;
//                     }
//                     None => continue,
//                     Some(_) => {
//                         return Position {
//                             x: old_position.x,
//                             y: old_position.y - number_of_cells_to_move,
//                         };
//                     }
//                 }
//             }
//             return Position {
//                 x: old_position.x,
//                 y: old_position.y - number_of_cells_to_move,
//             };
//         }
//     } else {
//         if new_position.x > old_position.x {
//             for i in 1..(new_position.x - old_position.x) + 1 {
//                 let cell = board.get_cell(old_position.x + i, old_position.y);

//                 match cell {
//                     Some(Tile::Empty) => {
//                         number_of_cells_to_move += 1;
//                         continue;
//                     }
//                     None => continue,
//                     Some(_) => {
//                         return Position {
//                             x: old_position.x + number_of_cells_to_move,
//                             y: old_position.y,
//                         }
//                     }
//                 }
//             }
//             return Position {
//                 x: old_position.x + number_of_cells_to_move,
//                 y: old_position.y,
//             };
//         } else {
//             for i in 1..(old_position.x - new_position.x) + 1 {
//                 let cell = board.get_cell(old_position.x - i, old_position.y);

//                 match cell {
//                     Some(Tile::Empty) => {
//                         number_of_cells_to_move += 1;
//                         continue;
//                     }
//                     None => continue,
//                     Some(_) => {
//                         return Position {
//                             x: old_position.x - number_of_cells_to_move,
//                             y: old_position.y,
//                         }
//                     }
//                 }
//             }
//             return Position {
//                 x: old_position.x - number_of_cells_to_move,
//                 y: old_position.y,
//             };
//         }
//     }
// }

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
