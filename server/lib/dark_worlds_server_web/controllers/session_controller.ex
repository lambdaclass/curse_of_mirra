defmodule DarkWorldsServerWeb.SessionController do
  use DarkWorldsServerWeb, :controller
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine

  def new(conn, _params) do
    {:ok, runner_pid} = Engine.start_child(%{players: []})

    headers = Enum.into(conn.req_headers, %{})
    session_id = Communication.pid_to_external_id(runner_pid)

    if headers["content-type"] == "application/json" do
      json(conn, %{session_id: session_id})
    else
      redirect(conn, to: "/board/#{session_id}")
    end
  end
end
