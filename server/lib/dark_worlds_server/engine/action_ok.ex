defmodule DarkWorldsServer.Engine.ActionOk do
  use DarkWorldsServer.Communication.Encoder
  alias DarkWorldsServer.Engine.ActionRaw

  @enforce_keys [:action, :value, :timestamp]
  defstruct [:action, :value, :timestamp]

  def from_action_raw(%ActionRaw{
        action: {:ok, action},
        value: {:ok, value},
        timestamp: {:ok, timestamp}
      }) do
    {:ok, %__MODULE__{action: action, value: value, timestamp: timestamp}}
  end

  def from_action_raw(_) do
    {:error, "Invalid message"}
  end
end
