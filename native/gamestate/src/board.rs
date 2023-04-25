use rustler::NifStruct;

#[derive(Debug, Clone, NifStruct)]
#[module = "DarkWorldsServer.Engine.Board"]
pub struct Board {
    pub width: usize,
    pub height: usize,
    pub grid: Vec<Vec<u64>>,
}

impl Board {
    pub fn new(grid: Vec<Vec<u64>>, width: usize, height: usize) -> Self {
        Self {
            grid,
            width,
            height,
        }
    }

    pub fn get_cell(&self, row_idx: usize, col_idx: usize) -> Option<&u64> {
        if let Some(row) = self.grid.get(row_idx) {
            row.get(col_idx)
        } else {
            None
        }
    }

    pub fn set_cell(&mut self, row_idx: usize, col_idx: usize, value: u64) {
        self.grid[row_idx][col_idx] = value;
    }
}
