use rustler::{NifStruct, NifTaggedEnum, ResourceArc};
use std::sync::Mutex;

#[derive(Debug)]
pub struct GridResource {
    pub resource: Mutex<Vec<Vec<Tile>>>,
}

#[derive(Debug, Clone, NifTaggedEnum, PartialEq)]
pub enum Tile {
    Player(u64),
    Empty,
    Wall,
}

#[derive(NifStruct)]
#[module = "DarkWorldsServer.Engine.Board"]
pub struct Board {
    pub width: usize,
    pub height: usize,
    pub grid: ResourceArc<GridResource>,
}
impl Board {
    pub fn new(width: usize, height: usize) -> Self {
        let resource = GridResource {
            resource: Mutex::new(vec![vec![Tile::Empty; height]; width]),
        };
        let grid = ResourceArc::new(resource);

        Self {
            grid,
            width,
            height,
        }
    }

    pub fn get_cell(self: &Self, row_idx: usize, col_idx: usize) -> Option<Tile> {
        if let Some(row) = self.grid.resource.lock().unwrap().get(row_idx) {
            row.get(col_idx).cloned()
        } else {
            None
        }
    }

    // If you want to move players around, use game::GameState::move_player instead.
    pub fn set_cell(self: &mut Self, row_idx: usize, col_idx: usize, value: Tile) {
        self.grid.resource.lock().unwrap()[row_idx][col_idx] = value;
    }
}
