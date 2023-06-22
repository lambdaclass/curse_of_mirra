defmodule LoadTest.Communication.Proto.GameEventType do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:STATE_UPDATE, 0)
  field(:PING_UPDATE, 1)
  field(:PLAYER_JOINED, 2)
  field(:NEXT_ROUND, 3)
  field(:LAST_ROUND, 4)
  field(:GAME_FINISHED, 5)
  field(:INITIAL_POSITIONS, 6)
  field(:SELECTED_CHARACTER_UPDATE, 7)
  field(:FINISH_CHARACTER_SELECTION, 8)
end

defmodule LoadTest.Communication.Proto.Status do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:ALIVE, 0)
  field(:DEAD, 1)
end

defmodule LoadTest.Communication.Proto.Action do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:ACTION_UNSPECIFIED, 0)
  field(:MOVE, 1)
  field(:ATTACK, 2)
  field(:TELEPORT, 4)
  field(:ATTACK_AOE, 5)
  field(:MOVE_WITH_JOYSTICK, 6)
  field(:ADD_BOT, 7)
  field(:AUTO_ATTACK, 8)
  field(:BASIC_ATTACK, 9)
  field(:SKILL_1, 10)
  field(:SKILL_2, 11)
  field(:SKILL_3, 12)
  field(:SKILL_4, 13)
  field(:SELECT_CHARACTER, 14)
end

defmodule LoadTest.Communication.Proto.Direction do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:DIRECTION_UNSPECIFIED, 0)
  field(:UP, 1)
  field(:DOWN, 2)
  field(:LEFT, 3)
  field(:RIGHT, 4)
end

defmodule LoadTest.Communication.Proto.PlayerAction do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:NOTHING, 0)
  field(:ATTACKING, 1)
  field(:ATTACKING_AOE, 2)
  field(:EXECUTING_SKILL_1, 3)
  field(:TELEPORTING, 4)
  field(:EXECUTING_SKILL_2, 5)
  field(:EXECUTING_SKILL_3, 6)
  field(:EXECUTING_SKILL_4, 7)
end

defmodule LoadTest.Communication.Proto.LobbyEventType do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:TYPE_UNSPECIFIED, 0)
  field(:CONNECTED, 1)
  field(:PLAYER_ADDED, 2)
  field(:GAME_STARTED, 3)
  field(:PLAYER_COUNT, 4)
  field(:START_GAME, 5)
  field(:PLAYER_REMOVED, 6)
end

defmodule LoadTest.Communication.Proto.ProjectileType do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:BULLET, 0)
  field(:DISARMING_BULLET, 1)
end

defmodule LoadTest.Communication.Proto.ProjectileStatus do
  @moduledoc false

  use Protobuf, enum: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:ACTIVE, 0)
  field(:EXPLODED, 1)
end

defmodule LoadTest.Communication.Proto.GameEvent.SelectedCharactersEntry do
  @moduledoc false

  use Protobuf, map: true, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:key, 1, type: :uint64)
  field(:value, 2, type: :string)
end

defmodule LoadTest.Communication.Proto.GameEvent do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:type, 1, type: LoadTest.Communication.Proto.GameEventType, enum: true)
  field(:players, 2, repeated: true, type: LoadTest.Communication.Proto.Player)
  field(:latency, 3, type: :uint64)
  field(:projectiles, 4, repeated: true, type: LoadTest.Communication.Proto.Projectile)
  field(:player_joined_id, 5, type: :uint64, json_name: "playerJoinedId")
  field(:winner_player, 6, type: LoadTest.Communication.Proto.Player, json_name: "winnerPlayer")
  field(:current_round, 7, type: :uint64, json_name: "currentRound")
  field(:timestamp, 8, type: :int64)

  field(:selected_characters, 9,
    repeated: true,
    type: LoadTest.Communication.Proto.GameEvent.SelectedCharactersEntry,
    json_name: "selectedCharacters",
    map: true
  )
end

defmodule LoadTest.Communication.Proto.PlayerCharacter do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:player_id, 1, type: :uint64, json_name: "playerId")
  field(:character_name, 2, type: :string, json_name: "characterName")
end

defmodule LoadTest.Communication.Proto.Player do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:id, 1, type: :uint64)
  field(:health, 2, type: :sint64)
  field(:position, 3, type: LoadTest.Communication.Proto.Position)
  field(:last_melee_attack, 4, type: :uint64, json_name: "lastMeleeAttack")
  field(:status, 5, type: LoadTest.Communication.Proto.Status, enum: true)
  field(:action, 6, type: LoadTest.Communication.Proto.PlayerAction, enum: true)
  field(:aoe_position, 7, type: LoadTest.Communication.Proto.Position, json_name: "aoePosition")
  field(:kill_count, 8, type: :uint64, json_name: "killCount")
  field(:death_count, 9, type: :uint64, json_name: "deathCount")

  field(:teleport_position, 10,
    type: LoadTest.Communication.Proto.Position,
    json_name: "teleportPosition"
  )

  field(:basic_skill_cooldown_left, 11, type: :uint64, json_name: "basicSkillCooldownLeft")
  field(:first_skill_cooldown_left, 12, type: :uint64, json_name: "firstSkillCooldownLeft")
  field(:second_skill_cooldown_left, 13, type: :uint64, json_name: "secondSkillCooldownLeft")
  field(:third_skill_cooldown_left, 14, type: :uint64, json_name: "thirdSkillCooldownLeft")
  field(:fourth_skill_cooldown_left, 15, type: :uint64, json_name: "fourthSkillCooldownLeft")
  field(:character_name, 16, type: :string, json_name: "characterName")
