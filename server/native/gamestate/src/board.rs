use rustler::NifStruct;

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
