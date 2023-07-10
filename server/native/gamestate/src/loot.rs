use rustler::{NifMap, NifTaggedEnum};

use crate::player::Position;

#[derive(NifMap)]
pub struct Loot {
    pub id: u64,
    pub loot_type: LootType,
    pub position: Position,
    pub value: i64,
}

#[derive(NifTaggedEnum)]
pub enum LootType {
    Health,
    Skill1,
    Skill2,
    Skill3,
    Skill4,
}
