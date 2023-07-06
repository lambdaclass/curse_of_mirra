use crate::skills::*;
use rustler::NifTaggedEnum;
use std::collections::HashMap;
use std::ops::Div;
use std::str::FromStr;
use strum_macros::{Display, EnumString};
pub type TicksLeft = u64;

#[derive(NifTaggedEnum, Debug, Clone, EnumString, Display)]
pub enum Class {
    #[strum(serialize = "hun", serialize = "Hunter", ascii_case_insensitive)]
    Hunter,
    #[strum(serialize = "gua", serialize = "Guardian", ascii_case_insensitive)]
    Guardian,
    #[strum(serialize = "ass", serialize = "Assassin", ascii_case_insensitive)]
    Assassin,
}

#[derive(rustler::NifTaggedEnum, Debug, Hash, Clone, PartialEq, Eq)]
pub enum Effect {
    Petrified = 0,
    Disarmed = 1,
    Piercing = 2,
    Raged = 3,
}
impl Effect {
    pub fn is_crowd_control(&self) -> bool {
        match self {
            Effect::Petrified | Effect::Disarmed => true,
            _ => false,
        }
    }
}
#[derive(Debug, Clone, rustler::NifTaggedEnum, EnumString, Display, PartialEq)]
pub enum Name {
    #[strum(ascii_case_insensitive)]
    Uma,
    #[strum(ascii_case_insensitive)]
    H4ck,
    #[strum(ascii_case_insensitive)]
    Muflus,
}

pub type StatusEffects = HashMap<Effect, TicksLeft>;
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
    pub skill_basic: Skill,
    pub skill_1: Skill,
    pub skill_2: Skill,
    pub skill_3: Skill,
    pub skill_4: Skill,
}

impl Character {
    pub fn new(
        class: Class,
        base_speed: u64,
        name: &Name,
        skill_basic: Skill,
        skill_1: Skill,
        skill_2: Skill,
        skill_3: Skill,
        skill_4: Skill,
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
            base_speed,
            skill_basic,
            skill_1,
            skill_2,
            skill_3,
            skill_4,
        }
    }
    // NOTE:
    // A possible improvement here is that elixir sends a Json and
    // we deserialize it here with Serde
    pub fn from_config_map(
        config: &HashMap<String, String>,
        skills: &[Skill],
    ) -> Result<Character, String> {
        let name = get_key(config, "Name")?;
        let id = get_key(config, "Id")?;
        let active = get_key(config, "Active")?;
        let class = get_key(config, "Class")?;
        let faction = get_key(config, "Faction")?;
        let base_speed = get_key(config, "BaseSpeed")?;
        let skill_basic = get_key(config, "SkillBasic")?;
        let skill_1 = get_key(config, "SkillActive1")?;
        let skill_2 = get_key(config, "SkillActive2")?;
        let skill_3 = get_key(config, "SkillDash")?;
        let skill_4 = get_key(config, "SkillUltimate")?;
        Ok(Self {
            active: parse_character_attribute::<u64>(&active)? != 0,
            base_speed: parse_character_attribute(&base_speed)?,
            class: parse_character_attribute(&class)?,
            faction: parse_character_attribute(&faction)?,
            id: parse_character_attribute(&id)?,
            name: parse_character_attribute(&name)?,
            skill_basic: get_skill(&skills, &skill_basic)?,
            skill_1: get_skill(&skills, &skill_1)?,
            skill_2: get_skill(&skills, &skill_2)?,
            skill_3: get_skill(&skills, &skill_3)?,
            skill_4: get_skill(&skills, &skill_4)?,
        })
    }

    pub fn attack_dmg_basic_skill(&self) -> u32 {
        self.skill_basic.damage
    }
    pub fn attack_dmg_first_active(&self) -> u32 {
        self.skill_1.damage
    }
    pub fn attack_dmg_second_active(&mut self) -> u32 {
        self.skill_2.damage
    }

    pub fn cooldown_basic_skill(&self) -> u64 {
        self.skill_basic.cooldown_ms.div(1000)
    }

    pub fn cooldown_first_skill(&self) -> u64 {
        self.skill_1.cooldown_ms.div(1000)
    }

    pub fn cooldown_second_skill(&self) -> u64 {
        self.skill_2.cooldown_ms.div(1000)
    }

    pub fn cooldown_third_skill(&self) -> u64 {
        self.skill_3.cooldown_ms.div(1000)
    }

    pub fn cooldown_fourth_skill(&self) -> u64 {
        self.skill_4.cooldown_ms.div(1000)
    }

    // TODO:
    // There should be an extra logic to choose the aoe effect
    // An aoe effect can come from a skill 1, 2, etc.
    #[inline]
    pub fn select_basic_skill_effect(&self) -> Option<(Effect, TicksLeft)> {
        match self.name {
            Name::Uma => Some((Effect::Petrified, 300)),
            Name::H4ck => Some((Effect::Disarmed, 300)),
            _ => None,
        }
    }
}

//TODO: This character is broken, it has basic skill as all skills
impl Default for Character {
    fn default() -> Self {
        Character::new(
            Class::Hunter,
            5,
            &Name::H4ck,
            Skill::default(),
            Skill::default(),
            Skill::default(),
            Skill::default(),
            Skill::default(),
            true,
            1,
            Faction::Araban,
        )
    }
}
fn get_key(config: &HashMap<String, String>, key: &str) -> Result<String, String> {
    config
        .get(key)
        .ok_or(format!("Missing key: {:?}", key))
        .map(|s| s.to_string())
}
fn parse_character_attribute<T: FromStr>(to_parse: &str) -> Result<T, String> {
    let parsed = T::from_str(&to_parse);
    match parsed {
        Ok(parsed) => Ok(parsed),
        Err(_parsing_error) => Err(format!(
            "Could not parse value: {:?} for Character Type: {}",
            to_parse,
            std::any::type_name::<T>()
        )),
    }
}

fn get_skill(skills: &[Skill], skill_name: &str) -> Result<Skill, String> {
    skills
        .iter()
        .find(|skill| skill.name == skill_name)
        .ok_or(format!("Skill '{}' does not exist", skill_name))
        .map(|skill| skill.clone())
}
