use rand::{thread_rng, Rng};
use rustler::{NifStruct, NifUnitEnum};
use std::f32::consts::PI;

use crate::board::{Board, Tile};
use crate::character::{Character, Effect, Name};
use crate::player::{Player, PlayerAction, Position, RelativePosition, Status};
use crate::projectile::{JoystickValues, Projectile, ProjectileStatus, ProjectileType};
use crate::time_utils::time_now;
use std::cmp::{max, min};
use std::collections::HashMap;
use std::collections::HashSet;
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
    fn build_characters_with_config(
        character_config: &[HashMap<String, String>],
    ) -> Result<Vec<Character>, String> {
        character_config
            .into_iter()
            // Keep only characters
            // for which active is 1.
            .filter(|map| {
                let active = map
                    .get("Active")
                    .expect("Missing Active key for character")
                    .parse::<u64>()
                    .expect("Expected 1 or 0 for Active key");
                active == 1
            })
            .map(Character::from_config_map)
            .collect()
    }

    pub fn new(
        selected_characters: HashMap<u64, Name>,
        number_of_players: u64,
        board_width: usize,
        board_height: usize,
        build_walls: bool,
        characters_config: &[HashMap<String, String>],
    ) -> Result<Self, String> {
        let mut positions = HashSet::new();
        let characters = GameState::build_characters_with_config(&characters_config)?;
        let players: Vec<Player> = (1..number_of_players + 1)
            .map(|player_id| -> Result<Player, String> {
                let new_position = generate_new_position(&mut positions, board_width, board_height);

                let selected_character = selected_characters.get(&player_id).unwrap().clone();

                let character = characters
                    .iter()
                    .find(|x| x.name == selected_character)
                    .ok_or("Can't get the character")?
                    .clone();

                Ok(Player::new(player_id, 100, new_position, character))
            })
            .collect::<Result<Vec<Player>, String>>()?;

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

        Ok(Self {
            players,
            board,
            projectiles,
            next_projectile_id: 0,
        })
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

    pub fn move_player_to_coordinates(
        board: &mut Board,
        attacking_player: &mut Player,
        direction: &RelativePosition,
    ) -> Result<(), String> {
        let new_position_x = attacking_player.position.x as i64 - direction.y;
        let new_position_y = attacking_player.position.y as i64 + direction.x;

        // These changes are done so that if the player is moving into one of the map's borders
        // but is not already on the edge, they move to the edge. In simpler terms, if the player is
        // trying to move from (0, 1) to the left, this ensures that new_position is (0, 0) instead of
        // something invalid like (0, -1).

        let new_position_x = min(new_position_x, (board.height - 1).try_into().unwrap());
        let new_position_x = max(new_position_x, 0);
        let new_position_y = min(new_position_y, (board.height - 1).try_into().unwrap());
        let new_position_y = max(new_position_y, 0);

        let new_position_coordinates = Position {
            x: new_position_x as usize,
            y: new_position_y as usize,
        };
        // Remove the player from their previous position on the board
        board.set_cell(
            attacking_player.position.x,
            attacking_player.position.y,
            Tile::Empty,
        );
        attacking_player.position = new_position_coordinates;
        attacking_player.action = PlayerAction::TELEPORTING;

        board.set_cell(
            attacking_player.position.x,
            attacking_player.position.y,
            Tile::Player(attacking_player.id),
        );

        Ok(())
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
        x: f32,
        y: f32,
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

    pub fn basic_attack(
        self: &mut Self,
        attacking_player_id: u64,
        direction: &RelativePosition,
    ) -> Result<(), String> {
        let attacking_player = GameState::get_player_mut(&mut self.players, attacking_player_id)?;

        if !attacking_player.can_attack(attacking_player.basic_skill_cooldown_left) {
            return Ok(());
        }

        let now = time_now();
        attacking_player.last_melee_attack = now;
        attacking_player.action = PlayerAction::ATTACKING;
        attacking_player.basic_skill_cooldown_start = now;
        attacking_player.basic_skill_cooldown_left =
            attacking_player.character.cooldown_basic_skill();

        match attacking_player.character.name {
            Name::H4ck => Self::h4ck_basic_attack(
                &attacking_player,
                direction,
                &mut self.projectiles,
                &mut self.next_projectile_id,
            ),
            Name::Muflus => {
                let attacking_player = GameState::get_player(&self, attacking_player_id)?;
                let players = &mut self.players;
                Self::muflus_basic_attack(&mut self.board, players, &attacking_player, direction)
            }
            Name::Uma => Self::h4ck_basic_attack(
                &attacking_player,
                direction,
                &mut self.projectiles,
                &mut self.next_projectile_id,
            ),
        }
    }

    pub fn h4ck_basic_attack(
        attacking_player: &Player,
        direction: &RelativePosition,
        projectiles: &mut Vec<Projectile>,
        next_projectile_id: &mut u64,
    ) -> Result<(), String> {
        if direction.x != 0 || direction.y != 0 {
            let piercing = match attacking_player
                .character
                .status_effects
                .get(&Effect::Piercing)
            {
                Some((1_u64..=u64::MAX)) => true,
                None | Some(0) => false,
            };

            let projectile = Projectile::new(
                *next_projectile_id,
                attacking_player.position,
                JoystickValues::new(direction.x as f32 / 100f32, direction.y as f32 / 100f32),
                14,
                10,
                attacking_player.id,
                attacking_player.character.attack_dmg_basic_skill(),
                30,
                ProjectileType::BULLET,
                ProjectileStatus::ACTIVE,
                attacking_player.id,
                piercing,
            );
            projectiles.push(projectile);
            (*next_projectile_id) += 1;
        }
        Ok(())
    }

    pub fn position_to_direction(position: &RelativePosition) -> Direction {
        if position.x > 0 && position.y > 0 {
            if position.x > position.y {
                return Direction::RIGHT;
            } else {
                return Direction::UP;
            }
        } else if position.x > 0 && position.y <= 0 {
            if position.x > -position.y {
                return Direction::RIGHT;
            } else {
                return Direction::DOWN;
            }
        } else if position.x <= 0 && position.y > 0 {
            if -position.x > position.y {
                return Direction::LEFT;
            } else {
                return Direction::UP;
            }
        } else if position.x <= 0 && position.y <= 0 {
            if -position.x > -position.y {
                return Direction::LEFT;
            } else {
                return Direction::DOWN;
            }
        } else {
            return Direction::UP;
        }
    }

    pub fn muflus_basic_attack(
        board: &mut Board,
        players: &mut Vec<Player>,
        attacking_player: &Player,
        direction: &RelativePosition,
    ) -> Result<(), String> {
        let attack_dmg = attacking_player.character.attack_dmg_basic_skill() as i64;
        let attack_direction = Self::position_to_direction(direction);

        // TODO: This should be a config of the attack
        let attack_range = 20;
        let (top_left, bottom_right) = compute_attack_initial_positions(
            &(attack_direction),
            &(attacking_player.position),
            attack_range,
        );

        let mut affected_players: Vec<u64> =
            GameState::players_in_range(board, top_left, bottom_right)
                .into_iter()
                .filter(|&id| id != attacking_player.id)
                .collect();

        let mut kill_count = 0;
        for target_player_id in affected_players.iter_mut() {
            // FIXME: This is not ok, we should save referencies to the Game Players this is redundant
            let attacked_player = players
                .iter_mut()
                .find(|player| player.id == *target_player_id && player.id != attacking_player.id);

            match attacked_player {
                Some(ap) => {
                    ap.modify_health(-attack_dmg);
                    if matches!(ap.status, Status::DEAD) {
                        kill_count += 1;
                    }
                    let player = ap.clone();
                    GameState::modify_cell_if_player_died(board, &player);
                }
                _ => continue,
            }
        }
        add_kills(players, attacking_player.id, kill_count).expect("Player not found");

        Ok(())
    }

    pub fn skill_1(
        self: &mut Self,
        attacking_player_id: u64,
        direction: &RelativePosition,
    ) -> Result<(), String> {
        let attacking_player = GameState::get_player_mut(&mut self.players, attacking_player_id)?;

        if !attacking_player.can_attack(attacking_player.first_skill_cooldown_left) {
            return Ok(());
        }

        let now = time_now();
        attacking_player.last_melee_attack = now;
        attacking_player.action = PlayerAction::EXECUTINGSKILL1;
        attacking_player.first_skill_start = now;
        attacking_player.first_skill_cooldown_left =
            attacking_player.character.cooldown_first_skill();

        match attacking_player.character.name {
            Name::H4ck => Self::h4ck_skill_1(
                &attacking_player,
                direction,
                &mut self.projectiles,
                &mut self.next_projectile_id,
            ),
            Name::Muflus => {
                let players = &mut self.players;
                Self::muflus_skill_1(&mut self.board, players, attacking_player_id)
            }
            _ => Self::h4ck_skill_1(
                &attacking_player,
                direction,
                &mut self.projectiles,
                &mut self.next_projectile_id,
            ),
        }
    }

    pub fn h4ck_skill_1(
        attacking_player: &Player,
        direction: &RelativePosition,
        projectiles: &mut Vec<Projectile>,
        next_projectile_id: &mut u64,
    ) -> Result<(), String> {
        if direction.x != 0 || direction.y != 0 {
            let angle = (direction.y as f32).atan2(direction.x as f32); // Calculates the angle in radians.
            let angle_positive = if angle < 0.0 {
                (angle + 2.0 * PI).to_degrees() // Adjusts the angle if negative.
            } else {
                angle.to_degrees()
            };

            let angle_modifiers = [-20f32, -10f32, 0f32, 10f32, 20f32];

            for modifier in angle_modifiers {
                let projectile = Projectile::new(
                    *next_projectile_id,
                    attacking_player.position,
                    JoystickValues::new(
                        (angle_positive + modifier).to_radians().cos(),
                        (angle_positive + modifier).to_radians().sin(),
                    ),
                    10,
                    10,
                    attacking_player.id,
                    attacking_player.character.attack_dmg_first_active(),
                    10,
                    ProjectileType::BULLET,
                    ProjectileStatus::ACTIVE,
                    attacking_player.id,
                    false,
                );
                projectiles.push(projectile);
                (*next_projectile_id) += 1;
            }
        }
        Ok(())
    }

    pub fn muflus_skill_1(
        board: &mut Board,
        players: &mut Vec<Player>,
        attacking_player_id: u64,
    ) -> Result<(), String> {
        // TODO: This should be a config of the attack
        let attacking_player = GameState::get_player_mut(players, attacking_player_id)?;
        let attack_dmg = attacking_player.character.attack_dmg_first_active() as i64;

        // TODO: This should be a config of the attack
        let attack_range = 20;

        let (top_left, bottom_right) =
            compute_barrel_roll_initial_positions(&(attacking_player.position), attack_range);

        let mut affected_players: Vec<u64> =
            GameState::players_in_range(board, top_left, bottom_right)
                .into_iter()
                .filter(|&id| id != attacking_player_id)
                .collect();

        for target_player_id in affected_players.iter_mut() {
            // FIXME: This is not ok, we should save referencies to the Game Players this is redundant
            let attacked_player = players
                .iter_mut()
                .find(|player| player.id == *target_player_id && player.id != attacking_player_id);

            match attacked_player {
                Some(ap) => {
                    ap.modify_health(-attack_dmg);
                    let player = ap.clone();
                    GameState::modify_cell_if_player_died(board, &player);
                }
                _ => continue,
            }
        }
        Ok(())
    }

    pub fn leap(
        board: &mut Board,
        attacking_player_id: u64,
        direction: &RelativePosition,
        players: &mut Vec<Player>,
    ) -> Result<(), String> {
        let attacking_player = GameState::get_player_mut(players, attacking_player_id)?;
        Self::move_player_to_coordinates(board, attacking_player, direction)?;
        Self::muflus_skill_1(board, players, attacking_player_id)?;
        Ok(())
    }

    pub fn skill_2(
        self: &mut Self,
        attacking_player_id: u64,
        direction: &RelativePosition,
    ) -> Result<(), String> {
        let attacking_player = GameState::get_player_mut(&mut self.players, attacking_player_id)?;

        if !attacking_player.can_attack(attacking_player.second_skill_cooldown_left) {
            return Ok(());
        }

        let now = time_now();
        attacking_player.last_melee_attack = now;
        attacking_player.action = PlayerAction::EXECUTINGSKILL2;
        attacking_player.second_skill_cooldown_start = now;
        attacking_player.second_skill_cooldown_left =
            attacking_player.character.cooldown_basic_skill();

        match attacking_player.character.name {
            Name::H4ck => Self::h4ck_skill_2(
                &attacking_player,
                direction,
                &mut self.projectiles,
                &mut self.next_projectile_id,
            ),
            Name::Muflus => {
                let id = attacking_player.id;
                Self::leap(&mut self.board, id, direction, &mut self.players)
            }
            _ => Self::h4ck_skill_2(
                &attacking_player,
                direction,
                &mut self.projectiles,
                &mut self.next_projectile_id,
            ),
        }
    }

    pub fn h4ck_skill_2(
        attacking_player: &Player,
        direction: &RelativePosition,
        projectiles: &mut Vec<Projectile>,
        next_projectile_id: &mut u64,
    ) -> Result<(), String> {
        if direction.x != 0 || direction.y != 0 {
            let projectile = Projectile::new(
                *next_projectile_id,
                attacking_player.position,
                JoystickValues::new(direction.x as f32 / 100f32, direction.y as f32 / 100f32),
                14,
                10,
                attacking_player.id,
                0,
                30,
                ProjectileType::DISARMINGBULLET,
                ProjectileStatus::ACTIVE,
                attacking_player.id,
                false,
            );
            projectiles.push(projectile);
            (*next_projectile_id) += 1;
        }
        Ok(())
    }

    pub fn skill_3(
        self: &mut Self,
        _attacking_player_id: u64,
        _direction: &RelativePosition,
    ) -> Result<(), String> {
        return Ok(());
    }

    pub fn skill_4(
        self: &mut Self,
        attacking_player_id: u64,
        _direction: &RelativePosition,
    ) -> Result<(), String> {
        let attacking_player = GameState::get_player_mut(&mut self.players, attacking_player_id)?;

        if !attacking_player.can_attack(attacking_player.fourth_skill_cooldown_left) {
            return Ok(());
        }

        let now = time_now();
        attacking_player.last_melee_attack = now;
        attacking_player.action = PlayerAction::EXECUTINGSKILL4;
        attacking_player.fourth_skill_start = now;
        attacking_player.fourth_skill_cooldown_left =
            attacking_player.character.cooldown_fourth_skill();

        match attacking_player.character.name {
            Name::H4ck => {
                attacking_player
                    .character
                    .add_effect(Effect::Piercing.clone(), 300);
                Ok(())
            }
            _ => Ok(()),
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

    pub fn world_tick(self: &mut Self) -> Result<(), String> {
        self.players.iter_mut().for_each(|player| {
            // Clean each player actions
            player.action = PlayerAction::NOTHING;
            player.update_cooldowns();
            // Keep only (de)buffs that have
            // a non-zero amount of ticks left.
            player.character.status_effects.retain(|_, ticks_left| {
                *ticks_left = ticks_left.saturating_sub(1);
                *ticks_left != 0
            });
        });

        self.projectiles.iter_mut().for_each(|projectile| {
            projectile.move_or_explode_if_out_of_board(self.board.height, self.board.width);
            projectile.remaining_ticks = projectile.remaining_ticks.saturating_sub(1);
        });

        self.projectiles
            .retain(|projectile| projectile.remaining_ticks > 0);

        for projectile in self.projectiles.iter_mut() {
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
                        .filter(|&id| {
                            id != projectile.player_id && id != projectile.last_attacked_player_id
                        })
                        .collect();

                if affected_players.len() > 0 && !projectile.pierce {
                    projectile.status = ProjectileStatus::EXPLODED;
                }

                let mut kill_count = 0;
                for target_player_id in affected_players {
                    let attacked_player =
                        GameState::get_player_mut(&mut self.players, target_player_id)?;
                    match projectile.projectile_type {
                        ProjectileType::DISARMINGBULLET => {
                            attacked_player
                                .character
                                .add_effect(Effect::Disarmed.clone(), 300);
                        }
                        _ => {
                            attacked_player.modify_health(-(projectile.damage as i64));
                            if matches!(attacked_player.status, Status::DEAD) {
                                kill_count += 1;
                            }
                            GameState::modify_cell_if_player_died(&mut self.board, attacked_player);
                            projectile.last_attacked_player_id = attacked_player.id;
                        }
                    }
                }

                add_kills(&mut self.players, projectile.player_id, kill_count)?;
            }
        }
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
    range: usize,
) -> (Position, Position) {
    let x = position.x;
    let y = position.y;

    match direction {
        Direction::UP => (
            Position::new(x.saturating_sub(range), y.saturating_sub(range)),
            Position::new(x.saturating_sub(1), y + range),
        ),
        Direction::DOWN => (
            Position::new(x + 1, y.saturating_sub(range)),
            Position::new(x + range, y + range),
        ),
        Direction::LEFT => (
            Position::new(x.saturating_sub(range), y.saturating_sub(range)),
            Position::new(x + range, y.saturating_sub(1)),
        ),
        Direction::RIGHT => (
            Position::new(x.saturating_sub(range), y + 1),
            Position::new(x + range, y + range),
        ),
    }
}

fn compute_barrel_roll_initial_positions(
    position: &Position,
    range: usize,
) -> (Position, Position) {
    let x = position.x;
    let y = position.y;
    (
        Position::new(x.saturating_sub(range), y.saturating_sub(range)),
        Position::new(x + range, y + range),
    )
}
// fn compute_attack_aoe_initial_positions(
//     player_position: &Position,
//     attack_position: &RelativePosition,
// ) -> (Position, Position, Position) {
//     let modifier = 120_f64;

//     let x =
//         (player_position.x as f64 + modifier * (-(attack_position.y) as f64) / 100_f64) as usize;
//     let y = (player_position.y as f64 + modifier * (attack_position.x as f64) / 100_f64) as usize;

//     (
//         Position::new(x, y),
//         Position::new(x.saturating_sub(25), y.saturating_sub(25)),
//         Position::new(x + 25, y + 25),
//     )
// }

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
fn normalize_vector(x: f32, y: f32) -> (f32, f32) {
    let norm = f32::sqrt(x.powf(2.) + y.powf(2.));
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
    direction_x: f32,
    direction_y: f32,
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
    let movement_vector_x = movement_direction_x * (speed as f32);
    let movement_vector_y = movement_direction_y * (speed as f32);

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
