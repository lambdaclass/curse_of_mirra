use rustler::{NifStruct, NifTaggedEnum, ResourceArc};
use std::sync::Mutex;
pub type Grid = Vec<Tile>;
#[derive(Debug)]
pub struct GridResource {
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
}
impl Board {
    pub fn new(width: usize, height: usize) -> Self {
        Self { width, height }
    }
}
