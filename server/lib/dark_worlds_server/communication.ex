defmodule DarkWorldsServer.Communication do
  alias DarkWorldsServer.Communication.Proto.ClientAction
  alias DarkWorldsServer.Communication.Proto.GameEvent
  alias DarkWorldsServer.Communication.Proto.LobbyEvent

  @moduledoc """
  The Communication context
  """

  def lobby_connected!(lobby_id, player_id) do
    %LobbyEvent{type: :CONNECTED, lobby_id: lobby_id, player_id: player_id}
    |> LobbyEvent.encode()
  end

  def lobby_player_added!(player_id, players) do
    %LobbyEvent{type: :PLAYER_ADDED, added_player_id: player_id, players: players}
    |> LobbyEvent.encode()
  end

  def lobby_player_removed!(player_id, players) do
    %LobbyEvent{type: :PLAYER_REMOVED, removed_player_id: player_id, players: players}
    |> LobbyEvent.encode()
  end

  def lobby_player_count!(count) do
    %LobbyEvent{type: :PLAYER_COUNT, player_count: count}
    |> LobbyEvent.encode()
  end

  def lobby_game_started!(%{game_pid: game_pid, game_config: game_config, server_hash: server_hash}) do
    game_id = pid_to_external_id(game_pid)

    %LobbyEvent{type: :GAME_STARTED, game_id: game_id, game_config: game_config, server_hash: server_hash}
    |> LobbyEvent.encode()
  end

  def game_update!(%{
        players: players,
        projectiles: projectiles,
        killfeed: killfeed,
        player_timestamp: player_timestamp,
        server_timestamp: server_timestamp
      }) do
    %GameEvent{
      type: :STATE_UPDATE,
      players: players,
      projectiles: projectiles,
      killfeed: killfeed,
      player_timestamp: player_timestamp,
      server_timestamp: server_timestamp
    }
    |> GameEvent.encode()
  end

  def encode!(%{latency: latency}) do
    %GameEvent{type: :PING_UPDATE, latency: latency}
    |> GameEvent.encode()
  end

  def game_finished!(%{winner: winner, players: players}) do
    %GameEvent{winner_player: winner, type: :GAME_FINISHED, players: players}
    |> GameEvent.encode()
  end

  def game_player_joined(player_id) do
    %GameEvent{type: :PLAYER_JOINED, player_joined_id: player_id}
    |> GameEvent.encode()
  end

  def initial_positions(players) do
    %GameEvent{type: :INITIAL_POSITIONS, players: players}
    |> GameEvent.encode()
  end

  def selected_characters!(selected_characters) do
    %GameEvent{type: :SELECTED_CHARACTER_UPDATE, selected_characters: selected_characters}
    |> GameEvent.encode()
  end

  def finish_character_selection!(selected_characters, players) do
    %GameEvent{
      type: :FINISH_CHARACTER_SELECTION,
      selected_characters: selected_characters,
      players: players
    }
    |> GameEvent.encode()
  end

  def decode(value) do
    try do
      {:ok, ClientAction.decode(value)}
    rescue
      Protobuf.DecodeError -> {:error, :error_decoding}
    end
  end

  def lobby_decode(value) do
    try do
      {:ok, LobbyEvent.decode(value)}
    rescue
      Protobuf.DecodeError -> {:error, :error_decoding}
    end
  end

  def pid_to_external_id(pid) when is_pid(pid) do
    pid |> :erlang.term_to_binary() |> Base58.encode()
  end

  def external_id_to_pid(external_id) do
    external_id |> Base58.decode() |> :erlang.binary_to_term([:safe])
  end

  def pubsub_game_topic(game_pid) when is_pid(game_pid) do
    "game_play_#{pid_to_external_id(game_pid)}"
  end
end
