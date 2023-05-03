defmodule DarkWorldsServer.Communication.Proto.Action do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:ACTION_UNSPECIFIED, 0)
  field(:MOVE, 1)
  field(:ATTACK, 2)
  field(:PING, 3)
  field(:UPDATE_PING, 4)
end

defmodule DarkWorldsServer.Communication.Proto.Direction do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:DIRECTION_UNSPECIFIED, 0)
  field(:UP, 1)
  field(:DOWN, 2)
  field(:LEFT, 3)
  field(:RIGHT, 4)
end

defmodule DarkWorldsServer.Communication.Proto.GameStateUpdate do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:players, 1, repeated: true, type: DarkWorldsServer.Communication.Proto.Player)

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Player do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:id, 1, type: :uint32)
  field(:health, 2, type: :uint32)
  field(:position, 3, type: DarkWorldsServer.Communication.Proto.Position)
  field(:power, 4, type: :uint32)

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Position do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:x, 1, type: :uint32)
  field(:y, 2, type: :uint32)

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.UpdatePing do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:player_id, 1, type: :uint32, json_name: "playerId")
  field(:latency, 2, type: :uint32)

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.ClientAction do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:action, 1, type: DarkWorldsServer.Communication.Proto.Action, enum: true)
  field(:direction, 2, type: DarkWorldsServer.Communication.Proto.Direction, enum: true)
  field(:latency, 3, type: :uint32)

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end
