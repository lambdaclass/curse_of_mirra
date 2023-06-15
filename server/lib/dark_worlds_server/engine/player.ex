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
    :death_count
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
    :basic_cooldown_left,
    :first_cooldown_left,
    :second_cooldown_lef,
    :ultimate_cooldown_left
  ]
end
