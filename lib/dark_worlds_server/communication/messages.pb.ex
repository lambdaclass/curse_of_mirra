defmodule DarkWorldsServer.Communication.Proto.GameStateUpdate do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :players, 1, repeated: true, type: DarkWorldsServer.Communication.Proto.Player

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Player do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :id, 1, type: :uint32
  field :health, 2, type: :uint32
  field :position, 3, type: DarkWorldsServer.Communication.Proto.Position
  field :power, 4, type: :uint32

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Position do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :x, 1, type: :uint32
  field :y, 2, type: :uint32

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end