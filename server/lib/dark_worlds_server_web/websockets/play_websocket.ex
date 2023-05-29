defmodule DarkWorldsServerWeb.PlayWebSocket do
  @moduledoc """
  Play Websocket handler that parses msgs to be send to the runner genserver
  """
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine
  alias DarkWorldsServer.Engine.RequestTracker
  alias DarkWorldsServer.Engine.Runner

  require Logger

  @behaviour :cowboy_websocket
  @ping_interval_ms 500

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

    with :ok <- Phoenix.PubSub.subscribe(DarkWorldsServer.PubSub, "game_play_#{game_id}"),
         true <- runner_pid in Engine.list_runners_pids(),
         {:ok, player_id} <- Runner.join(runner_pid, String.to_integer(player_id)) do
      state = %{runner_pid: runner_pid, player_id: player_id}

      Process.send_after(self(), :send_ping, @ping_interval_ms)

      {:reply, {:text, "PLAYER_ID: #{player_id} CONNECTED_TO: #{Communication.pid_to_external_id(runner_pid)}"}, state}
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
      {:ok, action} ->
        RequestTracker.add_counter(state[:runner_pid], state[:player_id])
        Runner.play(state[:runner_pid], state[:player_id], action)
        {:reply, {:text, "OK"}, state}

      {:error, msg} ->
        {:reply, {:text, "ERROR: #{msg}"}, state}
    end
  end

  def websocket_handle(:pong, state) do
    last_ping_time = state.last_ping_time
    time_now = Time.utc_now()
    latency = Time.diff(time_now, last_ping_time, :millisecond)
    # Send back the player's ping
    {:reply, {:binary, Communication.encode!(%{latency: latency})}, state}
  end

  def websocket_handle(_, state) do
    {:reply, {:text, "ERROR unsupported message"}, state}
  end

  @impl true
  def websocket_info({:player_joined, player_id, _game_state}, state) do
    {:reply, {:text, "PLAYER_JOINED: #{player_id}"}, state}
  end

  # Send a ping frame every once in a while
  def websocket_info(:send_ping, state) do
    Process.send_after(self(), :send_ping, @ping_interval_ms)
    time_now = Time.utc_now()
    {:reply, :ping, Map.put(state, :last_ping_time, time_now)}
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

    Logger.info("THE GAME HAS FINISHED")

    {:reply, {:binary, Communication.encode!(reply_map)}, state}
  end

  # TODO: Use protobuf
  def websocket_info({:next_round, _game_state}, state) do
    Logger.info("A NEW ROUND STARTED")
    {:reply, {:text, "NEXT_ROUND"}, state}
  end

  # TODO: Use protobuf
  def websocket_info({:last_round, _game_state}, state) do
    Logger.info("THE LAST ROUND STARTED")
    {:reply, {:text, "LAST_ROUND"}, state}
  end

  def websocket_info(info, state), do: {:reply, {:text, info}, state}
end
