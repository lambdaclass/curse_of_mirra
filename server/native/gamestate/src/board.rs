use rustler::{NifStruct, NifTaggedEnum, ResourceArc};
use std::sync::Mutex;
#[derive(Debug)]
pub struct GridResource {
    pub resource: Mutex<Vec<Vec<Tile>>>,
}
#[derive(Debug)]
pub struct FlatGridResource {
    pub resource: Mutex<Vec<Tile>>,
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
    pub grid: ResourceArc<FlatGridResource>,
}
impl Board {
    pub fn new(width: usize, height: usize) -> Self {
        let resource = FlatGridResource {
            resource: Mutex::new(vec![Tile::Empty; width * height]),
        };
        let grid = ResourceArc::new(resource);

        Self {
            grid,
            width,
            height,
        }
    }
    pub fn get_cell(self: &Self, row_idx: usize, col_idx: usize) -> Option<Tile> {
        let indx = (row_idx * self.width) + col_idx;
        self.grid
            .resource
            .lock()
            .expect("Could not get lock to resource!")
            .get(indx)
            .map(|x| x.clone())
    }

    // If you want to move players around, use game::GameState::move_player instead.
    pub fn set_cell(self: &mut Self, row_idx: usize, col_idx: usize, value: Tile) {
        let indx = (row_idx * self.width) + col_idx;
        let mut board = self.grid.resource.lock().unwrap();
        let cell = board.get_mut(indx).unwrap();
        *cell = value;
    }
}
