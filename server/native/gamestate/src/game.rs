use rand::{thread_rng, Rng};
use rustler::{NifStruct, NifUnitEnum};
use std::collections::HashSet;

use crate::board::{Board, Tile};
use crate::character::Character;
use crate::player::{Player, PlayerAction, Position, RelativePosition, Status};
use crate::projectile::{JoystickValues, Projectile, ProjectileStatus, ProjectileType};
use crate::time_utils::time_now;
use std::cmp::{max, min};

#[derive(NifStruct)]
#[module = "DarkWorldsServer.Engine.Game"]
pub struct GameState {
    pub players: Vec<Player>,
    pub board: Board,
    pub projectiles: Vec<Projectile>,
    pub next_projectile_id: u64,
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
        let characters = [Default::default(), Character::muflus(), Character::uma()];
        let players: Vec<Player> = (1..number_of_players + 1)
            .map(|player_id| {
                let new_position = generate_new_position(&mut positions, board_width, board_height);
                Player::new(
                    player_id,
                    100,
                    new_position,
                    characters[player_id as usize % characters.len()].clone(),
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

        let projectiles = Vec::new();

        Self {
            players,
            board,
            projectiles,
            next_projectile_id: 0,
        }
    }

    pub fn new_round(self: &mut Self, players: Vec<Player>) {
        let mut positions = HashSet::new();
        let mut players: Vec<Player> = players;

        let mut board = Board::new(self.board.width, self.board.height);

        for player in players.iter_mut() {
            let new_position =
                generate_new_position(&mut positions, self.board.width, self.board.height);
            player.position.x = new_position.x;
            player.position.y = new_position.y;
            player.health = 100;
            player.status = Status::ALIVE;
            board.set_cell(
                player.position.x,
                player.position.y,
                Tile::Player(player.id),
            );
        }

        self.players = players;
        self.board = board;
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
            player.character.speed() as usize,
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

    pub fn move_player_to_coordinates(self: &mut Self, player_id: u64, mut new_position: Position) {
        let player = self
            .players
            .iter_mut()
            .find(|player| player.id == player_id)
            .unwrap();

        if matches!(player.status, Status::DEAD) {
            return;
        }

        // These changes are done so that if the player is moving into one of the map's borders
        // but is not already on the edge, they move to the edge. In simpler terms, if the player is
        // trying to move from (0, 1) to the left, this ensures that new_position is (0, 0) instead of
        // something invalid like (0, -1).
        new_position.x = min(new_position.x, self.board.height - 1);
        new_position.x = max(new_position.x, 0);
        new_position.y = min(new_position.y, self.board.width - 1);
        new_position.y = max(new_position.y, 0);

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

        let new_position = new_entity_position(
            self.board.height,
            self.board.width,
            x,
            y,
            player.position,
            player.character.speed() as i64,
        );

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

    pub fn get_player_mut(
        players: &mut Vec<Player>,
        player_id: u64,
    ) -> Result<&mut Player, String> {
        players
            .iter_mut()
            .find(|player| player.id == player_id)
            .ok_or(format!("Given id ({player_id}) is not valid"))
    }

    pub fn get_player(self: &Self, player_id: u64) -> Result<Player, String> {
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

        let mut affected_players: Vec<u64> =
            GameState::players_in_range(&self.board, top_left, bottom_right)
                .into_iter()
                .filter(|&id| id != attacking_player_id)
                .collect();

        let mut kill_count = 0;
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
                    if matches!(player.status, Status::DEAD) {
                        kill_count += 1;
                    }
                    GameState::modify_cell_if_player_died(&mut self.board, &player);
                }
                _ => continue,
            }
        }

        add_kills(&mut self.players, attacking_player_id, kill_count).expect("Player not found");
    }

    // Return all player_id inside an area
    pub fn players_in_range(board: &Board, top_left: Position, bottom_right: Position) -> Vec<u64> {
        let mut players: Vec<u64> = vec![];
        for fil in top_left.x..=bottom_right.x {
            for col in top_left.y..=bottom_right.y {
                let cell = board.get_cell(fil, col);
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

    pub fn aoe_attack(
        self: &mut Self,
        attacking_player_id: u64,
        attack_position: &RelativePosition,
    ) -> Result<(), String> {
        let attacking_player = GameState::get_player_mut(&mut self.players, attacking_player_id)?;

        if attacking_player_id % 2 == 0 {
            attacking_player.action = PlayerAction::ATTACKINGAOE;

            let cooldown = attacking_player.character.cooldown();

            if matches!(attacking_player.status, Status::DEAD) {
                return Ok(());
            }

            let now = time_now();

            if (now - attacking_player.last_melee_attack) < cooldown {
                return Ok(());
            }

            let (center, top_left, bottom_right) =
                compute_attack_aoe_initial_positions(&(attacking_player.position), attack_position);
            attacking_player.last_melee_attack = now;
            attacking_player.aoe_position = center;

            let affected_players: Vec<u64> =
                GameState::players_in_range(&self.board, top_left, bottom_right)
                    .into_iter()
                    .filter(|&id| id != attacking_player_id)
                    .collect();

            let special_effect = attacking_player.character.select_aoe_effect();

            let mut kill_count = 0;
            for target_player_id in affected_players {
                let attacked_player =
                    GameState::get_player_mut(&mut self.players, target_player_id)?;
                if let Some((effect, duration)) = &special_effect {
                    attacked_player
                        .character
                        .add_effect(effect.clone(), *duration)
                } else {
                    // Maybe health should be linked to
                    // the character instead?
                    attacked_player.modify_health(-10);
                    if matches!(attacked_player.status, Status::DEAD) {
                        kill_count += 1;
                    }
                    GameState::modify_cell_if_player_died(&mut self.board, attacked_player);
                }
            }

            add_kills(&mut self.players, attacking_player_id, kill_count)
                .expect("Player not found");
        } else {
            let attacking_player =
                GameState::get_player_mut(&mut self.players, attacking_player_id)?;
            if attack_position.x != 0 || attack_position.y != 0 {
                let projectile = Projectile::new(
                    self.next_projectile_id,
                    attacking_player.position,
                    JoystickValues::new(attack_position.x as f64, attack_position.y as f64),
                    5,
                    10,
                    attacking_player.id,
                    20,
                    30,
                    ProjectileType::BULLET,
                    ProjectileStatus::ACTIVE,
                );
                self.projectiles.push(projectile);
                self.next_projectile_id += 1;
            }
        }

        Ok(())
    }

    pub fn disconnect(self: &mut Self, player_id: u64) -> Result<(), String> {
        if let Some(player) = self.players.get_mut((player_id - 1) as usize) {
            player.status = Status::DISCONNECTED;
            Ok(())
        } else {
            Err(format!("Player not found with id: {}", player_id))
        }
    }

    pub fn world_tick(self: &mut Self) -> Result<(), String> {
        self.players.iter_mut().for_each(|player| {
            // Clean each player actions
            player.action = PlayerAction::NOTHING;
            // Keep only (de)buffs that have
            // a non-zero amount of ticks left.
            player.character.status_effects.retain(|_, ticks_left| {
                *ticks_left = ticks_left.saturating_sub(1);
                *ticks_left != 0
            });
        });

        self.projectiles.iter_mut().for_each(|projectile| {
            projectile.position = new_entity_position(
                self.board.height,
                self.board.width,
                projectile.direction.x,
                projectile.direction.y,
                projectile.position,
                projectile.speed as i64,
            );
            projectile.remaining_ticks = projectile.remaining_ticks.saturating_sub(1);
        });

        self.projectiles
            .retain(|projectile| projectile.remaining_ticks > 0);

        self.projectiles.iter_mut().for_each(|projectile| {
            if projectile.status == ProjectileStatus::ACTIVE {
                let top_left = Position::new(
                    projectile
                        .position
                        .x
                        .saturating_sub(projectile.range as usize),
                    projectile
                        .position
                        .y
                        .saturating_sub(projectile.range as usize),
                );
                let bottom_right = Position::new(
                    projectile.position.x + projectile.range as usize,
                    projectile.position.y + projectile.range as usize,
                );

                let affected_players: Vec<u64> =
                    GameState::players_in_range(&self.board, top_left, bottom_right)
                        .into_iter()
                        .filter(|&id| id != projectile.player_id)
                        .collect();

                if affected_players.len() > 0 {
                    projectile.status = ProjectileStatus::EXPLODED;
                }

                let mut kill_count = 0;
                for target_player_id in affected_players {
                    let attacked_player =
                        GameState::get_player_mut(&mut self.players, target_player_id);
                    match attacked_player {
                        Ok(ap) => {
                            ap.modify_health(-(projectile.damage as i64));
                            if matches!(ap.status, Status::DEAD) {
                                kill_count += 1;
                            }
                            GameState::modify_cell_if_player_died(&mut self.board, ap);
                        }
                        _ => continue,
                    }
                }

                add_kills(&mut self.players, projectile.player_id, kill_count)
                    .expect("Player not found");
            }
        });

        Ok(())
    }

    fn modify_cell_if_player_died(board: &mut Board, player: &Player) {
        if matches!(player.status, Status::DEAD) {
            board.set_cell(player.position.x, player.position.y, Tile::Empty);
        }
    }

    pub fn spawn_player(self: &mut Self, player_id: u64) {
        let mut tried_positions = HashSet::new();
        let mut position: Position;

        loop {
            position =
                generate_new_position(&mut tried_positions, self.board.width, self.board.height);
            if let Some(Tile::Empty) = self.board.get_cell(position.x, position.y) {
                break;
            }
        }

        self.board
            .set_cell(position.x, position.y, Tile::Player(player_id));
        self.players
            .push(Player::new(player_id, 100, position, Default::default()));
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

    let x =
        (player_position.x as f64 + modifier * (-(attack_position.y) as f64) / 100_f64) as usize;
    let y = (player_position.y as f64 + modifier * (attack_position.x as f64) / 100_f64) as usize;

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

#[allow(dead_code)]
fn distance_to_center(player: &Player, center: &Position) -> f64 {
    let distance_squared =
        (player.position.x - center.x).pow(2) + (player.position.y - center.y).pow(2);
    (distance_squared as f64).sqrt()
}

// We might want to abstract this into a Vector2 type or something, whatever.
fn normalize_vector(x: f64, y: f64) -> (f64, f64) {
    let norm = f64::sqrt(x.powf(2.) + y.powf(2.));
    (x / norm, y / norm)
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

pub fn new_entity_position(
    height: usize,
    width: usize,
    direction_x: f64,
    direction_y: f64,
    entity_position: Position,
    entity_speed: i64,
) -> Position {
    let Position { x: old_x, y: old_y } = entity_position;
    let speed = entity_speed as i64;

    /*
        We take the joystick coordinates, normalize the vector, then multiply by speed,
        then round the values.
    */
    let (movement_direction_x, movement_direction_y) = normalize_vector(-direction_y, direction_x);
    let movement_vector_x = movement_direction_x * (speed as f64);
    let movement_vector_y = movement_direction_y * (speed as f64);

    let mut new_position_x = old_x as i64 + (movement_vector_x.round() as i64);
    let mut new_position_y = old_y as i64 + (movement_vector_y.round() as i64);

    new_position_x = min(new_position_x, (height - 1) as i64);
    new_position_x = max(new_position_x, 0);
    new_position_y = min(new_position_y, (width - 1) as i64);
    new_position_y = max(new_position_y, 0);

    let new_position = Position {
        x: new_position_x as usize,
        y: new_position_y as usize,
    };
    new_position
}

fn add_kills(
    players: &mut Vec<Player>,
    attacking_player_id: u64,
    kills: u64,
) -> Result<(), String> {
    let attacking_player = GameState::get_player_mut(players, attacking_player_id)?;
    attacking_player.add_kills(kills);
    Ok(())
}
