defmodule DarkWorldsServerWeb.PlayWebSocket do
  @moduledoc """
  Play Websocket handler that parses msgs to be send to the runner genserver
  """
  alias DarkWorldsServer.Engine.Runner
  alias DarkWorldsServer.Engine
  alias DarkWorldsServer.Communication

  @behaviour :cowboy_websocket

  def init(req, _opts) do
    game_id = :cowboy_req.binding(:game_id, req)
    {:cowboy_websocket, req, %{game_id: game_id}}
  end

  def websocket_init(%{game_id: :undefined}) do
    {:stop, %{}}
  end

  def websocket_init(%{game_id: game_id}) do
    runner_pid = Communication.external_id_to_pid(game_id)

    with :ok = Phoenix.PubSub.subscribe(DarkWorldsServer.PubSub, "game_play_#{game_id}"),
         true <- runner_pid in Engine.list_runners_pids(),
         {:ok, player_id} <- Runner.join(runner_pid) do
      state = %{runner_pid: runner_pid, player_id: player_id}
      {:reply, {:text, "CONNECTED_TO: #{Communication.pid_to_external_id(runner_pid)}"}, state}
    else
      false -> {:stop, %{}}
      {:error, _reason} -> {:stop, %{}}
    end
  end

  def websocket_handle({:text, message}, state) do
    case Communication.decode(message) do
      {:ok, %{action: :ping}} ->
        {:reply, {:text, "pong"}, state}

      {:ok, action} ->
        Runner.play(state[:runner_pid], state[:player_id], action)
        {:reply, {:text, "OK"}, state}

      {:error, msg} ->
        {:reply, {:text, "ERROR: #{msg}"}, state}
    end
  end

  def websocket_info({:game_update, game_state}, state) do
    reply_map = %{
      players: game_state.current_state.game.players
    }

    {:reply, {:text, Communication.encode!(reply_map)}, state}
  end

  def websocket_info({:game_finished, game_state}, state) do
    reply_map = %{
      players: game_state.current_state.game.players
    }

    {:reply, {:text, Communication.encode!(reply_map)}, state}
  end

  def websocket_info({:update_ping, player, ping}, state) do
    {:reply, {:text, Communication.encode!({player,  ping})}, state}
  end

  def websocket_info(info, state), do: {:reply, {:text, info}, state}
end
