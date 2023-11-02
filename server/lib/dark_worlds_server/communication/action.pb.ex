defmodule DarkWorldsServer.Communication.Proto.Move do
  @moduledoc false

  use Protobuf, syntax: :proto3, protoc_gen_elixir_version: "0.12.0"

  field :angle, 1, type: :float

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.UseSkill do
  @moduledoc false

  use Protobuf, syntax: :proto3, protoc_gen_elixir_version: "0.12.0"

  field :skill, 1, type: :string
  field :angle, 2, type: :float

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.GameAction do
  @moduledoc false

  use Protobuf, syntax: :proto3, protoc_gen_elixir_version: "0.12.0"

  oneof :action_type, 0

  field :move, 1, type: DarkWorldsServer.Communication.Proto.Move, oneof: 0

  field :use_skill, 2,
    type: DarkWorldsServer.Communication.Proto.UseSkill,
    json_name: "useSkill",
    oneof: 0

  field :timestamp, 3, type: :int64

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end