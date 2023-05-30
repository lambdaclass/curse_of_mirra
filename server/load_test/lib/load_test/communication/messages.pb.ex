defmodule LoadTest.Communication.Proto.GameEventType do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :STATE_UPDATE, 0
  field :PING_UPDATE, 1
end

defmodule LoadTest.Communication.Proto.Status do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :ALIVE, 0
  field :DEAD, 1
end

defmodule LoadTest.Communication.Proto.Action do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :ACTION_UNSPECIFIED, 0
  field :MOVE, 1
  field :ATTACK, 2
  field :ATTACK_AOE, 5
  field :MOVE_WITH_JOYSTICK, 6
end

defmodule LoadTest.Communication.Proto.Direction do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :DIRECTION_UNSPECIFIED, 0
  field :UP, 1
  field :DOWN, 2
  field :LEFT, 3
  field :RIGHT, 4
end

defmodule LoadTest.Communication.Proto.PlayerAction do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :NOTHING, 0
  field :ATTACKING, 1
  field :ATTACKING_AOE, 2
end

defmodule LoadTest.Communication.Proto.LobbyEventType do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :TYPE_UNSPECIFIED, 0
  field :CONNECTED, 1
  field :PLAYER_ADDED, 2
  field :GAME_STARTED, 3
  field :PLAYER_COUNT, 4
  field :START_GAME, 5
  field :PLAYER_REMOVED, 6
end

defmodule LoadTest.Communication.Proto.GameEvent do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :type, 1, type: LoadTest.Communication.Proto.GameEventType, enum: true
  field :players, 2, repeated: true, type: LoadTest.Communication.Proto.Player
  field :latency, 3, type: :uint64
end

defmodule LoadTest.Communication.Proto.Player do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :id, 1, type: :uint64
  field :health, 2, type: :sint64
  field :position, 3, type: LoadTest.Communication.Proto.Position
  field :last_melee_attack, 4, type: :uint64, json_name: "lastMeleeAttack"
  field :status, 5, type: LoadTest.Communication.Proto.Status, enum: true
  field :action, 6, type: LoadTest.Communication.Proto.PlayerAction, enum: true
  field :aoe_position, 7, type: LoadTest.Communication.Proto.Position, json_name: "aoePosition"
end

defmodule LoadTest.Communication.Proto.Position do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :x, 1, type: :uint64
  field :y, 2, type: :uint64
end

defmodule LoadTest.Communication.Proto.RelativePosition do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :x, 1, type: :int64
  field :y, 2, type: :int64
end

defmodule LoadTest.Communication.Proto.ClientAction do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :action, 1, type: LoadTest.Communication.Proto.Action, enum: true
  field :direction, 2, type: LoadTest.Communication.Proto.Direction, enum: true
  field :position, 3, type: LoadTest.Communication.Proto.RelativePosition
  field :move_delta, 4, type: LoadTest.Communication.Proto.JoystickValues, json_name: "moveDelta"
end

defmodule LoadTest.Communication.Proto.JoystickValues do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :x, 1, type: :float
  field :y, 2, type: :float
end

defmodule LoadTest.Communication.Proto.LobbyEvent do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :type, 1, type: LoadTest.Communication.Proto.LobbyEventType, enum: true
  field :lobby_id, 2, type: :string, json_name: "lobbyId"
  field :player_id, 3, type: :uint64, json_name: "playerId"
  field :added_player_id, 4, type: :uint64, json_name: "addedPlayerId"
  field :game_id, 5, type: :string, json_name: "gameId"
  field :player_count, 6, type: :uint64, json_name: "playerCount"
  field :players, 7, repeated: true, type: :uint64
  field :removed_player_id, 8, type: :uint64, json_name: "removedPlayerId"
end