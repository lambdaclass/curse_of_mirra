use crate::skills::*;
use std::collections::HashMap;
pub type TicksLeft = u64;
#[derive(rustler::NifTaggedEnum, Debug, Hash, Clone, PartialEq, Eq)]
pub enum Effect {
    Petrified,
}
#[derive(Debug, Clone, rustler::NifTaggedEnum)]
pub enum Name {
    Uma,
    H4ck,
    Muflus,
}
#[derive(Debug, Clone, rustler::NifStruct)]
#[module = "DarkWorldsServer.Engine.Character"]
pub struct Character {
    pub class: Class,
    pub name: Name,
    pub base_speed: u64,
    pub basic_skill: BasicSkill,
    pub status_effects: HashMap<Effect, TicksLeft>,
}

impl Character {
    pub fn new(class: Class, speed: u64, name: &Name, basic_skill: BasicSkill) -> Self {
        Self {
            class,
            name: name.clone(),
            basic_skill,
            base_speed: speed,
            status_effects: HashMap::new(),
        }
    }
    pub fn muflus() -> Self {
        Character {
            class: Class::Guardian,
            basic_skill: BasicSkill::Bash,
            base_speed: 5,
            name: Name::Muflus,
            ..Default::default()
        }
    }
    pub fn uma() -> Self {
        Character {
            class: Class::Assassin,
            name: Name::Uma,
            base_speed: 4,
            basic_skill: BasicSkill::BackStab,
            ..Default::default()
        }
    }
    #[inline]
    pub fn attack_dmg(&self) -> u64 {
        // TODO have a trait for this
        // instead of matching enums.
        match self.basic_skill {
            BasicSkill::Slingshot => 10_u64,
            BasicSkill::Bash => 30_u64,
            BasicSkill::BackStab => 10_u64,
        }
    }
    // Cooldown in seconds
    #[inline]
    pub fn cooldown(&self) -> u64 {
        match self.basic_skill {
            BasicSkill::Slingshot => 5,
            BasicSkill::Bash => 3,
            BasicSkill::BackStab => 1,
        }
    }
    #[inline]
    pub fn speed(&self) -> u64 {
        match self.status_effects.get(&Effect::Petrified) {
            Some((1_u64..=u64::MAX)) => 0,
            None | Some(0) => self.base_speed,
        }
    }
    #[inline]
    pub fn add_effect(&mut self, e: Effect, tl: TicksLeft) {
        self.status_effects.insert(e.clone(), tl);
    }

    // TODO:
    // There should be an extra logic to choose the aoe effect
    // An aoe effect can come from a skill 1, 2, etc.
    #[inline]
    pub fn select_aoe_effect(&self) -> Option<(Effect, TicksLeft)> {
        match self.name {
            Name::Uma => Some((Effect::Petrified, 300)),
            _ => None,
        }
    }
}
impl Default for Character {
    fn default() -> Self {
        Character::new(Class::Hunter, 3, &Name::H4ck, BasicSkill::Slingshot)
    }
}
