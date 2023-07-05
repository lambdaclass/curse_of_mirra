use std::{str::FromStr, collections::HashMap};

use rustler::NifTaggedEnum;
use strum_macros::{Display, EnumString};
// TODO: Add misssing classes
#[derive(NifTaggedEnum, Debug, Clone, EnumString, Display)]
pub enum Class {
    #[strum(serialize = "hun", serialize = "Hunter", ascii_case_insensitive)]
    Hunter,
    #[strum(serialize = "gua", serialize = "Guardian", ascii_case_insensitive)]
    Guardian,
    #[strum(serialize = "ass", serialize = "Assassin", ascii_case_insensitive)]
    Assassin,
}
// TODO: Add misssing skills
#[derive(NifTaggedEnum, Debug, Clone, EnumString, Display)]
pub enum Basic {
    Slingshot,
    #[strum(serialize = "Bash")]
    Bash,
    #[strum(ascii_case_insensitive)]
    Backstab,
}

#[derive(NifTaggedEnum, Debug, Clone, EnumString, Display)]
pub enum FirstActive {
    #[strum(
        serialize = "Barrel Roll",
        serialize = "BarrelRoll",
        ascii_case_insensitive
    )]
    BarrelRoll,
    #[strum(serialize = "Serpent Strike", serialize = "SerpentStrike")]
    SerpentStrike,
    #[strum(ascii_case_insensitive)]
    MultiShot,
}
#[derive(NifTaggedEnum, Debug, Clone, EnumString, Display)]
pub enum SecondActive {
    #[strum(ascii_case_insensitive)]
    Rage,
    Petrify,
    Disarm,
    MirrorImage,
}

#[derive(NifTaggedEnum, Debug, Clone, EnumString, Display)]
pub enum Dash {
    Leap,
    #[strum(ascii_case_insensitive)]
    ShadowStep,
    Hacktivate,
    Blink,
}

#[derive(NifTaggedEnum, Debug, Clone, EnumString, Display)]
pub enum Ultimate {
    #[strum(serialize = "Fiery Rampage", ascii_case_insensitive)]
    FieryRampage,
    #[strum(serialize = "Toxic Tempest", ascii_case_insensitive)]
    ToxicTempest,
    #[strum(serialize = "Denial Of Service", ascii_case_insensitive)]
    DenialOfService,
    #[strum(serialize = "The Trickster", ascii_case_insensitive)]
    TheTrickster,
}
// TODO have a trait for this
// instead of matching enums.
// Something like:
// impl Attack for BasicSkill

pub struct Skill {
    pub name: String,
    pub do_func: u64,
    pub cooldown_ms: u64,
    pub damage: u64,
    pub duration: u64,
    pub projectile: String,
    pub minion: String
}

impl Skill {
    pub fn from_config_map(config: &HashMap<String, String>) -> Result<Skill, String> {
        let name = get_skill_field(config, "Name")?;
        let do_func = get_skill_field(config, "DoFunc")?;
        let cooldown_ms = get_skill_field(config, "Cooldown")?;
        let damage = get_skill_field(config, "Damage")?;
        let duration = get_skill_field(config, "Duration")?;
        let projectile = get_skill_field(config, "Projectile")?;
        let minion = get_skill_field(config, "Minion")?;
        Ok(Self { name, do_func, cooldown_ms, damage, duration, projectile, minion })
    }
}

fn get_skill_field<T: FromStr + std::fmt::Debug>(config: &HashMap<String, String>, key: &str) -> Result<T, String> {
    let value_result = config
        .get(key)
        .ok_or(format!("Missing key: {:?}", key))
        .map(|s| s.to_string());

    match value_result {
        Ok(value) => parse_attribute(&value),
        Err(error) => Err(error)
    }
}

fn parse_attribute<T: FromStr + std::fmt::Debug>(to_parse: &str) -> Result<T, String> {
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
