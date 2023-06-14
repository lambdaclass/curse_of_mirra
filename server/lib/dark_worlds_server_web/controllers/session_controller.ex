defmodule DarkWorldsServerWeb.SessionController do
  use DarkWorldsServerWeb, :controller
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine

  def new(conn, _params) do
    {:ok, runner_pid} =
      Engine.start_child(%{
        players: [1],
        game_config: %{
          runner_config: %{
            board_width: 1000,
            board_height: 1000,
            server_tickrate_ms: 30,
            game_timeout_ms: 1_200_000
          }
        }
      })

    headers = Enum.into(conn.req_headers, %{})
    session_id = Communication.pid_to_external_id(runner_pid)

    if headers["content-type"] == "application/json" do
      json(conn, %{session_id: session_id})
    else
      redirect(conn, to: "/board/#{session_id}")
    end
  end
end
