defmodule DarkWorldsServer.Engine.Runner do
  use GenServer, restart: :transient
  require Logger
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.ActionOk
  alias DarkWorldsServer.Engine.Game
  require Logger
  @build_walls false
  # The game will be closed twenty minute after it starts
  @game_timeout 20 * 60 * 1000
  # The session will be closed one minute after the game has finished
  @session_timeout 60 * 1000
  # This is the amount of time between state updates in milliseconds
  @tick_rate_ms 20

  case Mix.env() do
    :test ->
      # Check player count every 3 seconds in testing
      @player_check 3 * 1000

    _ ->
      # Check player count every minute.
      @player_check 1 * 60 * 1000
  end

  #######
  # API #
  #######
  def start_link(args) do
    GenServer.start_link(__MODULE__, args)
  end

  def join(runner_pid, player_id) do
    GenServer.call(runner_pid, {:join, player_id})
  end

  def play(runner_pid, player_id, %ActionOk{} = action) do
    GenServer.cast(runner_pid, {:play, player_id, action})
  end

  def disconnect(runner_pid, player_id) do
    GenServer.cast(runner_pid, {:disconnect, player_id})
  end

  def get_game_state(runner_pid) do
    GenServer.call(runner_pid, :get_state)
  end

  def get_board(runner_pid) do
    GenServer.call(runner_pid, :get_board)
  end

  def get_logged_players(runner_pid) do
    GenServer.call(runner_pid, :get_logged_players)
  end

  #######################
  # GenServer callbacks #
  #######################
  @doc """
  Starts a new game, triggers the first
  update and the final game timeout.
  """
  def init(opts) do
    priority =
      Application.fetch_env!(:dark_worlds_server, __MODULE__)
      |> Keyword.fetch!(:process_priority)

    Process.flag(:priority, priority)

    Logger.info("#{DateTime.utc_now()} Starting runner, pid: #{inspect(self())}")
    Logger.info("#{DateTime.utc_now()} Received config: #{inspect(opts.game_config, pretty: true)}")

    {:ok, game} = create_new_game(opts.game_config, length(opts.players))

    tick_rate = Map.get(opts.game_config.runner_config, :server_tickrate_ms, @tick_rate_ms)

    # Finish game after @game_timeout seconds or the specified in the game_settings file

    Process.send_after(self(), :game_timeout, Map.get(opts.game_config.runner_config, :game_timeout, @game_timeout))
    Process.send_after(self(), :check_player_amount, @player_check)

    Process.send_after(self(), :update_state, tick_rate)

    {:ok,
     %{
       client_game_state: %{game: game},
       server_game_state: %{game: game},
       max_players: length(opts.players),
       players: opts.players,
       current_players: 0,
       current_round: 1,
       game_state: :playing,
       winners: [],
       is_single_player?: length(opts.players) == 1,
       tick_rate: tick_rate
     }}
  end

  def handle_cast(_actions, %{game_state: :game_finished} = gen_server_state) do
    {:noreply, gen_server_state}
  end

  def handle_cast(_actions, %{game_state: :round_finished} = gen_server_state) do
    {:noreply, gen_server_state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :move_with_joystick, value: %{x: x, y: y}}},
        %{server_game_state: %{game: game} = server_game_state} = gen_server_state
      ) do
    {:ok, game} = Game.move_with_joystick(game, player, x, y)

    server_game_state = Map.put(server_game_state, :game, game)

    gen_server_state = Map.put(gen_server_state, :server_game_state, server_game_state)

    {:noreply, gen_server_state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :move, value: value}},
        %{server_game_state: %{game: game} = server_game_state} = gen_server_state
      ) do
    game =
      game
      |> Game.move_player(player, value)

    server_game_state = Map.put(server_game_state, :game, game)

    gen_server_state = Map.put(gen_server_state, :server_game_state, server_game_state)

    {:noreply, gen_server_state}
  end

  def handle_cast(
        {:play, player_id, %ActionOk{action: :teleport, value: position_transform}},
        %{next_state: %{game: game} = next_state} = state
      ) do
    game =
      game
      |> Game.move_player_to_coordinates(player_id, position_transform)

    next_state = Map.put(next_state, :game, game)

    state = Map.put(state, :next_state, next_state)

    {:noreply, state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :attack, value: value}},
        %{server_game_state: %{game: game} = server_game_state} = gen_server_state
      ) do
    game =
      game
      |> Game.attack_player(player, value)

    server_game_state = server_game_state |> Map.put(:game, game)
    gen_server_state = Map.put(gen_server_state, :server_game_state, server_game_state)
    {:noreply, gen_server_state}
  end

  def handle_cast(
        {:play, player_id, %ActionOk{action: :skill_1, value: value}},
        %{server_game_state: %{game: game} = server_game_state} = gen_server_state
      ) do
    {:ok, game} = Game.skill_1(game, player_id, value)

    server_game_state = server_game_state |> Map.put(:game, game)
    gen_server_state = Map.put(gen_server_state, :server_game_state, server_game_state)

    {:noreply, gen_server_state}
  end

  def handle_cast(
        {:play, player_id, %ActionOk{action: :basic_attack, value: value}},
        %{server_game_state: %{game: game} = server_game_state} = gen_server_state
      ) do
    {:ok, game} = Game.basic_attack(game, player_id, value)

    server_game_state = server_game_state |> Map.put(:game, game)
    gen_server_state = Map.put(gen_server_state, :server_game_state, server_game_state)

    {:noreply, gen_server_state}
  end

  def handle_cast({:play, _, %ActionOk{action: :add_bot}}, gen_server_state) do
    %{server_game_state: %{game: game} = game_state, current_players: current} = gen_server_state
    player_id = current + 1
    new_game = Game.spawn_player(game, player_id)

    Phoenix.PubSub.broadcast(
      DarkWorldsServer.PubSub,
      Communication.pubsub_game_topic(self()),
      {:player_joined, player_id}
    )

    {:noreply, %{gen_server_state | server_game_state: %{game_state | game: new_game}, current_players: current + 1}}
  end

  def handle_cast(
        {:disconnect, player_id},
        %{client_game_state: %{game: game} = game_state, current_players: current} = gen_server_state
      ) do
    current = current - 1
    {:ok, game} = Game.disconnect(game, player_id)
    {:noreply, %{gen_server_state | client_game_state: %{game_state | game: game}, current_players: current}}
  end

  def handle_call(
        {:join, player_id},
        _,
        %{max_players: max, current_players: current} = gen_server_state
      )
      when current < max do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(
      Communication.pubsub_game_topic(self()),
      {:player_joined, player_id}
    )

    {:reply, {:ok, player_id}, %{gen_server_state | current_players: current + 1}}
  end

  def handle_call(:join, _, %{max_players: max, current_players: max} = gen_server_state) do
    {:reply, {:error, :game_full}, gen_server_state}
  end

  def handle_call(:get_board, _from, %{client_game_state: %{game: %Game{board: board}}} = gen_server_state) do
    {:reply, board, gen_server_state}
  end

  def handle_call(:get_players, _from, %{client_game_state: %{game: %Game{players: players}}} = gen_server_state) do
    {:reply, players, gen_server_state}
  end

  def handle_call(:get_logged_players, _from, %{players: players} = gen_server_state) do
    {:reply, players, gen_server_state}
  end

  def handle_call(:get_state, _from, %{client_game_state: game_state} = gen_server_state) do
    {:reply, game_state, gen_server_state}
  end

  def handle_info(
        :check_player_amount,
        gen_server_state = %{current_players: current}
      )
      when current > 0 do
    Process.send_after(self(), :check_player_amount, @player_check)
    {:noreply, gen_server_state}
  end

  def handle_info(:check_player_amount, gen_server_state = %{current_players: current})
      when current == 0 do
    Process.send_after(self(), :session_timeout, 500)
    {:noreply, Map.put(gen_server_state, :game_state, :game_finished)}
  end

  def handle_info(:game_timeout, gen_server_state) do
    Process.send_after(self(), :session_timeout, @session_timeout)

    {:noreply, Map.put(gen_server_state, :game_state, :game_finished)}
  end

  def handle_info(:session_timeout, gen_server_state) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(
      Communication.pubsub_game_topic(self()),
      {:game_finished, gen_server_state}
    )

    {:stop, :normal, gen_server_state}
  end

  def handle_info(:update_state, %{server_game_state: server_game_state} = gen_server_state) do
    gen_server_state = Map.put(gen_server_state, :client_game_state, server_game_state)

    game =
      server_game_state.game
      |> Game.world_tick()

    game_state = has_a_player_won?(game.players, gen_server_state.is_single_player?)

    server_game_state = server_game_state |> Map.put(:game, game)

    gen_server_state =
      Map.put(gen_server_state, :server_game_state, server_game_state) |> Map.put(:game_state, game_state)

    decide_next_game_update(gen_server_state)
    |> broadcast_game_update()
  end

  def handle_info(:next_round, %{server_game_state: server_game_state} = gen_server_state) do
    gen_server_state = Map.put(gen_server_state, :client_game_state, server_game_state)

    decide_next_game_update(gen_server_state)
    |> broadcast_game_update()
  end

  ####################
  # Internal helpers #
  ####################
  defp has_a_player_won?(_players, true = _is_single_player?), do: :playing

  defp has_a_player_won?(players, _is_single_player?) do
    players_alive =
      Enum.filter(players, fn player ->
        player.status == :alive
      end)

    if Enum.count(players_alive) == 1 do
      :round_finished
    else
      :playing
    end
  end

  defp decide_next_game_update(
         %{game_state: :round_finished, winners: winners, current_round: current_round} = gen_server_state
       ) do
    # This has to be done in order to apply the last attack
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:game_update, gen_server_state})

    [winner] = Enum.filter(gen_server_state.server_game_state.game.players, fn player -> player.status == :alive end)

    winners = [winner | winners]
    amount_of_winners = winners |> Enum.uniq_by(fn winner -> winner.id end) |> Enum.count()

    gen_server_state = Map.put(gen_server_state, :winners, winners)

    next_game_update =
      cond do
        current_round == 2 and amount_of_winners == 2 ->
          :last_round

        (current_round == 2 && amount_of_winners == 1) || current_round == 3 ->
          :game_finished

        true ->
          :next_round
      end

    {next_game_update, gen_server_state, winner}
  end

  defp decide_next_game_update(%{game_state: :playing} = gen_server_state) do
    {:game_update, gen_server_state}
  end

  defp broadcast_game_update(
         {:last_round,
          %{winners: winners, current_round: current_round, server_game_state: server_game_state} = gen_server_state,
          winner}
       ) do
    game = Game.new_round(server_game_state.game, winners)

    server_game_state = Map.put(server_game_state, :game, game)

    gen_server_state =
      gen_server_state
      |> Map.put(:server_game_state, server_game_state)
      |> Map.put(:current_round, current_round + 1)
      |> Map.put(:game_state, :playing)

    Process.send_after(self(), :update_state, gen_server_state.tick_rate)

    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:last_round, winner, gen_server_state})

    Process.send_after(self(), :update_state, gen_server_state.tick_rate)

    {:noreply, gen_server_state}
  end

  defp broadcast_game_update(
         {:next_round, %{current_round: current_round, server_game_state: server_game_state} = gen_server_state, winner}
       ) do
    game = Game.new_round(server_game_state.game, server_game_state.game.players)

    server_game_state = Map.put(server_game_state, :game, game)

    gen_server_state =
      gen_server_state
      |> Map.put(:server_game_state, server_game_state)
      |> Map.put(:current_round, current_round + 1)
      |> Map.put(:game_state, :playing)

    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:next_round, winner, gen_server_state})

    Process.send_after(self(), :update_state, gen_server_state.tick_rate)

    {:noreply, gen_server_state}
  end

  defp broadcast_game_update({:game_update, gen_server_state}) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:game_update, gen_server_state})

    Process.send_after(self(), :update_state, gen_server_state.tick_rate)

    {:noreply, gen_server_state}
  end

  defp broadcast_game_update({:game_finished, gen_server_state, winner}) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:game_finished, winner, gen_server_state})

    Process.send_after(self(), :session_timeout, @session_timeout)

    {:noreply, gen_server_state}
  end

  defp create_new_game(%{runner_config: rg, character_config: %{Items: character_info}}, players) do
    character_info =
      for character <- character_info do
        Enum.reduce(character, %{}, fn
          {:__unknown_fields__, _}, map -> map
          {key, val}, map -> Map.put(map, key |> Atom.to_string(), val)
        end)
      end

    config = %{
      number_of_players: players,
      board: {rg.board_width, rg.board_height},
      build_walls: @build_walls,
      characters: character_info
    }

    Game.new(config)
  end
end
