defmodule DarkWorldsServer.Engine.Player do
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [
    :id,
    :health,
    :position,
    :status,
    :action,
    :aoe_position,
    :kill_count,
    :death_count,
    :basic_skill_cooldown_left,
    :skill_1_cooldown_left,
    :skill_2_cooldown_left,
    :skill_3_cooldown_left,
    :skill_4_cooldown_left,
    :character_name,
    :effects
  ]
  defstruct [
    :id,
    :health,
    :position,
    :status,
    :action,
    :aoe_position,
    :kill_count,
    :death_count,
    :basic_skill_cooldown_left,
    :skill_1_cooldown_left,
    :skill_2_cooldown_left,
    :skill_3_cooldown_left,
    :skill_4_cooldown_left,
    :character_name,
    :effects
  ]
end
