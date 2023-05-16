defmodule DarkWorldsServerWeb.LobbyWebsocket do
  alias DarkWorldsServer.Matchmaking
  alias DarkWorldsServer.Communication

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

    {:reply, {:text, "CONNECTED_TO: #{lobby_id} YOU'RE PLAYER #{player_id}"},
     %{lobby_pid: matchmaking_session_pid}}
  end

  def websocket_handle({:text, "START_GAME"}, state) do
    Matchmaking.start_game(state[:lobby_pid])
    {:reply, {:text, "STARTING_GAME..."}, state}
  end

  def websocket_info({:player_added, id}, state) do
    {:reply, {:text, "JOINED PLAYER: #{id}"}, state}
  end

  def websocket_info({:game_started, game_pid}, state) do
    {:reply, {:text, "GAME_ID: #{Communication.pid_to_external_id(game_pid)}"}, state}
  end

  def websocket_info({:amount_of_players, players}, state) do
    {:reply, {:text, "AMOUNT_OF_PLAYERS: #{players}"}, state}
  end
end
