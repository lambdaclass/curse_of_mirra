defmodule DarkWorldsServer.Engine.BotPlayer do
  use GenServer, restart: :transient
  require Logger
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.ActionOk
  alias DarkWorldsServer.Engine.Runner

  #######
  # API #
  #######
  def start_link(game_pid, tick_rate) do
    GenServer.start_link(__MODULE__, {game_pid, tick_rate})
  end

  def add_bot(bot_pid, bot_id) do
    GenServer.cast(bot_pid, {:add_bot, bot_id})
  end

  def enable_bots(bot_pid) do
    GenServer.cast(bot_pid, {:bots_enabled, true})
  end

  def disable_bots(bot_pid) do
    GenServer.cast(bot_pid, {:bots_enabled, false})
  end

  #######################
  # GenServer callbacks #
  #######################
  @impl GenServer
  def init({game_pid, tick_rate}) do
    game_id = Communication.pid_to_external_id(game_pid)
    Phoenix.PubSub.subscribe(DarkWorldsServer.PubSub, "game_play_#{game_id}")
    {:ok, %{game_pid: game_pid, bots_enabled: true, game_tick_rate: tick_rate * 2, bots: %{}}}
  end

  @impl GenServer
  def handle_cast({:add_bot, bot_id}, state) do
    send(self(), {:decide_action, bot_id})
    send(self(), {:do_action, bot_id})
    {:noreply, put_in(state, [:bots, bot_id], %{alive: true})}
  end

  def handle_cast({:bots_enabled, toggle}, state) do
    {:noreply, %{state | bots_enabled: toggle}}
  end

  @impl GenServer
  def handle_info({:decide_action, bot_id}, state) do
    bot_state = get_in(state, [:bots, bot_id])
    new_bot_state = decide_action(bot_id, bot_state)
    state = put_in(state, [:bots, bot_id], new_bot_state)
    {:noreply, state}
  end

  def handle_info({:do_action, bot_id}, %{bots_enabled: false} = state) do
    Process.send_after(self(), {:do_action, bot_id}, state.game_tick_rate)
    {:noreply, state}
  end

  def handle_info({:do_action, bot_id}, state) do
    bot_state = get_in(state, [:bots, bot_id])
    do_action(bot_id, state.game_pid, state.game_tick_rate, bot_state)
    {:noreply, state}
  end

  def handle_info(_msg, state) do
    {:noreply, state}
  end

  #############################
  # Callbacks implementations #
  #############################
  defp decide_action(_, %{alive: false} = bot_state) do
    bot_state
  end

  defp decide_action(bot_id, bot_state) do
    Process.send_after(self(), {:decide_action, bot_id}, 5_000)
    [movement] = Enum.take_random([{1.0, 1.0}, {-1.0, 1.0}, {1.0, -1.0}, {-1.0, -1.0}, {0.0, 0.0}], 1)
    Map.put(bot_state, :action, {:move, movement})
  end

  defp do_action(_, _, _, %{alive: false}) do
    :noop
  end

  defp do_action(bot_id, game_pid, action_tick_rate, %{action: {:move, {x, y}}}) do
    Process.send_after(self(), {:do_action, bot_id}, action_tick_rate)
    Runner.play(game_pid, bot_id, %ActionOk{action: :move_with_joystick, value: %{x: x, y: y}, timestamp: nil})
  end
end
