use crate::skills::*;
use std::collections::HashMap;
pub type TicksLeft = u64;
#[derive(rustler::NifTaggedEnum, Debug, Hash, Clone, PartialEq, Eq)]
pub enum Effect {
    Petrified,
}
#[derive(Debug, Clone, rustler::NifStruct)]
#[module = "DarkWorldsServer.Engine.Character"]
pub struct Character {
    pub class: Class,
    pub name: String,
    pub base_speed: u64,
    pub basic_skill: BasicSkill,
    pub status_effects: HashMap<Effect, TicksLeft>,
}

impl Character {
    pub fn new(class: Class, speed: u64, name: &str, basic_skill: BasicSkill) -> Self {
        Self {
            class,
            name: name.into(),
            basic_skill,
            base_speed: speed,
            status_effects: HashMap::new(),
        }
    }
    #[inline]
    pub fn attack_dmg(&self) -> u64 {
        // TODO have a trait for this
        // instead of matching enums.
        match self.basic_skill {
            BasicSkill::Slingshot => 10_u64,
            BasicSkill::Bash => 40_u64,
        }
    }
    // Cooldown in seconds
    #[inline]
    pub fn cooldown(&self) -> u64 {
        match self.basic_skill {
            BasicSkill::Slingshot => 1,
            BasicSkill::Bash => 5,
        }
    }
    #[inline]
    pub fn speed(&self) -> u64 {
        match self.status_effects.get(&Effect::Petrified) {
            Some((1_u64..=u64::MAX)) => 0,
            None | Some(0) => self.base_speed,
        }
    }
}
impl Default for Character {
    fn default() -> Self {
        Character::new(Class::Hunter, 5, "H4ck", BasicSkill::Slingshot)
    }
}
