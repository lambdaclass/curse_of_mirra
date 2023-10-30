defmodule DarkWorldsServer.Communication.Proto.Config do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :charactersConfig, 1, type: DarkWorldsServer.Communication.Proto.CharactersConfig
  field :effectsConfig, 2, type: DarkWorldsServer.Communication.Proto.EffectsConfig
  field :gameConfig, 3, type: DarkWorldsServer.Communication.Proto.GameConfig
  field :lootsConfig, 4, type: DarkWorldsServer.Communication.Proto.LootsConfig
  field :projectilesConfig, 5, type: DarkWorldsServer.Communication.Proto.ProjectilesConfig
  field :SkillsConfig, 6, type: DarkWorldsServer.Communication.Proto.SkillsConfig

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.CharactersConfig do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :characters, 1, repeated: true, type: DarkWorldsServer.Communication.Proto.Character

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.EffectsConfig do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :effects, 1, repeated: true, type: DarkWorldsServer.Communication.Proto.Effect

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.GameConfig do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :width, 1, type: :uint64
  field :height, 2, type: :uint64

  field :map_modification, 3,
    proto3_optional: true,
    type: DarkWorldsServer.Communication.Proto.MapModification,
    json_name: "mapModification"

  field :loot_interval_ms, 4,
    proto3_optional: true,
    type: DarkWorldsServer.Communication.Proto.MillisTime,
    json_name: "lootIntervalMs"

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.LootsConfig do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :loots, 1, repeated: true, type: DarkWorldsServer.Communication.Proto.Loot

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.ProjectilesConfig do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :projectiles_config, 1,
    repeated: true,
    type: DarkWorldsServer.Communication.Proto.Projectile,
    json_name: "projectilesConfig"

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.SkillsConfig do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :skills, 1, repeated: true, type: DarkWorldsServer.Communication.Proto.Skill

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.MapModification do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :modification, 1, type: DarkWorldsServer.Communication.Proto.Modification
  field :starting_radius, 2, type: :uint64, json_name: "startingRadius"
  field :minimum_radius, 3, type: :uint64, json_name: "minimumRadius"
  field :max_radius, 4, type: :uint64, json_name: "maxRadius"

  field :outside_radius_effects, 5,
    repeated: true,
    type: :string,
    json_name: "outsideRadiusEffects"

  field :inside_radius_effects, 6, repeated: true, type: :string, json_name: "insideRadiusEffects"

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Modification do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :modifier, 1, type: :string
  field :value, 2, type: :int64

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Loot do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :name, 1, type: :string
  field :size, 2, type: :uint64
  field :effects, 3, repeated: true, type: :string

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Projectile do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :name, 1, type: :string
  field :base_damage, 2, type: :uint64, json_name: "baseDamage"
  field :base_speed, 3, type: :uint64, json_name: "baseSpeed"
  field :base_size, 4, type: :uint64, json_name: "baseSize"
  field :player_collision, 5, type: :bool, json_name: "playerCollision"
  field :on_hit_effect, 6, repeated: true, type: :string, json_name: "onHitEffect"
  field :max_distance, 7, proto3_optional: true, type: :uint64, json_name: "maxDistance"
  field :duration_ms, 8, proto3_optional: true, type: :float, json_name: "durationMs"

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Character do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :name, 1, type: :string
  field :active, 2, type: :bool
  field :base_speed, 3, type: :uint64, json_name: "baseSpeed"
  field :base_size, 4, type: :uint64, json_name: "baseSize"
  field :skills, 5, repeated: true, type: DarkWorldsServer.Communication.Proto.Skill

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Skill do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :name, 1, type: :string

  field :cooldown_ms, 2,
    type: DarkWorldsServer.Communication.Proto.MillisTime,
    json_name: "cooldownMs"

  field :is_passive, 3, type: :bool, json_name: "isPassive"
  field :mechanics, 4, repeated: true, type: DarkWorldsServer.Communication.Proto.Mechanic

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Mechanic do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :name, 1, type: :string
  field :effects, 2, repeated: true, type: :string
  field :damage, 3, proto3_optional: true, type: :uint64
  field :range, 4, proto3_optional: true, type: :uint64
  field :cone_angle, 5, proto3_optional: true, type: :uint64, json_name: "coneAngle"
  field :on_hit_effects, 6, repeated: true, type: :string, json_name: "onHitEffects"

  field :projectile, 7,
    proto3_optional: true,
    type: DarkWorldsServer.Communication.Proto.Projectile

  field :count, 8, proto3_optional: true, type: :uint64

  field :duration_ms, 9,
    proto3_optional: true,
    type: DarkWorldsServer.Communication.Proto.MillisTime,
    json_name: "durationMs"

  field :max_range, 10, proto3_optional: true, type: :uint64, json_name: "maxRange"

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Effect.Duration do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :type, 1, type: :string

  field :duration_ms, 2,
    type: DarkWorldsServer.Communication.Proto.MillisTime,
    json_name: "durationMs"

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Effect.Periodic do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :type, 1, type: :string
  field :instant_application, 2, type: :string, json_name: "instantApplication"

  field :interval_ms, 3,
    type: DarkWorldsServer.Communication.Proto.MillisTime,
    json_name: "intervalMs"

  field :trigger_count, 4, type: :uint64, json_name: "triggerCount"

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.Effect do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  oneof :effect_type, 0

  field :name, 1, type: :string
  field :simple_type, 2, type: :string, json_name: "simpleType", oneof: 0

  field :duration_type, 3,
    type: DarkWorldsServer.Communication.Proto.Effect.Duration,
    json_name: "durationType",
    oneof: 0

  field :periodic_type, 4,
    type: DarkWorldsServer.Communication.Proto.Effect.Periodic,
    json_name: "periodicType",
    oneof: 0

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end

defmodule DarkWorldsServer.Communication.Proto.MillisTime do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :high, 1, type: :uint64
  field :low, 2, type: :uint64

  def transform_module(), do: DarkWorldsServer.Communication.ProtoTransform
end