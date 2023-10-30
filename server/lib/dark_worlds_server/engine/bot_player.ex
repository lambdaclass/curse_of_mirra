defmodule DarkWorldsServer.Engine.BotPlayer do
  use GenServer, restart: :transient
  require Logger
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.ActionOk
  alias DarkWorldsServer.Engine.Runner
  alias LambdaGameEngine.MyrraEngine.RelativePosition

  # This variable will decide how much time passes between bot decisions in milis
  @decide_delay_ms 500

  # We'll decide the view range of a bot measured in grid cells
  # e.g. from {x=1, y=1} to {x=5, y=1} you have 4 cells
  @visibility_max_range_cells 2000

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

    {:ok, %{game_pid: game_pid, bots_enabled: true, game_tick_rate: tick_rate, players: [], bots: %{}, game_state: %{}}}
  end

  @impl GenServer
  def handle_cast({:add_bot, bot_id}, state) do
    send(self(), {:decide_action, bot_id})
    send(self(), {:do_action, bot_id})

    {:noreply, put_in(state, [:bots, bot_id], %{alive: true, objective: :nothing, current_wandering_position: nil})}
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

          closest_entity = get_closest_entity(state.game_state, bot_id)

          bot_state
          |> decide_action(bot_id, state.players, state, closest_entity)
          |> decide_objective(state, bot_id, closest_entity)
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
  defp decide_action(%{alive: false} = bot_state, _, _, _game_state, _closest_entity) do
    Map.put(bot_state, :action, :die)
  end

  defp decide_action(
         %{objective: :wander, current_wandering_position: wandering_position} = bot_state,
         bot_id,
         players,
         _game_state,
         _closest_entity
       ) do
    bot = Enum.find(players, fn player -> player.id == bot_id end)

    if bot do
      target =
        calculate_circle_point(
          bot.position,
          wandering_position
        )

      Map.put(bot_state, :action, {:move, target})
    else
      Map.put(bot_state, :action, {:nothing, nil})
    end
  end

  defp decide_action(%{objective: :attack_enemy} = bot_state, _bot_id, _players, _game_state, closest_entity) do
    Map.put(bot_state, :action, {:try_attack, closest_entity})
  end

  defp decide_action(%{objective: :flee_from_zone} = bot_state, bot_id, players, state, _closest_entity) do
    bot = Enum.find(players, fn player -> player.id == bot_id end)

    target =
      calculate_circle_point(
        bot.position,
        state.game_state.myrra_state.shrinking_center
      )

    Map.put(bot_state, :action, {:move, target})
  end

  defp decide_action(bot_state, _bot_id, _players, _game_state, _closest_entity) do
    bot_state
    |> Map.put(:action, {:nothing, nil})
  end

  defp do_action(bot_id, game_pid, _players, %{action: {:move, {x, y}}}) do
    Runner.play(game_pid, bot_id, %ActionOk{action: :move_with_joystick, value: %{x: x, y: y}, timestamp: nil})
  end

  # Entity detected is an enemy we should try an attack
  defp do_action(bot_id, game_pid, _players, %{
         action: {:try_attack, %{type: :enemy, direction_to_entity: {x, y}} = direction_to_entity}
       }) do
    # TODO replace this 400 with a function that determines if any skill would hit the enemy
    # If the entity detected is in attack range we should perfom an attack
    if direction_to_entity.distance_to_entity > 400 do
      Runner.play(game_pid, bot_id, %ActionOk{action: :move_with_joystick, value: %{x: x, y: y}, timestamp: nil})
    else
      Runner.play(game_pid, bot_id, %ActionOk{
        action: :basic_attack,
        value: %RelativePosition{x: x, y: y},
        timestamp: nil
      })
    end
  end

  # Entity detected is a loot crate we should try to pick it
  defp do_action(bot_id, game_pid, _players, %{
         action: {:try_attack, %{direction_to_entity: {x, y}}}
       }) do
    Runner.play(game_pid, bot_id, %ActionOk{action: :move_with_joystick, value: %{x: x, y: y}, timestamp: nil})
  end

  defp do_action(_bot_id, _game_pid, _players, _) do
    nil
  end

  ####################
  # Internal helpers #
  ####################
  def calculate_circle_point(%{x: start_x, y: start_y}, %{x: end_x, y: end_y}) do
    calculate_circle_point(start_x, start_y, end_x, end_y)
  end

  def calculate_circle_point(cx, cy, x, y) do
    radius = 1
    angle = Nx.atan2(x - cx, y - cy)
    x = Nx.cos(angle) |> Nx.multiply(radius) |> Nx.to_number()
    y = Nx.sin(angle) |> Nx.multiply(radius) |> Nx.to_number()
    {x, -y}
  end

  def decide_objective(bot_state, %{bots_enabled: false}, _bot_id, _closest_entity) do
    Map.put(bot_state, :objective, :nothing)
  end

  def decide_objective(bot_state, %{game_state: %{myrra_state: myrra_state}}, bot_id, closest_entity) do
    bot = Enum.find(myrra_state.players, fn player -> player.id == bot_id end)

    objective =
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
              :wander
          end
      end

    if objective == :wander do
      maybe_put_wandering_position(bot_state, bot, myrra_state)
    else
      Map.put(bot_state, :objective, objective)
    end
  end

  def decide_objective(bot_state, _, _, _), do: Map.put(bot_state, :objective, :nothing)

  defp get_closest_entity(%{myrra_state: game_state}, bot_id) do
    # TODO maybe we could add a priority to the entities.
    # e.g. if the bot has low health priorities the loot boxes
    bot = Enum.find(game_state.players, fn player -> player.id == bot_id end)

    case bot do
      nil ->
        %{}

      bot ->
        players_distances =
          game_state.players
          |> Enum.filter(fn player -> player.status == :alive and player.id != bot.id end)
          |> map_entities(bot, :enemy)

        loots_distances =
          game_state.loots
          |> map_entities(bot, :loot)

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

  defp get_closest_entity(_, _) do
    %{}
  end

  defp get_distance_to_point(%{x: start_x, y: start_y}, %{x: end_x, y: end_y}) do
    diagonal_movement_cost = 14
    straight_movement_cost = 10

    x_distance = abs(end_x - start_x)
    y_distance = abs(end_y - start_y)
    remaining = abs(x_distance - y_distance)

    (diagonal_movement_cost * Enum.min([x_distance, y_distance]) + remaining * straight_movement_cost)
    |> div(10)
  end

  defp map_entities(entities, bot, type) do
    entities
    |> Enum.map(fn entity ->
      %{
        type: type,
        entity_id: entity.id,
        distance_to_entity: get_distance_to_point(bot.position, entity.position),
        direction_to_entity: calculate_circle_point(bot.position, entity.position)
      }
    end)
    |> Enum.sort_by(fn distances -> distances.distance_to_entity end, :asc)
    |> Enum.filter(fn distances -> distances.distance_to_entity <= @visibility_max_range_cells end)
  end

  def maybe_put_wandering_position(
        %{objective: :wander, current_wandering_position: current_wandering_position} = bot_state,
        bot,
        myrra_state
      ) do
    if get_distance_to_point(bot.position, %{x: current_wandering_position.x, y: current_wandering_position.y}) <
         500 do
      put_wandering_position(bot_state, bot, myrra_state)
    else
      bot_state
    end
  end

  def maybe_put_wandering_position(bot_state, bot, myrra_state),
    do: put_wandering_position(bot_state, bot, myrra_state)

  def put_wandering_position(
        bot_state,
        %{position: bot_position},
        myrra_state
      ) do
    bot_visibility_radius = @visibility_max_range_cells * 2

    # We need to pick and X and Y wich are in a safe zone close to the bot that's also inside of the board
    left_x =
      Enum.max([myrra_state.shrinking_center.x - myrra_state.playable_radius, bot_position.x - bot_visibility_radius, 0])

    right_x =
      Enum.min([
        myrra_state.shrinking_center.x + myrra_state.playable_radius,
        bot_position.x + bot_visibility_radius,
        myrra_state.board.width
      ])

    down_y =
      Enum.max([myrra_state.shrinking_center.y - myrra_state.playable_radius, bot_position.y - bot_visibility_radius, 0])

    up_y =
      Enum.min([
        myrra_state.shrinking_center.y + myrra_state.playable_radius,
        bot_position.y + bot_visibility_radius,
        myrra_state.board.height
      ])

    wandering_position = %{
      x: Enum.random(left_x..right_x),
      y: Enum.random(down_y..up_y)
    }

    Map.merge(bot_state, %{current_wandering_position: wandering_position, objective: :wander})
  end
end
