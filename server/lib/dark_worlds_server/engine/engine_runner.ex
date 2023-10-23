defmodule DarkWorldsServer.Engine.EngineRunner do
  use GenServer, restart: :transient
  require Logger
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.ActionOk

  # This is the amount of time between state updates in milliseconds
  @game_tick_rate_ms 20
  # Amount of time between loot spawn
  @loot_spawn_rate_ms 20_000

  #######
  # API #
  #######
  def start_link(args) do
    GenServer.start_link(__MODULE__, args)
  end

  def join(runner_pid, user_id, character_name) do
    GenServer.call(runner_pid, {:join, user_id, character_name})
  end

  def play(runner_pid, user_id, action) do
    GenServer.cast(runner_pid, {:play, user_id, action})
  end

  def start_game_tick(runner_pid) do
    GenServer.cast(runner_pid, :start_game_tick)
  end

  if Mix.env() == :dev do
    def enable() do
      config =
        Application.get_env(:dark_worlds_server, DarkWorldsServer.Engine.Runner)
        |> Keyword.put(:use_engine_runner, true)

      Application.put_env(:dark_worlds_server, DarkWorldsServer.Engine.Runner, config)
    end
  end

  #######################
  # GenServer callbacks #
  #######################
  @impl true
  def init(%{engine_config_raw_json: engine_config_raw_json}) do
    priority =
      Application.fetch_env!(:dark_worlds_server, DarkWorldsServer.Engine.Runner)
      |> Keyword.fetch!(:process_priority)

    Process.flag(:priority, priority)

    engine_config = LambdaGameEngine.parse_config(engine_config_raw_json)

    state = %{
      game_state: LambdaGameEngine.engine_new_game(engine_config),
      player_timestamps: %{},
      broadcast_topic: Communication.pubsub_game_topic(self()),
      user_to_player: %{}
    }

    Process.put(:map_size, {engine_config.game.width, engine_config.game.height})

    {:ok, state}
  end

  @impl true
  def handle_call({:join, user_id, character_name}, _from, state) do
    {game_state, player_id} = LambdaGameEngine.add_player(state.game_state, character_name)

    state =
      Map.put(state, :game_state, game_state)
      |> put_in([:user_to_player, user_id], player_id)

    {:reply, :ok, state}
  end

  def handle_call(msg, from, state) do
    Logger.error("Unexpected handle_call msg", %{msg: msg, from: from})
    {:noreply, state}
  end

  @impl true
  def handle_cast({:play, user_id, %ActionOk{action: :move_with_joystick, value: value, timestamp: timestamp}}, state) do
    angle =
      case Nx.atan2(value.y, value.x) |> Nx.multiply(Nx.divide(180.0, Nx.Constants.pi())) |> Nx.to_number() do
        pos_degree when pos_degree >= 0 -> pos_degree
        neg_degree -> neg_degree + 360
      end

    player_id = state.user_to_player[user_id]
    game_state = LambdaGameEngine.move_player(state.game_state, player_id, angle)

    state =
      Map.put(state, :game_state, game_state)
      |> put_in([:player_timestamps, player_id], timestamp)

    {:noreply, state}
  end

  def handle_cast(:start_game_tick, state) do
    Process.send_after(self(), :game_tick, @game_tick_rate_ms)
    Process.send_after(self(), :spawn_loot, @loot_spawn_rate_ms)

    {:noreply, state}
  end

  def handle_cast(msg, state) do
    Logger.error("Unexpected handle_cast msg", %{msg: msg})
    {:noreply, state}
  end

  @impl true
  def handle_info(:game_tick, state) do
    Process.send_after(self(), :game_tick, @game_tick_rate_ms)

    ## TODO: implement game tick
    # LambdaGameEngine.game_tick(state.game_state)
    game_state = state.game_state

    broadcast_game_state(state.broadcast_topic, Map.put(game_state, :player_timestamps, state.player_timestamps))

    {:noreply, %{state | game_state: game_state}}
  end

  def handle_info(:spawn_loot, state) do
    Process.send_after(self(), :spawn_loot, @loot_spawn_rate_ms)

    {game_state, _loot_id} = LambdaGameEngine.spawn_random_loot(state.game_state)

    {:noreply, %{state | game_state: game_state}}
  end

  def handle_info(msg, state) do
    Logger.error("Unexpected handle_info msg", %{msg: msg})
    {:noreply, state}
  end

  ####################
  # Internal helpers #
  ####################
  defp broadcast_game_state(topic, game_state) do
    Phoenix.PubSub.broadcast(DarkWorldsServer.PubSub, topic, {:game_state, transform_state_to_myrra_state(game_state)})
  end

  defp transform_state_to_myrra_state(game_state) do
    %{
      __struct__: LambdaGameEngine.MyrraEngine.Game,
      players: transform_players_to_myrra_players(game_state.players),
      board: %{
        width: game_state.config.game.width,
        __struct__: LambdaGameEngine.MyrraEngine.Board,
        height: game_state.config.game.height
      },
      projectiles: [],
      killfeed: [],
      playable_radius: 20_000,
      shrinking_center: %LambdaGameEngine.MyrraEngine.Position{x: 5000, y: 5000},
      loots: transform_loots_to_myrra_loots(game_state.loots),
      next_killfeed: [],
      next_projectile_id: 0,
      next_loot_id: 0,
      player_timestamps: game_state.player_timestamps
    }
  end

  defp transform_players_to_myrra_players(players) do
    Enum.map(players, fn {_id, player} ->
      %{
        ## Transformed
        __struct__: LambdaGameEngine.MyrraEngine.Player,
        id: player.id,
        position: transform_position_to_myrra_position(player.position),
        status: if(player.health <= 0, do: :dead, else: :alive),
        health: player.health,
        body_size: player.size,
        character_name: transform_character_name_to_myrra_character_name(player.character.name),
        ## Placeholder values
        kill_count: 0,
        effects: %{},
        death_count: 0,
        action: :nothing,
        direction: %LambdaGameEngine.MyrraEngine.RelativePosition{
          x: 0.0,
          y: 0.0
        },
        aoe_position: %LambdaGameEngine.MyrraEngine.Position{x: 0, y: 0}
      }
      |> transform_player_cooldowns_to_myrra_player_cooldowns(player)
    end)
  end

  defp transform_player_cooldowns_to_myrra_player_cooldowns(myrra_player, engine_player) do
    myrra_cooldowns = %{
      basic_skill_cooldown_left: transform_milliseconds_to_myrra_millis_time(engine_player.cooldowns["1"]),
      skill_1_cooldown_left: transform_milliseconds_to_myrra_millis_time(engine_player.cooldowns["2"]),
      skill_2_cooldown_left: transform_milliseconds_to_myrra_millis_time(engine_player.cooldowns["3"]),
      skill_3_cooldown_left: transform_milliseconds_to_myrra_millis_time(engine_player.cooldowns["4"]),
      skill_4_cooldown_left: transform_milliseconds_to_myrra_millis_time(engine_player.cooldowns["5"])
    }

    Map.merge(myrra_player, myrra_cooldowns)
  end

  defp transform_milliseconds_to_myrra_millis_time(nil), do: %{high: 0, low: 0}
  defp transform_milliseconds_to_myrra_millis_time(cooldown), do: %{high: 0, low: cooldown}

  defp transform_loots_to_myrra_loots(loots) do
    Enum.map(loots, fn loot ->
      %{
        id: loot.id,
        loot_type: {:health, :placeholder},
        position: transform_position_to_myrra_position(loot.position)
      }
    end)
  end

  defp transform_position_to_myrra_position(position) do
    {width, height} = Process.get(:map_size)
    %LambdaGameEngine.MyrraEngine.Position{x: -1 * position.y + div(width, 2), y: position.x + div(height, 2)}
  end

  defp transform_character_name_to_myrra_character_name("h4ck"), do: "H4ck"
end
