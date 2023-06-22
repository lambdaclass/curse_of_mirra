defmodule DarkWorldsServer.Engine.Player do
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [
    :id,
    :health,
    :position,
    :last_melee_attack,
    :status,
    :action,
    :aoe_position,
    :kill_count,
    :death_count,
    :basic_skill_cooldown_left,
    :first_skill_cooldown_left,
    :second_skill_cooldown_left,
    :third_skill_cooldown_left,
    :fourth_skill_cooldown_left,
    :character_name
  ]
  defstruct [
    :id,
    :health,
    :position,
    :last_melee_attack,
    :status,
    :action,
    :aoe_position,
    :kill_count,
    :death_count,
    :basic_skill_cooldown_left,
    :first_skill_cooldown_left,
    :second_skill_cooldown_left,
    :third_skill_cooldown_left,
    :fourth_skill_cooldown_left,
    :character_name
  ]
end
