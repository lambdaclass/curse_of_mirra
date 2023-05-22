defmodule DarkWorldsServerWeb.LobbyWebsocket do
  require Logger
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Matchmaking

  @behaviour :cowboy_websocket

  def init(req, _opts) do
    lobby_id = :cowboy_req.binding(:lobby_id, req)
    {:cowboy_websocket, req, %{lobby_id: lobby_id}}
  end

  def websocket_init(%{lobby_id: :undefined}) do
    {:stop, %{}}
  end

  def websocket_init(%{lobby_id: lobby_id}) do
    Phoenix.PubSub.subscribe(DarkWorldsServer.PubSub, Matchmaking.session_topic(lobby_id))

    matchmaking_session_pid = Communication.external_id_to_pid(lobby_id)
    players = Matchmaking.list_players(matchmaking_session_pid)
    player_id = Enum.count(players) + 1
    Matchmaking.add_player(player_id, matchmaking_session_pid)

    {:reply, {:binary, Communication.lobby_connected!(lobby_id, player_id)}, %{lobby_pid: matchmaking_session_pid}}
  end

  def websocket_handle({:binary, message}, state) do
    case Communication.lobby_decode(message) do
      {:ok, %{type: :START_GAME}} ->
        Matchmaking.start_game(state[:lobby_pid])
        {:ok, state}

      {:error, msg} ->
        Logger.error("Received frame with an invalid message: #{msg}")
        {:ok, state}
    end
  end

  def websocket_info({:player_added, id}, state) do
    {:reply, {:binary, Communication.lobby_player_added!(id)}, state}
  end

  def websocket_info({:game_started, game_pid}, state) do
    {:reply, {:binary, Communication.lobby_game_started!(game_pid)}, state}
  end

  def websocket_info({:amount_of_players, count}, state) do
    {:reply, {:binary, Communication.lobby_player_count!(count)}, state}
  end
end
