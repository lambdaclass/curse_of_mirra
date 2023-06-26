defmodule DarkWorldsServer.Engine.Position do
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [:x, :y]
  defstruct [:x, :y]
end

defmodule DarkWorldsServer.Engine.RelativePosition do
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [:x, :y]
  defstruct [:x, :y]
end

defmodule DarkWorldsServer.Engine.JoystickValues do
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [:x, :y]
  defstruct [:x, :y]
end
