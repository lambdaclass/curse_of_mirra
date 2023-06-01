defmodule DarkWorldsServer.Engine.Runner do
  use GenServer, restart: :transient

  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.ActionOk
  alias DarkWorldsServer.Engine.Game
  alias DarkWorldsServer.Engine.Player

  @build_walls false
  @board {1000, 1000}
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
  Starts a new game state, triggers the first
  update and the final game timeout.
  """
  def init(opts) do
    priority =
      Application.fetch_env!(:dark_worlds_server, __MODULE__)
      |> Keyword.fetch!(:process_priority)

    Process.flag(:priority, priority)

    state = create_new_game(opts)

    tick_rate = Map.get(opts.game_config, :server_tickrate_ms, @tick_rate_ms)

    # Finish game after @game_timeout seconds or the specified in the game_settings file
    Process.send_after(self(), :game_timeout, Map.get(opts.game_config, :game_timeout, @game_timeout))
    Process.send_after(self(), :check_player_amount, @player_check)

    initial_state = %{
      game: state
    }

    Process.send_after(self(), :update_state, tick_rate)

    {:ok,
     %{
       current_state: initial_state,
       next_state: initial_state,
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

  def handle_cast(_actions, %{game_state: :game_finished} = state) do
    {:noreply, state}
  end

  def handle_cast(_actions, %{game_state: :round_finished} = state) do
    {:noreply, state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :move_with_joystick, value: %{x: x, y: y}}},
        %{next_state: %{game: game} = next_state} = state
      ) do
    {:ok, game} = Game.move_with_joystick(game, player, x, y)

    next_state = Map.put(next_state, :game, game)

    state = Map.put(state, :next_state, next_state)

    {:noreply, state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :move, value: value}},
        %{next_state: %{game: game} = next_state} = state
      ) do
    game =
      game
      |> Game.move_player(player, value)

    next_state = Map.put(next_state, :game, game)

    state = Map.put(state, :next_state, next_state)

    {:noreply, state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :attack, value: value}},
        %{next_state: %{game: game} = next_state} = state
      ) do
    game =
      game
      |> Game.attack_player(player, value)

    game_state = has_a_player_won?(game.players, state.is_single_player?)

    next_state = next_state |> Map.put(:game, game)
    state = Map.put(state, :next_state, next_state) |> Map.put(:game_state, game_state)

    {:noreply, state}
  end

  def handle_cast(
        {:play, player_id, %ActionOk{action: :attack_aoe, value: value}},
        %{next_state: %{game: game} = next_state} = state
      ) do
    %Player{position: _position} = get_player(game.players, player_id)
    game = Game.attack_aoe(game, player_id, value)

    game_state = has_a_player_won?(game.players, state.is_single_player?)

    next_state = next_state |> Map.put(:game, game)
    state = Map.put(state, :next_state, next_state) |> Map.put(:game_state, game_state)

    {:noreply, state}
  end

  def handle_cast({:play, _, %ActionOk{action: :add_bot}}, state) do
    %{next_state: %{game: game} = game_state, current_players: current} = state
    player_id = current + 1
    new_game = Game.spawn_player(game, player_id)

    Phoenix.PubSub.broadcast(
      DarkWorldsServer.PubSub,
      Communication.pubsub_game_topic(self()),
      {:player_joined, player_id}
    )

    {:noreply, %{state | next_state: %{game_state | game: new_game}, current_players: current + 1}}
  end

  def handle_cast(
        {:disconnect, player_id},
        %{current_state: %{game: game} = game_state, current_players: current} = state
      ) do
    current = current - 1
    {:ok, game} = Game.disconnect(game, player_id)
    {:noreply, %{state | current_state: %{game_state | game: game}, current_players: current}}
  end

  def handle_call(
        {:join, player_id},
        _,
        %{max_players: max, current_players: current} = state
      )
      when current < max do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(
      Communication.pubsub_game_topic(self()),
      {:player_joined, player_id}
    )

    {:reply, {:ok, player_id}, %{state | current_players: current + 1}}
  end

  def handle_call(:join, _, %{max_players: max, current_players: max} = state) do
    {:reply, {:error, :game_full}, state}
  end

  def handle_call(:get_board, _from, %{current_state: %{game: %Game{board: board}}} = state) do
    {:reply, board, state}
  end

  def handle_call(:get_players, _from, %{current_state: %{game: %Game{players: players}}} = state) do
    {:reply, players, state}
  end

  def handle_call(:get_logged_players, _from, %{players: players} = state) do
    {:reply, players, state}
  end

  def handle_call(:get_state, _from, %{current_state: game_state} = state) do
    {:reply, game_state, state}
  end

  def handle_info(
        :check_player_amount,
        state = %{current_players: current}
      )
      when current > 0 do
    Process.send_after(self(), :check_player_amount, @player_check)
    {:noreply, state}
  end

  def handle_info(:check_player_amount, state = %{current_players: current})
      when current == 0 do
    Process.send_after(self(), :session_timeout, 500)
    {:noreply, Map.put(state, :has_finished?, true)}
  end

  def handle_info(:game_timeout, state) do
    Process.send_after(self(), :session_timeout, @session_timeout)

    {:noreply, Map.put(state, :has_finished?, true)}
  end

  def handle_info(:session_timeout, state) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(
      Communication.pubsub_game_topic(self()),
      {:game_finished, state}
    )

    {:stop, :normal, state}
  end

  def handle_info(:update_state, %{next_state: next_state} = state) do
    state = Map.put(state, :current_state, next_state)

    game =
      next_state.game
      |> Game.world_tick()

    next_state = next_state |> Map.put(:game, game)
    state = Map.put(state, :next_state, next_state)

    decide_next_game_update(state)
    |> broadcast_game_update()
  end

  def handle_info(:next_round, %{next_state: next_state} = state) do
    state = Map.put(state, :current_state, next_state)

    decide_next_game_update(state)
    |> broadcast_game_update()
  end

  ####################
  # Internal helpers #
  ####################
  defp has_a_player_won?(_players, true = _is_single_player?), do: :playing

  defp has_a_player_won?(players, _is_single_player?) do
    players_alive = Enum.filter(players, fn player -> player.status == :alive end)

    if Enum.count(players_alive) == 1 do
      :round_finished
    else
      :playing
    end
  end

  defp decide_next_game_update(%{game_state: :round_finished, winners: winners, current_round: current_round} = state) do
    # This has to be done in order to apply the last attack
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:game_update, state})

    [winner] = Enum.filter(state.next_state.game.players, fn player -> player.status == :alive end)
    winners = [winner | winners]
    amount_of_winners = winners |> Enum.uniq_by(fn winner -> winner.id end) |> Enum.count()

    state = Map.put(state, :winners, winners)

    next_game_update =
      cond do
        current_round == 2 and amount_of_winners == 2 ->
          :last_round

        (current_round == 2 && amount_of_winners == 1) || current_round == 3 ->
          :game_finished

        true ->
          :next_round
      end

    {next_game_update, state}
  end

  defp decide_next_game_update(%{game_state: :playing} = state) do
    {:game_update, state}
  end

  defp broadcast_game_update(
         {:last_round, %{winners: winners, current_round: current_round, next_state: next_state} = state}
       ) do
    game = Game.new_round(next_state.game, winners)

    next_state = Map.put(next_state, :game, game)

    state =
      state
      |> Map.put(:next_state, next_state)
      |> Map.put(:current_round, current_round + 1)
      |> Map.put(:game_state, :playing)

    Process.send_after(self(), :update_state, state.tick_rate)

    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:last_round, state})

    Process.send_after(self(), :update_state, state.tick_rate)

    {:noreply, state}
  end

  defp broadcast_game_update({:next_round, %{current_round: current_round, next_state: next_state} = state}) do
    game = Game.new_round(next_state.game, next_state.game.players)

    next_state = Map.put(next_state, :game, game)

    state =
      state
      |> Map.put(:next_state, next_state)
      |> Map.put(:current_round, current_round + 1)
      |> Map.put(:game_state, :playing)

    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:next_round, state})

    Process.send_after(self(), :update_state, state.tick_rate)

    {:noreply, state}
  end

  defp broadcast_game_update({:game_update, state}) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:game_update, state})

    Process.send_after(self(), :update_state, state.tick_rate)

    {:noreply, state}
  end

  defp broadcast_game_update({:game_finished, state}) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:game_finished, state})

    Process.send_after(self(), :session_timeout, @session_timeout)

    {:noreply, state}
  end

  defp get_player(players, player_id) do
    Enum.find(players, fn p -> p.id == player_id end)
  end

  defp create_new_game(%{game_config: %{board_size: board}, players: players}) do
    board = {board.width, board.height}

    config = %{
      number_of_players: length(players),
      board: board,
      build_walls: @build_walls
    }

    Game.new(config)
  end

  defp create_new_game(%{players: players}) do
    config = %{
      number_of_players: length(players),
      board: @board,
      build_walls: @build_walls
    }

    Game.new(config)
  end
end
