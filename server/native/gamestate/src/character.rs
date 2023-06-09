use crate::skills::*;
use std::collections::HashMap;
use std::str::FromStr;
use strum_macros::EnumString;
pub type TicksLeft = u64;
#[derive(rustler::NifTaggedEnum, Debug, Hash, Clone, PartialEq, Eq)]
pub enum Effect {
    Petrified,
}
#[derive(Debug, Clone, rustler::NifTaggedEnum, EnumString)]
pub enum Name {
    #[strum(ascii_case_insensitive)]
    Uma,
    #[strum(ascii_case_insensitive)]
    H4ck,
    #[strum(ascii_case_insensitive)]
    Muflus,
}
#[derive(Debug, Clone, rustler::NifTaggedEnum, EnumString)]
pub enum Faction {
    #[strum(serialize = "ara", serialize = "Araban", ascii_case_insensitive)]
    Araban,
    #[strum(serialize = "kal", serialize = "Araban", ascii_case_insensitive)]
    Kaline,
    #[strum(serialize = "oto", serialize = "Otobi", ascii_case_insensitive)]
    Otobi,
    #[strum(serialize = "mer", serialize = "Merliot", ascii_case_insensitive)]
    Merliot,
}
#[derive(Debug, Clone, rustler::NifStruct)]
#[module = "DarkWorldsServer.Engine.Character"]
pub struct Character {
    pub class: Class,
    pub id: u64,
    pub active: bool,
    pub faction: Faction,
    pub name: Name,
    pub base_speed: u64,
    pub basic_skill: Basic,
    pub skill_active_first: Basic,
    pub skill_active_second: Basic,
    pub skill_dash: Basic,
    pub skill_ultimate: Basic,
    pub status_effects: HashMap<Effect, TicksLeft>,
}

impl Character {
    pub fn new(
        class: Class,
        speed: u64,
        name: &Name,
        basic_skill: Basic,
        active: bool,
        id: u64,
        faction: Faction,
    ) -> Self {
        Self {
            class,
            name: name.clone(),
            active,
            id,
            faction,
            basic_skill,
            base_speed: speed,
            status_effects: HashMap::new(),
            skill_active_first: Basic::BackStab,
            skill_active_second: Basic::Bash,
            skill_dash: Basic::BackStab,
            skill_ultimate: Basic::BackStab,
        }
    }
    pub fn muflus() -> Self {
        Character {
            class: Class::Guardian,
            basic_skill: Basic::Bash,
            base_speed: 3,
            name: Name::Muflus,
            ..Default::default()
        }
    }
    pub fn uma() -> Self {
        Character {
            class: Class::Assassin,
            name: Name::Uma,
            base_speed: 4,
            basic_skill: Basic::BackStab,
            ..Default::default()
        }
    }
    #[inline]
    pub fn attack_dmg(&self) -> u64 {
        // TODO have a trait for this
        // instead of matching enums.
        match self.basic_skill {
            Basic::Slingshot => 10_u64,
            Basic::Bash => 40_u64,
            Basic::BackStab => 10_u64,
        }
    }
    // Cooldown in seconds
    #[inline]
    pub fn cooldown(&self) -> u64 {
        match self.basic_skill {
            Basic::Slingshot => 1,
            Basic::Bash => 5,
            Basic::BackStab => 1,
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
        Character::new(
            Class::Hunter,
            5,
            &Name::H4ck,
            Basic::Slingshot,
            true,
            1,
            Faction::Araban,
        )
    }
}
