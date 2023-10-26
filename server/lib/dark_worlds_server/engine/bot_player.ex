defmodule DarkWorldsServer.Engine.BotPlayer do
  use GenServer, restart: :transient
  require Logger
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.ActionOk
  alias DarkWorldsServer.Engine.Runner
  alias LambdaGameEngine.MyrraEngine.RelativePosition

  # This variable will dice how much time passes between bot decisions in milis
  @decide_delay_ms 500

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

    {:ok,
     %{game_pid: game_pid, bots_enabled: true, game_tick_rate: tick_rate * 2, players: [], bots: %{}, game_state: %{}}}
  end

  @impl GenServer
  def handle_cast({:add_bot, bot_id}, state) do
    send(self(), {:decide_action, bot_id})
    send(self(), {:do_action, bot_id})
    {:noreply, put_in(state, [:bots, bot_id], %{alive: true, objective: :random_movement})}
  end

  def handle_cast({:bots_enabled, toggle}, state) do
    {:noreply, %{state | bots_enabled: toggle}}
  end

  @impl GenServer
  def handle_info({:decide_action, bot_id}, state) do
    bot_state = get_in(state, [:bots, bot_id])

    new_bot_state =
      case bot_state do
        %{action: :die} ->
          bot_state

        bot_state ->
          Process.send_after(self(), {:decide_action, bot_id}, @decide_delay_ms)

          decide_action(bot_id, state.players, bot_state, state)
          |> Map.put(:objective, decide_objective(state.game_state, bot_id))
      end

    state =
      put_in(state, [:bots, bot_id], new_bot_state)

    {:noreply, state}
  end

  def handle_info({:do_action, bot_id}, state) do
    bot_state = get_in(state, [:bots, bot_id])

    if bot_state.alive do
      Process.send_after(self(), {:do_action, bot_id}, state.game_tick_rate)
      do_action(bot_id, state.game_pid, state.players, bot_state)
    end

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

    {:noreply, %{state | players: players, bots: bots, game_state: game_state.client_game_state.game}}
  end

  def handle_info(_msg, state) do
    {:noreply, state}
  end

  #############################
  # Callbacks implementations #
  #############################
  defp decide_action(_, _, %{alive: false} = bot_state, _game_state) do
    Map.put(bot_state, :action, :die)
  end

  defp decide_action(_bot_id, _players, %{objective: :random_movement} = bot_state, _game_state) do
    movement = Enum.random([{1.0, 1.0}, {-1.0, 1.0}, {1.0, -1.0}, {-1.0, -1.0}, {0.0, 0.0}])
    Map.put(bot_state, :action, {:move, movement})
  end

  defp decide_action(_bot_id, [], %{objective: :attack_enemy} = bot_state, _game_state) do
    movement = Enum.random([{1.0, 1.0}, {-1.0, 1.0}, {1.0, -1.0}, {-1.0, -1.0}, {0.0, 0.0}])
    Map.put(bot_state, :action, {:move, movement})
  end

  defp decide_action(bot_id, players, %{objective: :attack_enemy} = bot_state, _game_state) do
    case Enum.reject(players, fn player -> player.id == bot_id or player.health <= 0 end) do
      [] ->
        movement = Enum.random([{1.0, 1.0}, {-1.0, 1.0}, {1.0, -1.0}, {-1.0, -1.0}, {0.0, 0.0}])
        Map.put(bot_state, :action, {:move, movement})

      players ->
        Map.put(bot_state, :action, {:try_attack, Enum.random(players).id})
    end
  end

  defp decide_action(bot_id, players, %{objective: :flee_from_zone} = bot_state, state) do
    bot = Enum.find(players, fn player -> player.id == bot_id end)

    target =
      calculate_circle_point(
        bot.position.x,
        bot.position.y,
        state.game_state.myrra_state.shrinking_center.x,
        state.game_state.myrra_state.shrinking_center.y
      )

    Map.put(bot_state, :action, {:move, target})
  end

  defp decide_action(_bot_id, _players, bot_state, _game_state) do
    bot_state
    |> Map.put(:action, {:nothing, nil})
  end

  defp do_action(bot_id, game_pid, _players, %{action: {:move, {x, y}}}) do
    Runner.play(game_pid, bot_id, %ActionOk{action: :move_with_joystick, value: %{x: x, y: y}, timestamp: nil})
  end

  defp do_action(bot_id, game_pid, players, %{action: {:try_attack, player_id}}) do
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

  defp do_action(_bot_id, _game_pid, _players, _) do
    nil
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

  def decide_objective(%{bots_enabled: false}, _bot_id) do
    :nothing
  end

  def decide_objective(%{myrra_state: myrra_state}, bot_id) do
    bot = Enum.find(myrra_state.players, fn player -> player.id == bot_id end)

    case bot do
      nil ->
        :waiting_game_update

      bot ->
        out_of_area? = Enum.any?(bot.effects, fn {k, _v} -> k == :out_of_area end)

        if out_of_area? do
          :flee_from_zone
        else
          Enum.random([:attack_enemy, :random_movement])
        end
    end
  end

  def decide_objective(_, _), do: :nothing
end
