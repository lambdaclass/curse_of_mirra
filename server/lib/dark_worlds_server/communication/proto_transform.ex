defmodule DarkWorldsServer.Communication.ProtoTransform do
  alias DarkWorldsServer.Communication.Proto.CharacterConfig
  alias DarkWorldsServer.Communication.Proto.CharacterConfigItem
  alias DarkWorldsServer.Communication.Proto.ClientAction, as: ProtoAction
  alias DarkWorldsServer.Communication.Proto.JoystickValues, as: ProtoJoystickValues
  alias DarkWorldsServer.Communication.Proto.Player, as: ProtoPlayer
  alias DarkWorldsServer.Communication.Proto.Position, as: ProtoPosition
  alias DarkWorldsServer.Communication.Proto.Projectile, as: ProtoProjectile
  alias DarkWorldsServer.Communication.Proto.RelativePosition, as: ProtoRelativePosition
  alias DarkWorldsServer.Communication.Proto.RunnerConfig
  alias DarkWorldsServer.Communication.Proto.ServerGameSettings
  alias DarkWorldsServer.Engine.ActionOk, as: EngineAction
  alias DarkWorldsServer.Engine.JoystickValues, as: EngineJoystickValues
  alias DarkWorldsServer.Engine.Player, as: EnginePlayer
  alias DarkWorldsServer.Engine.Position, as: EnginePosition
  alias DarkWorldsServer.Engine.Projectile, as: EngineProjectile
  alias DarkWorldsServer.Engine.RelativePosition, as: EngineRelativePosition

  @behaviour Protobuf.TransformModule

  def encode(runner_config, RunnerConfig) do
    runner_config
  end

  def encode(character_config, CharacterConfig) do
    character_config
  end

  def encode(character_config_item, CharacterConfigItem) do
    character_config_item
  end

  @impl Protobuf.TransformModule
  def encode(
        %{character_config: character_config, runner_config: runner_config},
        ServerGameSettings
      ) do
    %{
      Name: name,
      board_height: board_height,
      board_width: board_width,
      game_timeout_ms: game_timeout_ms,
      server_tickrate_ms: server_tickrate_ms
    } = runner_config

    runner_config = %RunnerConfig{
      Name: name,
      board_height: board_height,
      board_width: board_width,
      game_timeout_ms: game_timeout_ms,
      server_tickrate_ms: server_tickrate_ms
    }

    character_config = %CharacterConfig{
      Items: character_config[:Items]
    }

    %ServerGameSettings{
      runner_config: runner_config,
      character_config: character_config
    }
  end

  def encode(%EnginePosition{} = position, ProtoPosition) do
    %{x: x, y: y} = position
    %ProtoPosition{x: x, y: y}
  end

  def encode(%EngineJoystickValues{} = position, ProtoJoystickValues) do
    %{x: x, y: y} = position
    %ProtoJoystickValues{x: x, y: y}
  end

  def encode(%EnginePlayer{} = player, ProtoPlayer) do
    %EnginePlayer{
      id: id,
      health: health,
      position: position,
      action: action,
      aoe_position: aoe_position,
      kill_count: kill_count,
      death_count: death_count,
      basic_skill_cooldown_left: b_cooldown,
      first_skill_cooldown_left: f_cooldown,
      second_skill_cooldown_left: s_cooldown,
      third_skill_cooldown_left: t_cooldown,
      character_name: name
    } = player

    %ProtoPlayer{
      id: id,
      health: health,
      position: position,
      action: player_action_encode(action),
      aoe_position: aoe_position,
      kill_count: kill_count,
      death_count: death_count,
      basic_skill_cooldown_left: b_cooldown,
      first_skill_cooldown_left: f_cooldown,
      second_skill_cooldown_left: s_cooldown,
      third_skill_cooldown_left: t_cooldown,
      character_name: name
    }
  end

  def encode(%EngineProjectile{} = projectile, ProtoProjectile) do
    %{
      id: id,
      position: position,
      direction: direction,
      speed: speed,
      range: range,
      player_id: player_id,
      damage: damage,
      remaining_ticks: remaining_ticks,
      projectile_type: projectile_type,
      status: status
    } = projectile

    %ProtoProjectile{
      id: id,
      position: position,
      direction: direction,
      speed: speed,
      range: range,
      player_id: player_id,
      damage: damage,
      remaining_ticks: remaining_ticks,
      projectile_type: projectile_encode(projectile_type),
      status: projectile_status_encode(status)
    }
  end

  def encode(%EngineAction{action: :move, value: direction, timestamp: timestamp}, ProtoAction) do
    %ProtoAction{action: :MOVE, direction: direction_encode(direction), timestamp: timestamp}
  end

  def encode(%EngineAction{action: :teleport, value: position, timestamp: timestamp}, ProtoAction) do
    %ProtoAction{action: :TELEPORT, position: position, timestamp: timestamp}
  end

  def encode(%EngineAction{action: :attack, value: direction, timestamp: timestamp}, ProtoAction) do
    %ProtoAction{action: :ATTACK, direction: direction_encode(direction), timestamp: timestamp}
  end

  def encode(%EngineAction{action: :attack_aoe, value: position, timestamp: timestamp}, ProtoAction) do
    %ProtoAction{action: :ATTACK_AOE, position: position, timestamp: timestamp}
  end

  def encode(%EngineAction{action: :skill_1, value: position, timestamp: timestamp}, ProtoAction) do
    %ProtoAction{action: :SKILL_1, position: position, timestamp: timestamp}
  end

  def encode(%EngineAction{action: :skill_2, value: position, timestamp: timestamp}, ProtoAction) do
    %ProtoAction{action: :SKILL_2, position: position, timestamp: timestamp}
  end

  def encode(%EngineAction{action: :basic_attack, value: position, timestamp: timestamp}, ProtoAction) do
    %ProtoAction{action: :BASIC_ATTACK, position: position, timestamp: timestamp}
  end

  @impl Protobuf.TransformModule
  def decode(%ProtoPosition{} = position, ProtoPosition) do
    %{x: x, y: y} = position
    %EnginePosition{x: x, y: y}
  end

  @impl Protobuf.TransformModule
  def decode(%ProtoRelativePosition{} = position, ProtoRelativePosition) do
    %{x: x, y: y} = position
    %EngineRelativePosition{x: x, y: y}
  end

  @impl Protobuf.TransformModule
  def decode(%ProtoJoystickValues{} = position, ProtoJoystickValues) do
    %{x: x, y: y} = position
    %EngineJoystickValues{x: x, y: y}
  end

  def decode(%ProtoPlayer{} = player, ProtoPlayer) do
    %ProtoPlayer{
      id: id,
      health: health,
      position: position,
      last_melee_attack: attack,
      status: status,
      action: action,
      aoe_position: aoe_position,
      kill_count: kill_count,
      death_count: death_count,
      basic_skill_cooldown_left: b_cooldown,
      first_skill_cooldown_left: f_cooldown,
      second_skill_cooldown_left: s_cooldown,
      third_skill_cooldown_left: t_cooldown,
      character_name: name
    } = player

    %EnginePlayer{
      id: id,
      health: health,
      position: position,
      last_melee_attack: attack,
      status: status,
      action: player_action_decode(action),
      aoe_position: aoe_position,
      kill_count: kill_count,
      death_count: death_count,
      basic_skill_cooldown_left: b_cooldown,
      first_skill_cooldown_left: f_cooldown,
      second_skill_cooldown_left: s_cooldown,
      third_skill_cooldown_left: t_cooldown,
      character_name: name
    }
  end

  def decode(%ProtoAction{action: :AUTO_ATTACK, target: target, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :auto_attack, value: target, timestamp: timestamp}
  end

  def decode(%ProtoProjectile{} = projectile, ProtoProjectile) do
    %{
      id: id,
      position: position,
      direction: direction,
      speed: speed,
      range: range,
      player_id: player_id,
      damage: damage,
      remaining_ticks: remaining_ticks,
      projectile_type: projectile_type,
      status: status
    } = projectile

    %EngineProjectile{
      id: id,
      position: position,
      direction: direction,
      speed: speed,
      range: range,
      player_id: player_id,
      damage: damage,
      remaining_ticks: remaining_ticks,
      projectile_type: projectile_decode(projectile_type),
      status: projectile_status_decode(status)
    }
  end

  def decode(%ProtoAction{action: :MOVE_WITH_JOYSTICK, move_delta: %{x: x, y: y}, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :move_with_joystick, value: %{x: x, y: y}, timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :MOVE, direction: direction, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :move, value: direction_decode(direction), timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :ATTACK, direction: direction, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :attack, value: direction_decode(direction), timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :ATTACK_AOE, position: position, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :attack_aoe, value: position, timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :SKILL_1, position: position, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :skill_1, value: position, timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :SKILL_2, position: position, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :skill_2, value: position, timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :BASIC_ATTACK, position: position, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :basic_attack, value: position, timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :ADD_BOT}, ProtoAction, timestamp: timestamp) do
    %EngineAction{action: :add_bot, value: nil, timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :TELEPORT, position: position, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :teleport, value: position, timestamp: timestamp}
  end

  def decode(%struct{} = msg, struct) do
    Map.from_struct(msg)
  end

  ###############################
  # Helpers for transformations #
  ###############################
  defp direction_encode(:up), do: :UP
  defp direction_encode(:down), do: :DOWN
  defp direction_encode(:left), do: :LEFT
  defp direction_encode(:right), do: :RIGHT

  defp direction_decode(:UP), do: :up
  defp direction_decode(:DOWN), do: :down
  defp direction_decode(:LEFT), do: :left
  defp direction_decode(:RIGHT), do: :right

  defp player_action_encode(:attacking), do: :ATTACKING
  defp player_action_encode(:nothing), do: :NOTHING
  defp player_action_encode(:attackingaoe), do: :ATTACKING_AOE
  defp player_action_encode(:executingskill1), do: :EXECUTING_SKILL_1
  defp player_action_encode(:executingskill2), do: :EXECUTING_SKILL_2
  defp player_action_encode(:teleporting), do: :TELEPORTING

  defp player_action_decode(:ATTACKING), do: :attacking
  defp player_action_decode(:NOTHING), do: :nothing
  defp player_action_decode(:ATTACKING_AOE), do: :attackingaoe
  defp player_action_decode(:EXECUTING_SKILL_1), do: :executingskill1
  defp player_action_decode(:EXECUTING_SKILL_2), do: :executingskill2
  defp player_action_decode(:TELEPORTING), do: :teleporting

  defp projectile_encode(:bullet), do: :BULLET
  defp projectile_encode(:disarmingbullet), do: :DISARMING_BULLET
  defp projectile_decode(:BULLET), do: :bullet
  defp projectile_decode(:DISARMING_BULLET), do: :disarmingbullet

  defp projectile_status_encode(:active), do: :ACTIVE
  defp projectile_status_encode(:exploded), do: :EXPLODED

  defp projectile_status_decode(:ACTIVE), do: :active
  defp projectile_status_decode(:EXPLODED), do: :exploded
end
