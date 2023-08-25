defmodule DarkWorldsServerWeb.LobbyController do
  use DarkWorldsServerWeb, :controller

  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Matchmaking
  alias DarkWorldsServer.Matchmaking.MatchingSupervisor

  @server_hash Application.compile_env(:dark_worlds_server, :information)
               |> Keyword.get(:version_hash)

  def new(conn, _params) do
    matchmaking_session_pid = Matchmaking.create_session()
    lobby_id = Communication.pid_to_external_id(matchmaking_session_pid)
    json(conn, %{lobby_id: lobby_id})
  end

  def current_lobbies(conn, _params) do
    matchmaking_pids = MatchingSupervisor.children_pids()
    lobbies = Enum.map(matchmaking_pids, fn pid -> Communication.pid_to_external_id(pid) end)

    json(conn, %{lobbies: lobbies, server_version: @server_hash})
  end
end
