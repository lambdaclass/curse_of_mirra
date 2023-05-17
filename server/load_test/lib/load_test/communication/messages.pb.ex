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
  field :PING, 3
  field :UPDATE_PING, 4
  field :ATTACK_AOE, 5
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

defmodule LoadTest.Communication.Proto.LobbyEventType do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :TYPE_UNSPECIFIED, 0
  field :CONNECTED, 1
  field :PLAYER_ADDED, 2
  field :GAME_STARTED, 3
  field :PLAYER_COUNT, 4
end

defmodule LoadTest.Communication.Proto.GameStateUpdate do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :players, 1, repeated: true, type: LoadTest.Communication.Proto.Player
end

defmodule LoadTest.Communication.Proto.Player do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :id, 1, type: :uint64
  field :health, 2, type: :sint64
  field :position, 3, type: LoadTest.Communication.Proto.Position
  field :last_melee_attack, 4, type: :uint64, json_name: "lastMeleeAttack"
  field :status, 5, type: LoadTest.Communication.Proto.Status, enum: true
end

defmodule LoadTest.Communication.Proto.Position do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :x, 1, type: :uint64
  field :y, 2, type: :uint64
end

defmodule LoadTest.Communication.Proto.UpdatePing do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :player_id, 1, type: :uint32, json_name: "playerId"
  field :latency, 2, type: :uint32
end

defmodule LoadTest.Communication.Proto.ClientAction do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field :action, 1, type: LoadTest.Communication.Proto.Action, enum: true
  field :direction, 2, type: LoadTest.Communication.Proto.Direction, enum: true
  field :latency, 3, type: :uint32
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
end
