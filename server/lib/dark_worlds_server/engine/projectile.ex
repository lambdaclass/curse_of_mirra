defmodule DarkWorldsServer.Engine.Projectile do
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [
    :id,
    :position,
    :direction,
    :speed,
    :range,
    :player_id,
    :damage,
    :remaining_ticks,
    :projectile_type,
    :status,
    :last_attacked_player_id,
    :pierce
  ]
  defstruct [
    :id,
    :position,
    :direction,
    :speed,
    :range,
    :player_id,
    :damage,
    :remaining_ticks,
    :projectile_type,
    :status,
    :last_attacked_player_id,
    :pierce
  ]
end
