defmodule DarkWorldsServerWeb.SessionController do
  use DarkWorldsServerWeb, :controller
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine

  def new(conn, _params) do
    {:ok, engine_config_json} =
      Application.app_dir(:lambda_game_engine, "priv/config.json") |> File.read()

    engine_config = LambdaGameEngine.parse_config(engine_config_json)

    {:ok, runner_pid} = Engine.start_child(engine_config)

    headers = Enum.into(conn.req_headers, %{})
    session_id = Communication.pid_to_external_id(runner_pid)

    if headers["content-type"] == "application/json" do
      json(conn, %{session_id: session_id})
    else
      redirect(conn, to: "/board/#{session_id}")
    end
  end
end
