use rustler::NifTaggedEnum;
// TODO: Add misssing classes
#[derive(NifTaggedEnum, Debug, Clone)]
pub enum Class {
    Hunter,
    Guardian,
    Assassin,
}
// TODO: Add misssing skills
#[derive(NifTaggedEnum, Debug, Clone)]
pub enum BasicSkill {
    Slingshot,
    Bash,
    BackStab,
}
// TODO have a trait for this
// instead of matching enums.
// Something like:
// impl Attack for BasicSkill
