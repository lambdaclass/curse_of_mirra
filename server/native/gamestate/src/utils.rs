use std::cmp::Ordering;

use rustler::NifStruct;

#[derive(Debug, Clone, Copy, NifStruct, PartialEq)]
#[module = "DarkWorldsServer.Engine.RelativePosition"]
pub struct RelativePosition {
    pub x: f32,
    pub y: f32,
}

impl RelativePosition {
    pub fn new(x: f32, y: f32) -> Self {
        Self { x, y }
    }
}

pub fn cmp_float(f1: f64, f2: f64) -> Ordering {
    if f1 < f2 {
        Ordering::Less
    } else if f1 > f2 {
        Ordering::Greater
    } else {
        Ordering::Equal
    }
}
