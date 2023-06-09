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
    :death_count
  ]
end
