use std::{collections::HashMap, str::FromStr};

use rustler::NifStruct;

#[derive(Debug, Clone, NifStruct)]
#[module = "DarkWorldsServer.Engine.Skill"]
pub struct Skill {
    pub name: String,
    pub do_func: u64,
    pub cooldown_ms: u64,
    pub damage: u32,
    pub duration: u64,
    pub projectile: String,
    pub minion: String,
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
        Ok(Self {
            name,
            do_func,
            cooldown_ms,
            damage,
            duration,
            projectile,
            minion,
        })
    }
}

impl Default for Skill {
    fn default() -> Self {
        Skill {
            name: "Slingshot".to_string(),
            do_func: 0,
            cooldown_ms: 1000,
            damage: 10,
            duration: 0,
            projectile: "".to_string(),
            minion: "".to_string(),
        }
    }
}

pub fn build_from_config(skills_config: &[HashMap<String, String>]) -> Result<Vec<Skill>, String> {
    skills_config.iter().map(Skill::from_config_map).collect()
}

fn get_skill_field<T: FromStr>(config: &HashMap<String, String>, key: &str) -> Result<T, String> {
    let value_result = config
        .get(key)
        .ok_or(format!("Missing key: {:?}", key))
        .map(|s| s.to_string());

    match value_result {
        Ok(value) => parse_attribute(&value),
        Err(error) => Err(format!("Error parsing '{}'\n{}", key, error)),
    }
}

fn parse_attribute<T: FromStr>(to_parse: &str) -> Result<T, String> {
    let parsed = T::from_str(&to_parse);
    match parsed {
        Ok(parsed) => Ok(parsed),
        Err(_parsing_error) => Err(format!(
            "Could not parse value: '{}' for Skill Type: {}",
            to_parse,
            std::any::type_name::<T>()
        )),
    }
}
