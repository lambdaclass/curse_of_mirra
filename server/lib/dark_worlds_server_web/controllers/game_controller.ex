defmodule DarkWorldsServerWeb.GameController do
  use DarkWorldsServerWeb, :controller

  alias DarkWorldsServer.Engine
  alias DarkWorldsServer.Communication

  def current_games(conn, _params) do
    current_games_pids = Engine.list_runners_pids()

    current_games =
      Enum.map(current_games_pids, fn pid -> Communication.pid_to_external_id(pid) end)

    json(conn, %{current_games: current_games})
  end
end
