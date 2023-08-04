defmodule DarkWorldsServer.Communication.ProtoTransform do
  alias DarkWorldsServer.Communication.Proto.CharacterConfig
  alias DarkWorldsServer.Communication.Proto.CharacterConfigItem
  alias DarkWorldsServer.Communication.Proto.ClientAction, as: ProtoAction
  alias DarkWorldsServer.Communication.Proto.GameEvent.SelectedCharactersEntry
  alias DarkWorldsServer.Communication.Proto.KillEvent
  alias DarkWorldsServer.Communication.Proto.MillisTime, as: ProtoMillisTime
  alias DarkWorldsServer.Communication.Proto.Player, as: ProtoPlayer
  alias DarkWorldsServer.Communication.Proto.Player.EffectsEntry
  alias DarkWorldsServer.Communication.Proto.Position, as: ProtoPosition
  alias DarkWorldsServer.Communication.Proto.Projectile, as: ProtoProjectile
  alias DarkWorldsServer.Communication.Proto.RelativePosition, as: ProtoRelativePosition
  alias DarkWorldsServer.Communication.Proto.RunnerConfig
  alias DarkWorldsServer.Communication.Proto.ServerGameSettings
  alias DarkWorldsServer.Communication.Proto.SkillConfigItem
  alias DarkWorldsServer.Communication.Proto.SkillsConfig
  alias DarkWorldsServer.Communication.Proto.Status
  alias DarkWorldsServer.Engine.ActionOk, as: EngineAction
  alias DarkWorldsServer.Engine.Player, as: EnginePlayer
  alias DarkWorldsServer.Engine.Position, as: EnginePosition
  alias DarkWorldsServer.Engine.Projectile, as: EngineProjectile
  alias DarkWorldsServer.Engine.RelativePosition, as: EngineRelativePosition

  @behaviour Protobuf.TransformModule

  ###########
  # ENCODES #
  ###########

  def encode(status, Status) do
    status
  end

  def encode(effect, EffectsEntry) do
    effect_encode(effect)
  end

  def encode(entry, SelectedCharactersEntry) do
    entry
  end

  def encode(millis_time, ProtoMillisTime) do
    millis_time
  end

  def encode(skill_config, SkillsConfig) do
    skill_config
  end

  def encode(skill_config_item, SkillConfigItem) do
    skill_config_item
  end

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
        %{
          character_config: character_config,
          runner_config: runner_config,
          skills_config: skills_config
        },
        ServerGameSettings
      ) do
    %{
      Name: name,
      board_height: board_height,
      board_width: board_width,
      game_timeout_ms: game_timeout_ms,
      server_tickrate_ms: server_tickrate_ms,
      map_shrink_wait_ms: map_shrink_wait_ms,
      map_shrink_interval: map_shrink_interval,
      out_of_area_damage: out_of_area_damage,
      use_proxy: use_proxy
    } = runner_config

    runner_config = %RunnerConfig{
      Name: name,
      board_height: board_height,
      board_width: board_width,
      game_timeout_ms: game_timeout_ms,
      server_tickrate_ms: server_tickrate_ms,
      map_shrink_wait_ms: map_shrink_wait_ms,
      map_shrink_interval: map_shrink_interval,
      out_of_area_damage: out_of_area_damage,
      use_proxy: use_proxy
    }

    character_config = %CharacterConfig{
      Items: character_config[:Items]
    }

    skills_config = %SkillsConfig{
      Items: skills_config[:Items]
    }

    %ServerGameSettings{
      runner_config: runner_config,
      character_config: character_config,
      skills_config: skills_config
    }
  end

  def encode(%EnginePosition{} = position, ProtoPosition) do
    %{x: x, y: y} = position
    %ProtoPosition{x: x, y: y}
  end

  def encode(%EngineRelativePosition{} = position, ProtoRelativePosition) do
    %{x: x, y: y} = position
    %ProtoRelativePosition{x: x, y: y}
  end

  def encode(%EnginePlayer{} = player, ProtoPlayer) do
    %EnginePlayer{
      id: id,
      health: health,
      position: position,
      status: status,
      action: action,
      aoe_position: aoe_position,
      kill_count: kill_count,
      death_count: death_count,
      basic_skill_cooldown_left: basic_skill_cooldown_left,
      skill_1_cooldown_left: skill_1_cooldown_left,
      skill_2_cooldown_left: skill_2_cooldown_left,
      skill_3_cooldown_left: skill_3_cooldown_left,
      skill_4_cooldown_left: skill_4_cooldown_left,
      character_name: name,
      effects: effects,
      direction: direction
    } = player

    %ProtoPlayer{
      id: id,
      health: health,
      position: position,
      status: player_status_encode(status),
      action: player_action_encode(action),
      aoe_position: aoe_position,
      kill_count: kill_count,
      death_count: death_count,
      basic_skill_cooldown_left: basic_skill_cooldown_left,
      skill_1_cooldown_left: skill_1_cooldown_left,
      skill_2_cooldown_left: skill_2_cooldown_left,
      skill_3_cooldown_left: skill_3_cooldown_left,
      skill_4_cooldown_left: skill_4_cooldown_left,
      character_name: name,
      effects: effects,
      direction: direction
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
      status: status,
      last_attacked_player_id: last_attacked_player_id,
      pierce: pierce
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
      status: projectile_status_encode(status),
      last_attacked_player_id: last_attacked_player_id,
      pierce: pierce
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

  def encode(
        %EngineAction{action: :attack_aoe, value: position, timestamp: timestamp},
        ProtoAction
      ) do
    %ProtoAction{action: :ATTACK_AOE, position: position, timestamp: timestamp}
  end

  def encode(%EngineAction{action: :skill_1, value: position, timestamp: timestamp}, ProtoAction) do
    %ProtoAction{action: :SKILL_1, position: position, timestamp: timestamp}
  end

  def encode(%EngineAction{action: :skill_2, value: position, timestamp: timestamp}, ProtoAction) do
    %ProtoAction{action: :SKILL_2, position: position, timestamp: timestamp}
  end

  def encode(%EngineAction{action: :skill_3, value: position, timestamp: timestamp}, ProtoAction) do
    %ProtoAction{action: :SKILL_3, position: position, timestamp: timestamp}
  end

  def encode(%EngineAction{action: :skill_4, value: position, timestamp: timestamp}, ProtoAction) do
    %ProtoAction{action: :SKILL_4, position: position, timestamp: timestamp}
  end

  def encode(
        %EngineAction{action: :basic_attack, value: position, timestamp: timestamp},
        ProtoAction
      ) do
    %ProtoAction{action: :BASIC_ATTACK, position: position, timestamp: timestamp}
  end

  def encode({killed_by, killed}, KillEvent) do
    %KillEvent{killed_by: killed_by, killed: killed}
  end

  ###########
  # DECODES #
  ###########

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

  def decode(%ProtoPlayer{} = player, ProtoPlayer) do
    %ProtoPlayer{
      id: id,
      health: health,
      position: position,
      status: status,
      action: action,
      aoe_position: aoe_position,
      kill_count: kill_count,
      death_count: death_count,
      basic_skill_cooldown_left: basic_skill_cooldown_left,
      skill_1_cooldown_left: skill_1_cooldown_left,
      skill_2_cooldown_left: skill_2_cooldown_left,
      skill_3_cooldown_left: skill_3_cooldown_left,
      skill_4_cooldown_left: skill_4_cooldown_left,
      character_name: name,
      effects: effects,
      direction: direction
    } = player

    %EnginePlayer{
      id: id,
      health: health,
      position: position,
      status: player_status_decode(status),
      action: player_action_decode(action),
      aoe_position: aoe_position,
      kill_count: kill_count,
      death_count: death_count,
      basic_skill_cooldown_left: basic_skill_cooldown_left,
      skill_1_cooldown_left: skill_1_cooldown_left,
      skill_2_cooldown_left: skill_2_cooldown_left,
      skill_3_cooldown_left: skill_3_cooldown_left,
      skill_4_cooldown_left: skill_4_cooldown_left,
      character_name: name,
      effects: effects,
      direction: direction
    }
  end

  def decode(
        %ProtoAction{action: :AUTO_ATTACK, target: target, timestamp: timestamp},
        ProtoAction
      ) do
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
      status: status,
      last_attacked_player_id: last_attacked_player_id,
      pierce: pierce
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
      status: projectile_status_decode(status),
      last_attacked_player_id: last_attacked_player_id,
      pierce: pierce
    }
  end

  def decode(
        %ProtoAction{
          action: :MOVE_WITH_JOYSTICK,
          move_delta: %{x: x, y: y},
          timestamp: timestamp
        },
        ProtoAction
      ) do
    %EngineAction{action: :move_with_joystick, value: %{x: x, y: y}, timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :MOVE, direction: direction, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :move, value: direction_decode(direction), timestamp: timestamp}
  end

  def decode(
        %ProtoAction{action: :ATTACK, direction: direction, timestamp: timestamp},
        ProtoAction
      ) do
    %EngineAction{action: :attack, value: direction_decode(direction), timestamp: timestamp}
  end

  def decode(
        %ProtoAction{action: :ATTACK_AOE, position: position, timestamp: timestamp},
        ProtoAction
      ) do
    %EngineAction{action: :attack_aoe, value: position, timestamp: timestamp}
  end

  def decode(
        %ProtoAction{action: :SKILL_1, position: position, timestamp: timestamp},
        ProtoAction
      ) do
    %EngineAction{action: :skill_1, value: position, timestamp: timestamp}
  end

  def decode(
        %ProtoAction{action: :SKILL_2, position: position, timestamp: timestamp},
        ProtoAction
      ) do
    %EngineAction{action: :skill_2, value: position, timestamp: timestamp}
  end

  def decode(
        %ProtoAction{action: :SKILL_3, position: position, timestamp: timestamp},
        ProtoAction
      ) do
    %EngineAction{action: :skill_3, value: position, timestamp: timestamp}
  end

  def decode(
        %ProtoAction{action: :SKILL_4, position: position, timestamp: timestamp},
        ProtoAction
      ) do
    %EngineAction{action: :skill_4, value: position, timestamp: timestamp}
  end

  def decode(
        %ProtoAction{action: :BASIC_ATTACK, position: position, timestamp: timestamp},
        ProtoAction
      ) do
    %EngineAction{action: :basic_attack, value: position, timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :ADD_BOT, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :add_bot, value: nil, timestamp: timestamp}
  end

  def decode(
        %ProtoAction{
          action: :SELECT_CHARACTER,
          player_character: player_character,
          timestamp: timestamp
        },
        ProtoAction
      ) do
    %EngineAction{action: :select_character, value: player_character, timestamp: timestamp}
  end

  def decode(
        %ProtoAction{action: :TELEPORT, position: position, timestamp: timestamp},
        ProtoAction
      ) do
    %EngineAction{action: :teleport, value: position, timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :ENABLE_BOTS, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :enable_bots, value: :enable_bots, timestamp: timestamp}
  end

  def decode(%ProtoAction{action: :DISABLE_BOTS, timestamp: timestamp}, ProtoAction) do
    %EngineAction{action: :disable_bots, value: :disable_bots, timestamp: timestamp}
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

  defp player_status_encode(:alive), do: :ALIVE
  defp player_status_encode(:dead), do: :DEAD

  defp player_status_decode(:ALIVE), do: :alive
  defp player_status_decode(:DEAD), do: :dead

  defp player_action_encode(:attacking), do: :ATTACKING
  defp player_action_encode(:nothing), do: :NOTHING
  defp player_action_encode(:attackingaoe), do: :ATTACKING_AOE
  defp player_action_encode(:startingskill1), do: :STARTING_SKILL_1
  defp player_action_encode(:startingskill2), do: :STARTING_SKILL_2
  defp player_action_encode(:startingskill3), do: :STARTING_SKILL_3
  defp player_action_encode(:startingskill4), do: :STARTING_SKILL_4
  defp player_action_encode(:executingskill1), do: :EXECUTING_SKILL_1
  defp player_action_encode(:executingskill2), do: :EXECUTING_SKILL_2
  defp player_action_encode(:executingskill3), do: :EXECUTING_SKILL_3
  defp player_action_encode(:executingskill4), do: :EXECUTING_SKILL_4
  defp player_action_encode(:moving), do: :MOVING

  defp player_action_decode(:ATTACKING), do: :attacking
  defp player_action_decode(:NOTHING), do: :nothing
  defp player_action_decode(:ATTACKING_AOE), do: :attackingaoe
  defp player_action_decode(:STARTING_SKILL_1), do: :startingskill1
  defp player_action_decode(:STARTING_SKILL_2), do: :startingskill2
  defp player_action_decode(:STARTING_SKILL_3), do: :startingskill3
  defp player_action_decode(:STARTING_SKILL_4), do: :startingskill4
  defp player_action_decode(:EXECUTING_SKILL_1), do: :executingskill1
  defp player_action_decode(:EXECUTING_SKILL_2), do: :executingskill2
  defp player_action_decode(:EXECUTING_SKILL_3), do: :executingskill3
  defp player_action_decode(:EXECUTING_SKILL_4), do: :executingskill4
  defp player_action_decode(:MOVING), do: :moving

  defp projectile_encode(:bullet), do: :BULLET
  defp projectile_encode(:disarmingbullet), do: :DISARMING_BULLET
  defp projectile_decode(:BULLET), do: :bullet
  defp projectile_decode(:DISARMING_BULLET), do: :disarmingbullet

  defp projectile_status_encode(:active), do: :ACTIVE
  defp projectile_status_encode(:exploded), do: :EXPLODED

  defp projectile_status_decode(:ACTIVE), do: :active
  defp projectile_status_decode(:EXPLODED), do: :exploded

  defp effect_encode({:petrified, %{ends_at: ends_at}}), do: {0, ends_at}
  defp effect_encode({:disarmed, %{ends_at: ends_at}}), do: {1, ends_at}
  defp effect_encode({:piercing, %{ends_at: ends_at}}), do: {2, ends_at}
  defp effect_encode({:raged, %{ends_at: ends_at}}), do: {3, ends_at}
  defp effect_encode({:neon_crashing, %{ends_at: ends_at}}), do: {4, ends_at}
  defp effect_encode({:leaping, %{ends_at: ends_at}}), do: {5, ends_at}
  defp effect_encode({:out_of_area, %{ends_at: ends_at}}), do: {6, ends_at}
  defp effect_encode({:elnar_mark, %{ends_at: ends_at}}), do: {7, ends_at}
  defp effect_encode({:yugen_mark, %{ends_at: ends_at}}), do: {8, ends_at}
  defp effect_encode({:xanda_mark, %{ends_at: ends_at}}), do: {9, ends_at}
  defp effect_encode({:xanda_mark_owner, %{ends_at: ends_at}}), do: {10, ends_at}
  defp effect_encode({:poisoned, %{ends_at: ends_at}}), do: {11, ends_at}
  defp effect_encode({:slowed, %{ends_at: ends_at}}), do: {12, ends_at}
  defp effect_encode({:fiery_rampage, %{ends_at: ends_at}}), do: {13, ends_at}
  defp effect_encode({:burned, %{ends_at: ends_at}}), do: {14, ends_at}
end
