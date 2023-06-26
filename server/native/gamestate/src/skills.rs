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
