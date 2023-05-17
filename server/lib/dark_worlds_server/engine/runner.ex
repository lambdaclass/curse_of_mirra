defmodule DarkWorldsServer.Engine.Runner do
  use GenServer, restart: :transient

  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.{ActionOk, Game, Player}

  @build_walls false
  @amount_of_players 3
  @board {1000, 1000}
  # The game will be closed twenty minute after it starts
  @game_timeout 20 * 60 * 1000
  # The session will be closed one minute after the game has finished
  @session_timeout 60 * 1000
  # This is the amount of time between updates (30ms)
  @update_time 30
  # This is the number of tiles characters move per :move command
  @character_speed 3
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
    state =
      Game.new(number_of_players: @amount_of_players, board: @board, build_walls: @build_walls)

    # Finish game after @game_timeout seconds
    Process.send_after(self(), :game_timeout, @game_timeout)
    Process.send_after(self(), :check_player_amount, @player_check)

    initial_state = %{
      game: state,
      has_finished?: false
    }

    Process.send_after(self(), :update_state, @update_time)

    {:ok,
     %{
       current_state: initial_state,
       next_state: initial_state,
       max_players: @amount_of_players,
       players: opts.players,
       current_players: 0
     }}
  end

  def handle_cast(_actions, %{current_state: %{has_finished?: true}} = state) do
    {:noreply, state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :move, value: value}},
        %{next_state: %{game: game} = next_state} = state
      ) do
    game =
      game
      |> Game.move_player(player, value, @character_speed)

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

    has_a_player_won? = has_a_player_won?(game.players)
    next_state = next_state |> Map.put(:game, game) |> Map.put(:has_finished?, has_a_player_won?)
    state = Map.put(state, :next_state, next_state)

    {:noreply, state}
  end

  def handle_cast(
        {:play, player_id, %ActionOk{action: :attack_aoe}},
        %{next_state: %{game: game} = next_state} = state
      ) do
    %Player{position: position} = get_player(game.players, player_id)
    game = Game.attack_aoe(game, player_id, position)

    has_a_player_won? = has_a_player_won?(game.players)

    next_state = next_state |> Map.put(:game, game) |> Map.put(:has_finished?, has_a_player_won?)
    state = Map.put(state, :next_state, next_state)

    {:noreply, state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :update_ping, value: value}},
        state
      ) do
    broadcast_players_ping(player, value)

    {:noreply, state}
  end

  def handle_cast(
        {:disconnect, player_id},
        state = %{current_state: game_state = %{game: game}, current_players: current}
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
      {:player_joined, player_id, state}
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

  def handle_call(:get_character_speed, _from, state) do
    {:reply, @character_speed, state}
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

  def handle_info(:update_state, %{current_state: %{has_finished?: true}} = state) do
    {:noreply, state}
  end

  def handle_info(:update_state, %{next_state: next_state} = state) do
    state = Map.put(state, :current_state, next_state)

    has_a_player_won? = has_a_player_won?(next_state.game.players)

    maybe_broadcast_game_finished_message(has_a_player_won?, state)

    {:noreply, state}
  end

  ####################
  # Internal helpers #
  ####################
  defp has_a_player_won?(players) do
    players_alive = Enum.filter(players, fn player -> player.health != 0 end)
    Enum.count(players_alive) == 1
  end

  defp maybe_broadcast_game_finished_message(true, state) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:game_finished, state})

    Process.send_after(self(), :session_timeout, @session_timeout)
  end

  # Broadcast the current game state to
  # each connected player.
  defp maybe_broadcast_game_finished_message(_false, state) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(Communication.pubsub_game_topic(self()), {:game_update, state})

    Process.send_after(self(), :update_state, @update_time)
  end

  defp broadcast_players_ping(player, ping) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(
      Communication.pubsub_game_topic(self()),
      {:update_ping, player, ping}
    )
  end

  defp get_player(players, player_id) do
    Enum.find(players, fn p -> p.id == player_id end)
  end
end
