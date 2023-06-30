defmodule DarkWorldsServer.Engine.GameUpdate do
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys []
  defstruct [
    :killer_player_id,
    :killed_player_id,
    :game_update_type
  ]
end
