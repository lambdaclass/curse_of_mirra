defmodule DarkWorldsServer.Engine.BotPlayer do
  use GenServer, restart: :transient
  require Logger
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.ActionOk
  alias DarkWorldsServer.Engine.Runner
  alias LambdaGameEngine.MyrraEngine.RelativePosition
  alias LambdaGameEngine.MyrraEngine.Position

  # This variable will dice how much time passes between bot decisions in milis
  @decide_delay_ms 500

  # We'll decide the view range of a bot
  @visibility_max_range 2000

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

          closest_entity = get_closes_entity(state.game_state, bot_id)

          decide_action(bot_id, state.players, bot_state, state, closest_entity)
          |> Map.put(:objective, decide_objective(state.game_state, bot_id, closest_entity))
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
  defp decide_action(_, _, %{alive: false} = bot_state, _game_state, _closest_entity) do
    Map.put(bot_state, :action, :die)
  end

  defp decide_action(_bot_id, _players, %{objective: :random_movement} = bot_state, _game_state, _closest_entity) do
    movement = Enum.random([{1.0, 1.0}, {-1.0, 1.0}, {1.0, -1.0}, {-1.0, -1.0}, {0.0, 0.0}])
    Map.put(bot_state, :action, {:move, movement})
  end

  defp decide_action(_bot_id, _players, %{objective: :attack_enemy} = bot_state, _game_state, closest_entity) do
    Map.put(bot_state, :action, {:try_attack, closest_entity})
  end

  defp decide_action(bot_id, players, %{objective: :flee_from_zone} = bot_state, state, _closest_entity) do
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

  defp decide_action(_bot_id, _players, bot_state, _game_state, _closest_entity) do
    bot_state
    |> Map.put(:action, {:nothing, nil})
  end

  defp do_action(bot_id, game_pid, _players, %{action: {:move, {x, y}}}) do
    Runner.play(game_pid, bot_id, %ActionOk{action: :move_with_joystick, value: %{x: x, y: y}, timestamp: nil})
  end

  defp do_action(bot_id, game_pid, _players, %{
         action: {:try_attack, %{direction_to_entity: {x, y}} = direction_to_entity}
       }) do
    if direction_to_entity.distance_to_entity > 100 do
      Runner.play(game_pid, bot_id, %ActionOk{action: :move_with_joystick, value: %{x: x, y: y}, timestamp: nil})
    else
      Runner.play(game_pid, bot_id, %ActionOk{
        action: :basic_attack,
        value: %RelativePosition{x: x, y: y},
        timestamp: nil
      })
    end
  end

  defp do_action(_bot_id, _game_pid, _players, _) do
    nil
  end

  ####################
  # Internal helpers #
  ####################
  def calculate_circle_point(%Position{x: start_x, y: start_y}, %Position{x: end_x, y: end_y}) do
    calculate_circle_point(start_x, start_y, end_x, end_y)
  end

  def calculate_circle_point(cx, cy, x, y) do
    radius = 1
    angle = Nx.atan2(x - cx, y - cy)
    x = Nx.cos(angle) |> Nx.multiply(radius) |> Nx.to_number()
    y = Nx.sin(angle) |> Nx.multiply(radius) |> Nx.to_number()
    {x, -y}
  end

  def decide_objective(%{bots_enabled: false}, _bot_id, _closest_entity) do
    :nothing
  end

  def decide_objective(%{myrra_state: myrra_state}, bot_id, closest_entity) do
    bot = Enum.find(myrra_state.players, fn player -> player.id == bot_id end)

    case bot do
      nil ->
        :waiting_game_update

      bot ->
        out_of_area? = Enum.any?(bot.effects, fn {k, _v} -> k == :out_of_area end)

        cond do
          out_of_area? ->
            :flee_from_zone

          not Enum.empty?(closest_entity) ->
            :attack_enemy

          true ->
            :random_movement
        end
    end
  end

  def decide_objective(_, _, _), do: :nothing

  defp get_closes_entity(%{myrra_state: game_state}, bot_id) do
    bot = Enum.find(game_state.players, fn player -> player.id == bot_id end)

    case bot do
      nil ->
        %{}
      bot ->
        players_distances =
          game_state.players
          |> Enum.filter(fn player -> player.status == :alive and player.id != bot.id end)
          |> map_entities(bot)

        loots_distances =
          game_state.loots
          |> map_entities(bot)

        cond do
          Enum.empty?(loots_distances) and Enum.empty?(players_distances) ->
            %{}

          Enum.empty?(loots_distances) ->
            hd(players_distances)

          Enum.empty?(players_distances) ->
            hd(loots_distances)

          true ->
            Enum.min_by([hd(loots_distances), hd(players_distances)], fn entity -> entity.distance_to_entity end)
        end
    end
  end

  defp get_closes_entity(_, _ ) do
    %{}
  end

  defp get_distance_to_point(%Position{x: start_x, y: start_y}, %Position{x: end_x, y: end_y}) do
    :math.sqrt(:math.pow(end_x - start_x, 2) + :math.pow(end_y - start_y, 2))
  end

  defp map_entities(entities, bot) do
    entities
    |> Enum.map(fn entity ->
      %{
        entity_id: entity.id,
        distance_to_entity: get_distance_to_point(bot.position, entity.position),
        direction_to_entity: calculate_circle_point(bot.position, entity.position)
      }
    end)
    |> Enum.sort_by(fn distances -> distances.distance_to_entity end, :asc)
    |> Enum.filter(fn distances -> distances.distance_to_entity <= @visibility_max_range end)
  end
end
