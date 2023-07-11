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
