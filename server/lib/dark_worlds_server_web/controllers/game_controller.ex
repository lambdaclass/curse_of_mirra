defmodule DarkWorldsServerWeb.GameController do
  use DarkWorldsServerWeb, :controller

  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine
  alias DarkWorldsServer.Engine.PlayerTracker

  def current_games(conn, _params) do
    current_games_pids = Engine.list_runners_pids()

    current_games = Enum.map(current_games_pids, fn pid -> Communication.pid_to_external_id(pid) end)

    json(conn, %{current_games: current_games})
  end

  def player_game(conn, %{"player_id" => player_id}) do
    case PlayerTracker.get_player_game(player_id) do
      nil -> json(conn, %{current_game: nil})
      game_pid -> json(conn, %{current_game: Communication.pid_to_external_id(game_pid)})
    end
  end
end
