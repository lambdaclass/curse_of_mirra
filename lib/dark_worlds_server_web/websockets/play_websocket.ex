defmodule DarkWorldsServerWeb.PlayWebSocket do
  @moduledoc """
  Play Websocket handler that parses msgs to be send to the runner genserver
  """
  alias DarkWorldsServer.Engine
  alias DarkWorldsServer.Engine.{ActionRaw, ActionOk, Runner}

  @behaviour :cowboy_websocket

  def init(req, _opts) do
    game_id = :cowboy_req.binding(:game_id, req)
    {:cowboy_websocket, req, %{game_id: game_id}}
  end

  def websocket_init(%{game_id: :undefined}) do
    {:ok, runner_pid} = Engine.start_child()
    {:ok, %{runner_pid: runner_pid}}
  end

  def websocket_init(%{game_id: encoded_game_id}) do
    runner_pid = Base.decode64!(encoded_game_id) |> :erlang.binary_to_term([:safe])
    {:ok, %{runner_pid: runner_pid}}
  end

  def websocket_handle({:text, message}, state) do
    case ActionRaw.from_json(message) do
      {:ok, action} ->
        IO.inspect(action)

        case ActionOk.from_action_raw(action) do
          {:ok, action} ->
            Runner.play(state[:runner_pid], action)
            {:reply, {:text, "OK"}, state}

          {:error, msg} ->
            {:reply, {:text, "ERROR: #{msg}"}, state}
        end

      {:error, _error} ->
        {:reply, {:text, "ERROR: Invalid json"}, state}
    end
  end

  def websocket_info(info, state), do: {:reply, {:text, info}, state}
end
