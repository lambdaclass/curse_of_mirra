defmodule DarkWorldsServerWeb.PlayWebSocket do
  @moduledoc """
  Play Websocket handler that parses msgs to be send to the runner genserver
  """
  alias DarkWorldsServer.Engine.{Runner, RequestTracker}
  alias DarkWorldsServer.Engine
  alias DarkWorldsServer.Communication

  @behaviour :cowboy_websocket

  @impl true
  def init(req, _opts) do
    game_id = :cowboy_req.binding(:game_id, req)
    player_id = :cowboy_req.binding(:player_id, req)
    {:cowboy_websocket, req, %{game_id: game_id, player_id: player_id}}
  end

  @impl true
  def websocket_init(%{game_id: :undefined}) do
    {:stop, %{}}
  end

  def websocket_init(%{player_id: :undefined}) do
    {:stop, %{}}
  end

  def websocket_init(%{game_id: game_id, player_id: player_id}) do
    runner_pid = Communication.external_id_to_pid(game_id)

    with :ok = Phoenix.PubSub.subscribe(DarkWorldsServer.PubSub, "game_play_#{game_id}"),
         true <- runner_pid in Engine.list_runners_pids(),
         {:ok, player_id} <- Runner.join(runner_pid, String.to_integer(player_id)) do
      state = %{runner_pid: runner_pid, player_id: player_id}

      {:reply,
       {:text,
        "PLAYER_ID: #{player_id} CONNECTED_TO: #{Communication.pid_to_external_id(runner_pid)}"},
       state}
    else
      false -> {:stop, %{}}
      {:error, _reason} -> {:stop, %{}}
    end
  end

  @impl true
  def terminate(_reason, _partialreq, %{runner_pid: pid, player_id: id}) do
    Runner.disconnect(pid, id)
    :ok
  end

  @impl true
  def websocket_handle({:binary, message}, state) do
    case Communication.decode(message) do
      {:ok, %{action: :ping}} ->
        {:reply, {:text, "pong"}, state}

      {:ok, action} ->
        RequestTracker.add_counter(state[:runner_pid], state[:player_id])
        Runner.play(state[:runner_pid], state[:player_id], action)
        {:reply, {:text, "OK"}, state}

      {:error, msg} ->
        {:reply, {:text, "ERROR: #{msg}"}, state}
    end
  end

  def websocket_handle(_, state) do
    {:reply, {:text, "ERROR unsupported message"}, state}
  end

  @impl true
  def websocket_info({:player_joined, player_id, _game_state}, state) do
    {:reply, {:text, "PLAYER_JOINED: #{player_id}"}, state}
  end

  def websocket_info({:game_update, game_state}, state) do
    reply_map = %{
      players: game_state.current_state.game.players
    }

    {:reply, {:binary, Communication.encode!(reply_map)}, state}
  end

  def websocket_info({:game_finished, game_state}, state) do
    reply_map = %{
      players: game_state.current_state.game.players
    }

    {:reply, {:binary, Communication.encode!(reply_map)}, state}
  end

  def websocket_info({:update_ping, player, ping}, state) do
    {:reply, {:binary, Communication.encode!({player, ping})}, state}
  end

  def websocket_info(info, state), do: {:reply, {:text, info}, state}
end
