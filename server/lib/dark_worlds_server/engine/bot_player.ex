defmodule DarkWorldsServer.Engine.BotPlayer do
  use GenServer, restart: :transient
  require Logger
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.ActionOk
  alias DarkWorldsServer.Engine.Runner
  alias LambdaGameEngine.MyrraEngine.RelativePosition

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
    {:ok, %{game_pid: game_pid, bots_enabled: true, game_tick_rate: tick_rate * 2, players: [], bots: %{}}}
  end

  @impl GenServer
  def handle_cast({:add_bot, bot_id}, state) do
    send(self(), {:decide_action, bot_id})
    send(self(), {:do_action, bot_id})
    objective = Enum.random([:random_movement, :attack_player])
    {:noreply, put_in(state, [:bots, bot_id], %{alive: true, objective: objective})}
  end

  def handle_cast({:bots_enabled, toggle}, state) do
    {:noreply, %{state | bots_enabled: toggle}}
  end

  @impl GenServer
  def handle_info({:decide_action, bot_id}, state) do
    bot_state = get_in(state, [:bots, bot_id])
    new_bot_state = decide_action(bot_id, state.players, bot_state)
    state = put_in(state, [:bots, bot_id], new_bot_state)
    {:noreply, state}
  end

  def handle_info({:do_action, bot_id}, %{bots_enabled: false} = state) do
    Process.send_after(self(), {:do_action, bot_id}, state.game_tick_rate)
    {:noreply, state}
  end

  def handle_info({:do_action, bot_id}, state) do
    bot_state = get_in(state, [:bots, bot_id])
    do_action(bot_id, state.game_pid, state.game_tick_rate, state.players, bot_state)
    {:noreply, state}
  end

  def handle_info({:game_update, game_state}, state) do
    players =
      game_state.client_game_state.game.myrra_state.players
      |> Enum.map(&Map.take(&1, [:id, :health, :position]))
      |> Enum.sort_by(& &1.health, :desc)

    bots =
      Enum.reduce(players, state.bots, fn player, acc_bots ->
        case {player.health <= 0, acc_bots[player.id]} do
          {true, bot} when not is_nil(bot) -> put_in(acc_bots, [player.id, :alive], false)
          _ -> acc_bots
        end
      end)

    Enum.each(bots, fn {bot_id, _} -> send(self(), {:think_and_do, bot_id}) end)

    {:noreply, %{state | players: players, bots: bots}}
  end

  def handle_info(_msg, state) do
    {:noreply, state}
  end

  #############################
  # Callbacks implementations #
  #############################
  defp decide_action(_, _, %{alive: false} = bot_state) do
    bot_state
  end

  defp decide_action(bot_id, _, %{objective: :random_movement} = bot_state) do
    Process.send_after(self(), {:decide_action, bot_id}, 5_000)
    [movement] = Enum.take_random([{1.0, 1.0}, {-1.0, 1.0}, {1.0, -1.0}, {-1.0, -1.0}, {0.0, 0.0}], 1)
    Map.put(bot_state, :action, {:move, movement})
  end

  defp decide_action(bot_id, [], %{objective: :attack_player} = bot_state) do
    Process.send_after(self(), {:decide_action, bot_id}, 5_000)
    [movement] = Enum.take_random([{1.0, 1.0}, {-1.0, 1.0}, {1.0, -1.0}, {-1.0, -1.0}, {0.0, 0.0}], 1)
    Map.put(bot_state, :action, {:move, movement})
  end

  defp decide_action(bot_id, players, %{objective: :attack_player} = bot_state) do
    Process.send_after(self(), {:decide_action, bot_id}, 5_000)

    case Enum.reject(players, fn player -> player.id == bot_id or player.health <= 0 end) do
      [player | _] ->
        Map.put(bot_state, :action, {:try_attack, player.id})

      _ ->
        [movement] = Enum.take_random([{1.0, 1.0}, {-1.0, 1.0}, {1.0, -1.0}, {-1.0, -1.0}, {0.0, 0.0}], 1)
        Map.put(bot_state, :action, {:move, movement})
    end
  end

  defp do_action(_, _, _, _, %{alive: false}) do
    :noop
  end

  defp do_action(bot_id, game_pid, action_tick_rate, _players, %{action: {:move, {x, y}}}) do
    Process.send_after(self(), {:do_action, bot_id}, action_tick_rate)
    Runner.play(game_pid, bot_id, %ActionOk{action: :move_with_joystick, value: %{x: x, y: y}, timestamp: nil})
  end

  defp do_action(bot_id, game_pid, action_tick_rate, players, %{action: {:try_attack, player_id}}) do
    Process.send_after(self(), {:do_action, bot_id}, action_tick_rate)

    ## TODO: Not ideal to do this iteration over the lists twice, but not the worst considering the size
    player = Enum.find(players, fn player -> player.id == player_id end)
    bot = Enum.find(players, fn player -> player.id == bot_id end)

    case bot do
      nil ->
        :waiting_game_update

      bot ->
        x_distance = player.position.x - bot.position.x
        y_distance = player.position.y - bot.position.y

        {x, y} = calculate_circle_point(bot.position.x, bot.position.y, player.position.x, player.position.y)

        if abs(x_distance) >= 20 and abs(y_distance) >= 20 do
          Runner.play(game_pid, bot_id, %ActionOk{action: :move_with_joystick, value: %{x: x, y: y}, timestamp: nil})
        else
          Runner.play(game_pid, bot_id, %ActionOk{
            action: :basic_attack,
            value: %RelativePosition{x: x, y: y},
            timestamp: nil
          })
        end
    end
  end

  ####################
  # Internal helpers #
  ####################
  def calculate_circle_point(cx, cy, x, y) do
    radius = 1
    angle = Nx.atan2(x - cx, y - cy)
    x = Nx.cos(angle) |> Nx.multiply(radius) |> Nx.to_number()
    y = Nx.sin(angle) |> Nx.multiply(radius) |> Nx.to_number()
    {x, -y}
  end
end
