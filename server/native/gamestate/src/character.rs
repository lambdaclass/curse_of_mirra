use crate::skills::*;
use crate::skills::{Basic as BasicSkill, Class, FirstActive, SecondActive};
use std::collections::HashMap;
use std::str::FromStr;
use strum_macros::{Display, EnumString};
pub type TicksLeft = u64;
#[derive(rustler::NifTaggedEnum, Debug, Hash, Clone, PartialEq, Eq)]
pub enum Effect {
    Petrified,
    Disarmed,
}
#[derive(Debug, Clone, rustler::NifTaggedEnum, EnumString, Display)]
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
    pub skill_basic: Basic,
    pub skill_active_first: FirstActive,
    pub skill_active_second: SecondActive,
    pub skill_dash: Dash,
    pub skill_ultimate: Ultimate,
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
            skill_basic: basic_skill,
            base_speed: speed,
            status_effects: HashMap::new(),
            skill_active_first: FirstActive::BarrelRoll,
            skill_active_second: SecondActive::Disarm,
            skill_dash: Dash::Blink,
            skill_ultimate: Ultimate::DenialOfService,
        }
    }
    // NOTE:
    // A possible improvement here is that elixir sends a Json and
    // we deserialize it here with Serde
    pub fn from_config_map(config: &HashMap<String, String>) -> Result<Character, String> {
        let name = get_key(config, "Name")?;
        let id = get_key(config, "Id")?;
        let active = get_key(config, "Active")?;
        let class = get_key(config, "Class")?;
        let faction = get_key(config, "Faction")?;
        let base_speed = get_key(config, "BaseSpeed")?;
        let skill_basic = get_key(config, "SkillBasic")?;
        let skill_active_first = get_key(config, "SkillActive1")?;
        let skill_active_second = get_key(config, "SkillActive2")?;
        let skill_dash = get_key(config, "SkillDash")?;
        let skill_ultimate = get_key(config, "SkillUltimate")?;
        Ok(Self {
            active: parse_character_attribute::<u64>(&active)? != 0,
            base_speed: parse_character_attribute(&base_speed)?,
            class: parse_character_attribute(&class)?,
            faction: parse_character_attribute(&faction)?,
            id: parse_character_attribute(&id)?,
            name: parse_character_attribute(&name)?,
            skill_active_first: parse_character_attribute(&skill_active_first)?,
            skill_active_second: parse_character_attribute(&skill_active_second)?,
            skill_basic: parse_character_attribute(&skill_basic)?,
            skill_dash: parse_character_attribute(&skill_dash)?,
            skill_ultimate: parse_character_attribute(&skill_ultimate)?,
            status_effects: HashMap::new(),
        })
    }
    pub fn attack_dmg_basic_skill(&self) -> u32 {
        match self.skill_basic {
            BasicSkill::Slingshot => 10_u32, // H4ck basic attack damage
            BasicSkill::Bash => 30_u32,      // Muflus basic attack damage
            BasicSkill::Backstab => 10_u32,
        }
    }
    pub fn attack_dmg_first_active(&self) -> u32 {
        match self.skill_active_first {
            FirstActive::BarrelRoll => 50_u32,    // Muflus skill 1 damage
            FirstActive::SerpentStrike => 30_u32, // H4ck skill 1 damage
            FirstActive::MultiShot => 10_u32,
        }
    }
    pub fn attack_dmg_second_active(&self) -> u32 {
        match self.skill_active_second {
            SecondActive::Rage => 10_u32,
            SecondActive::Petrify => 30_u32,
            SecondActive::MirrorImage => 10_u32,
            SecondActive::Disarm => 5_u32,
        }
    }
    #[inline]
    pub fn cooldown_basic_skill(&self) -> u64 {
        match self.skill_basic {
            BasicSkill::Slingshot => 1_u64, // H4ck basic attack cooldown
            BasicSkill::Bash => 1_u64,      // Muflus basic attack cooldown
            BasicSkill::Backstab => 1_u64,
        }
    }
    pub fn cooldown_first_skill(&self) -> u64 {
        match self.skill_active_first {
            FirstActive::BarrelRoll => 5_u64, // Muflus skill 1 cooldown
            FirstActive::SerpentStrike => 5_u64,
            FirstActive::MultiShot => 5_u64, // H4ck skill 1 cooldown
        }
    }
    pub fn cooldown_second_skill(&self) -> u64 {
        match self.skill_active_second {
            SecondActive::Disarm => 5_u64,
            SecondActive::MirrorImage => 5_u64,
            SecondActive::Petrify => 5_u64,
            SecondActive::Rage => 5_u64,
        }
    }
    // Cooldown in seconds
    #[inline]
    pub fn cooldown(&self) -> u64 {
        match self.skill_basic {
            BasicSkill::Slingshot => 5,
            BasicSkill::Bash => 3,
            BasicSkill::Backstab => 1,
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
        self.status_effects.insert(e, tl);
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
fn get_key(config: &HashMap<String, String>, key: &str) -> Result<String, String> {
    config
        .get(key)
        .ok_or(format!("Missing key: {:?}", key))
        .map(|s| s.to_string())
}
fn parse_character_attribute<T: FromStr + std::fmt::Debug>(to_parse: &str) -> Result<T, String> {
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
