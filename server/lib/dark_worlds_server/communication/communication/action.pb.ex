defmodule DarkWorldsServer.Communication.Proto.Move do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :angle, 1, type: :float

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.UseSkill do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :skill, 1, type: GameSkill
  field :angle, 2, type: :float

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end