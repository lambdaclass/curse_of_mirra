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

pub fn angle_between_vectors(v1: RelativePosition, v2: RelativePosition) -> u64 {
    let angle1 = (v1.y as f32).atan2(v1.x as f32).to_degrees();
    let angle2 = (v2.y as f32).atan2(v2.x as f32).to_degrees();

    let mut angle_diff = angle1 - angle2;
    if angle_diff > 180. {
        angle_diff -= 360.;
    } else if angle_diff < -180. {
        angle_diff += 360.;
    }
    angle_diff.abs() as u64 % 360
}
