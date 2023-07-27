defmodule DarkWorldsServer.Engine.Decoy do
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [
    :id,
    :health,
    :owner,
    :position,
    :status,
    :should_respawn,
  ]
  defstruct [
    :id,
    :health,
    :owner,
    :position,
    :status,
    :should_respawn,
  ]
end