end

defmodule LoadTest.Communication.Proto.Position do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:x, 1, type: :uint64)
  field(:y, 2, type: :uint64)
end

defmodule LoadTest.Communication.Proto.RelativePosition do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:x, 1, type: :int64)
  field(:y, 2, type: :int64)
end

defmodule LoadTest.Communication.Proto.ClientAction do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:action, 1, type: LoadTest.Communication.Proto.Action, enum: true)
  field(:direction, 2, type: LoadTest.Communication.Proto.Direction, enum: true)
  field(:position, 3, type: LoadTest.Communication.Proto.RelativePosition)
  field(:move_delta, 4, type: LoadTest.Communication.Proto.JoystickValues, json_name: "moveDelta")
  field(:target, 5, type: :sint64)

  field(:player_character, 6,
    type: LoadTest.Communication.Proto.PlayerCharacter,
    json_name: "playerCharacter"
  )
end

defmodule LoadTest.Communication.Proto.JoystickValues do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:x, 1, type: :float)
  field(:y, 2, type: :float)
end

defmodule LoadTest.Communication.Proto.LobbyEvent do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:type, 1, type: LoadTest.Communication.Proto.LobbyEventType, enum: true)
  field(:lobby_id, 2, type: :string, json_name: "lobbyId")
  field(:player_id, 3, type: :uint64, json_name: "playerId")
  field(:added_player_id, 4, type: :uint64, json_name: "addedPlayerId")
  field(:game_id, 5, type: :string, json_name: "gameId")
  field(:player_count, 6, type: :uint64, json_name: "playerCount")
  field(:players, 7, repeated: true, type: :uint64)
  field(:removed_player_id, 8, type: :uint64, json_name: "removedPlayerId")

  field(:game_config, 9,
    type: LoadTest.Communication.Proto.ServerGameSettings,
    json_name: "gameConfig"
  )
end

defmodule LoadTest.Communication.Proto.RunnerConfig do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:Name, 1, type: :string)
  field(:board_width, 2, type: :uint64, json_name: "boardWidth")
  field(:board_height, 3, type: :uint64, json_name: "boardHeight")
  field(:server_tickrate_ms, 4, type: :uint64, json_name: "serverTickrateMs")
  field(:game_timeout_ms, 5, type: :uint64, json_name: "gameTimeoutMs")
end

defmodule LoadTest.Communication.Proto.GameConfig do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:board_size, 1, type: LoadTest.Communication.Proto.BoardSize, json_name: "boardSize")
  field(:server_tickrate_ms, 2, type: :uint64, json_name: "serverTickrateMs")
  field(:game_timeout_ms, 3, type: :uint64, json_name: "gameTimeoutMs")
end

defmodule LoadTest.Communication.Proto.BoardSize do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:width, 1, type: :uint64)
  field(:height, 2, type: :uint64)
end

defmodule LoadTest.Communication.Proto.CharacterConfigItem do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:Name, 1, type: :string)
  field(:Id, 2, type: :string)
  field(:Active, 3, type: :string)
  field(:Class, 4, type: :string)
  field(:Faction, 5, type: :string)
  field(:BaseSpeed, 6, type: :string)
  field(:SkillBasic, 7, type: :string)
  field(:SkillActive1, 8, type: :string)
  field(:SkillActive2, 9, type: :string)
  field(:SkillDash, 10, type: :string)
  field(:SkillUltimate, 11, type: :string)
end

defmodule LoadTest.Communication.Proto.CharacterConfig do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:Items, 1, repeated: true, type: LoadTest.Communication.Proto.CharacterConfigItem)
end

defmodule LoadTest.Communication.Proto.ServerGameSettings do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:runner_config, 1,
    type: LoadTest.Communication.Proto.RunnerConfig,
    json_name: "runnerConfig"
  )

  field(:character_config, 2,
    type: LoadTest.Communication.Proto.CharacterConfig,
    json_name: "characterConfig"
  )
end

defmodule LoadTest.Communication.Proto.Projectile do
  @moduledoc false

  use Protobuf, protoc_gen_elixir_version: "0.12.0", syntax: :proto3

  field(:id, 1, type: :uint64)
  field(:position, 2, type: LoadTest.Communication.Proto.Position)
  field(:direction, 3, type: LoadTest.Communication.Proto.JoystickValues)
  field(:speed, 4, type: :uint32)
  field(:range, 5, type: :uint32)
  field(:player_id, 6, type: :uint64, json_name: "playerId")
  field(:damage, 7, type: :uint32)
  field(:remaining_ticks, 8, type: :sint64, json_name: "remainingTicks")

  field(:projectile_type, 9,
    type: LoadTest.Communication.Proto.ProjectileType,
    json_name: "projectileType",
    enum: true
  )

  field(:status, 10, type: LoadTest.Communication.Proto.ProjectileStatus, enum: true)
  field(:last_attacked_player_id, 11, type: :uint64, json_name: "lastAttackedPlayerId")
  field(:pierce, 12, type: :bool)
end
