defmodule DarkWorldsServer.Engine.EngineRunner do
  use GenServer, restart: :transient
  require Logger
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.ActionOk

  # This is the amount of time between state updates in milliseconds
  @game_tick_rate_ms 20
  # Amount of time between loot spawn
  @loot_spawn_rate_ms 5_000

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
    # priority =
    #   Application.fetch_env!(:dark_worlds_server, __MODULE__)
    #   |> Keyword.fetch!(:process_priority)

    # Process.flag(:priority, priority)

    engine_config = LambdaGameEngine.parse_config(engine_config_raw_json)

    state = %{
      game_state: LambdaGameEngine.engine_new_game(engine_config),
      player_timestamps: %{},
      broadcast_topic: Communication.pubsub_game_topic(self()),
      user_to_player: %{}
    }

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
      playable_radius: 20000,
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
        id: player.id,
        position: transform_position_to_myrra_position(player.position),
        status: (if player.health <= 0, do: :dead, else: :alive),
        character: %{
          active: true,
          id: 2,
          name: :h4ck,
          __struct__: LambdaGameEngine.MyrraEngine.Character,
          class: :hunter,
          skill_1: %{
            name: "Multishot",
            __struct__: LambdaGameEngine.MyrraEngine.Skill,
            angle: 45,
            duration: 0,
            damage: 9,
            cooldown_ms: 4500,
            skill_range: 0.0,
            par1: 100,
            par1desc: "Projectiles speed",
            par2: 0,
            par2desc: "",
            par3: 0,
            par3desc: "",
            par4: 0,
            par4desc: "",
            par5: 0,
            par5desc: ""
          },
          skill_2: %{
            name: "Disarm",
            __struct__: LambdaGameEngine.MyrraEngine.Skill,
            angle: 0,
            duration: 4000,
            damage: 0,
            cooldown_ms: 6500,
            skill_range: 0.0,
            par1: 0,
            par1desc: "",
            par2: 0,
            par2desc: "",
            par3: 0,
            par3desc: "",
            par4: 0,
            par4desc: "",
            par5: 0,
            par5desc: ""
          },
          skill_3: %{
            name: "Neon Crash",
            __struct__: LambdaGameEngine.MyrraEngine.Skill,
            angle: 0,
            duration: 300,
            damage: 3,
            cooldown_ms: 5000,
            skill_range: 200.0,
            par1: 0,
            par1desc: "",
            par2: 0,
            par2desc: "",
            par3: 0,
            par3desc: "",
            par4: 0,
            par4desc: "",
            par5: 0,
            par5desc: ""
          },
          skill_4: %{
            name: "Denial of Service",
            __struct__: LambdaGameEngine.MyrraEngine.Skill,
            angle: 0,
            duration: 6000,
            damage: 0,
            cooldown_ms: 20000,
            skill_range: 0.0,
            par1: 0,
            par1desc: "",
            par2: 0,
            par2desc: "",
            par3: 0,
            par3desc: "",
            par4: 0,
            par4desc: "",
            par5: 0,
            par5desc: ""
          },
          body_size: 80.0,
          base_speed: 25,
          faction: :otobi,
          skill_basic: %{
            name: "Slingshot",
            __struct__: LambdaGameEngine.MyrraEngine.Skill,
            angle: 120,
            duration: 0,
            damage: 9,
            cooldown_ms: 800,
            skill_range: 350.0,
            par1: 100,
            par1desc: "Projectile speed",
            par2: 0,
            par2desc: "",
            par3: 0,
            par3desc: "",
            par4: 0,
            par4desc: "",
            par5: 0,
            par5desc: ""
          }
        },
        __struct__: LambdaGameEngine.MyrraEngine.Player,
        action: :nothing,
        direction: %LambdaGameEngine.MyrraEngine.RelativePosition{
          x: 0.0,
          y: 0.0
        },
        character_name: "H4ck",
        health: 100,
        skill_4_cooldown_left: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        skill_3_cooldown_left: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        skill_2_cooldown_left: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        skill_1_cooldown_left: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        kill_count: 0,
        effects: %{},
        death_count: 0,
        body_size: 80.0,
        basic_skill_cooldown_left: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        aoe_position: %LambdaGameEngine.MyrraEngine.Position{x: 0, y: 0},
        basic_skill_started_at: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        skill_1_started_at: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        skill_2_started_at: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        skill_3_started_at: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        skill_4_started_at: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        basic_skill_ends_at: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        skill_1_ends_at: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        skill_2_ends_at: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        skill_3_ends_at: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        },
        skill_4_ends_at: %{
          high: 0,
          low: 0,
          __struct__: LambdaGameEngine.MyrraEngine.Player
        }
      }
    end)
  end

  defp transform_loots_to_myrra_loots(loots) do
    Enum.map(loots, fn loot ->
      %{
        id: loot.id,
        loot_type: {:health, :placeholder}, # The only type of loot is health so we can leverage that
        position: transform_position_to_myrra_position(loot.position)
      }
    end)
  end

  defp transform_position_to_myrra_position(position) do
    %LambdaGameEngine.MyrraEngine.Position{x: -1*position.y + 5000, y: position.x + 5000}
  end
end
