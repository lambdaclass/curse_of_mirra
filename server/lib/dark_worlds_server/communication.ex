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

  def lobby_game_started!(%{game_pid: game_pid, game_config: game_config}) do
    game_id = pid_to_external_id(game_pid)
    # game_config = remove_unknown_fields(game_config)

    %LobbyEvent{type: :GAME_STARTED, game_id: game_id, game_config: game_config}
    |> LobbyEvent.encode()
  end

  def encode!(%{players: players}) do
    %GameEvent{type: :STATE_UPDATE, players: players}
    |> GameEvent.encode()
  end

  def encode!(%{latency: latency}) do
    %GameEvent{type: :PING_UPDATE, latency: latency}
    |> GameEvent.encode()
  end

  def game_player_joined(player_id) do
    %GameEvent{type: :PLAYER_JOINED, player_joined_id: player_id}
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

  def remove_unknown_fields(map) when is_map(map) do
    map
    |> Enum.reduce(%{}, fn {key, value}, acc ->
      case key do
        :__unknown_fields__ ->
          acc

        key ->
          Map.put(acc, key, remove_unknown_fields(value))
      end
    end)
  end

  def remove_unknown_fields(list) when is_list(list) do
    list
    |> Enum.reduce([], fn element, acc ->
      [remove_unknown_fields(element) | acc]
    end)
  end

  def remove_unknown_fields(value), do: value
end
