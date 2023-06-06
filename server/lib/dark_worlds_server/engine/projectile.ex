defmodule DarkWorldsServer.Engine.Projectile do
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [:id, :position, :direction, :speed, :range, :player_id, :damage, :remaining_ticks, :projectile_type]
  defstruct [:id, :position, :direction, :speed, :range, :player_id, :damage, :remaining_ticks, :projectile_type]
end
