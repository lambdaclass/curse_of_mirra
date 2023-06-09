use rustler::NifTaggedEnum;
use std::str::FromStr;
use strum_macros::EnumString;
// TODO: Add misssing classes
#[derive(NifTaggedEnum, Debug, Clone, EnumString)]
pub enum Class {
    #[strum(serialize = "hun", serialize = "Hunter", ascii_case_insensitive)]
    Hunter,
    #[strum(serialize = "gua", serialize = "Guardian", ascii_case_insensitive)]
    Guardian,
    #[strum(serialize = "ass", serialize = "Assassin", ascii_case_insensitive)]
    Assassin,
}
// TODO: Add misssing skills
#[derive(NifTaggedEnum, Debug, Clone, EnumString)]
pub enum Basic {
    Slingshot,
    #[strum(serialize = "Bash", ascii_case_insensitive)]
    Bash,
    BackStab,
}

#[derive(NifTaggedEnum, Debug, Clone, EnumString)]
pub enum FirstActive {
    #[strum(
        serialize = "Barrell Roll",
        serialize = "BarrellRoll",
        ascii_case_insensitive
    )]
    BarrelRoll,
    #[strum(
        serialize = "Serpent Strike",
        serialize = "SerpentStrike",
        ascii_case_insensitive
    )]
    SerpentStrike,
    MultiShot,
}

#[derive(NifTaggedEnum, Debug, Clone, EnumString)]
pub enum SecondActive {
    Rage,
    Petrify,
    Disarm,
    MirrorImage,
}

#[derive(NifTaggedEnum, Debug, Clone, EnumString)]
pub enum Dash {
    Leap,
    ShadowStep,
    Hacktivate,
    Blink,
}

pub enum Ultimate {
    FieryRampage,
    ToxicTempest,
    DenialOfService,
    TheTrickster,
}
// TODO have a trait for this
// instead of matching enums.
// Something like:
// impl Attack for BasicSkill
