defmodule DarkWorldsServer.Engine.Runner do
  use GenServer, restart: :transient

  alias DarkWorldsServer.Engine.Game
  alias DarkWorldsServer.Engine.{ActionOk}

  @players 3
  @board {100, 100}
  # The game will be closed five minute after it starts
  @game_timeout 20 * 60 * 1000
  # The session will be closed one minute after the game has finished
  @session_timeout 60 * 1000

  def start_link(args) do
    GenServer.start_link(__MODULE__, args)
  end

  def init(_opts) do
    state = Game.new(number_of_players: @players, board: @board, build_walls: false)
    IO.inspect(state)
    IO.inspect("To join: #{pid_to_game_id(self())}")
    Process.send_after(self(), :game_timeout, @game_timeout)
    {:ok, %{game: state, has_finished?: false, max_players: @players, current_players: 0}}
  end

  def join(runner_pid) do
    GenServer.call(runner_pid, :join)
  end

  def play(runner_pid, player_id, %ActionOk{} = action) do
    GenServer.cast(runner_pid, {:play, player_id, action})
  end

  def get_board(runner_pid) do
    GenServer.call(runner_pid, :get_board)
  end

  def get_players(runner_pid) do
    GenServer.call(runner_pid, :get_players)
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :move, value: value}},
        %{game: game} = state
      ) do
    game =
      game
      |> Game.move_player(player, value)

    new_state = Map.put(state, :game, game)

    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast("game_play_#{pid_to_game_id(self())}", {:move, new_state})

    {:noreply, new_state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :attack, value: value}},
        %{game: game} = state
      ) do
    game =
      game
      |> Game.attack_player(player, value)

    has_a_player_won? = has_a_player_won?(game.players)
    maybe_broadcast_game_finished_message(has_a_player_won?, state)

    state = state |> Map.put(:game, game) |> Map.put(:has_finished?, has_a_player_won?)

    {:noreply, state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :attack_aoe, value: value}},
        %{game: game} = state
      ) do
    game =
      game
      |> Game.attack_aoe(player, value)

    has_a_player_won? = has_a_player_won?(game.players)
    maybe_broadcast_game_finished_message(has_a_player_won?, state)

    state = state |> Map.put(:game, game) |> Map.put(:has_finished?, has_a_player_won?)

    {:noreply, state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :update_ping, value: value}},
        state
      ) do
    broadcast_players_ping(player, value)

    {:noreply, state}
  end

  def handle_call(:join, _, %{max_players: max, current_players: current} = state)
      when current < max do
    {:reply, {:ok, current + 1}, %{state | current_players: current + 1}}
  end

  def handle_call(:join, _, %{max_players: max, current_players: max} = state) do
    {:reply, {:error, :game_full}, state}
  end

  def handle_call(:get_board, _from, %{game: %Game{board: board}} = state) do
    {:reply, board, state}
  end

  def handle_call(:get_players, _from, %{game: %Game{players: players}} = state) do
    {:reply, players, state}
  end

  def handle_info(:game_timeout, state) do
    Process.send_after(self(), :session_timeout, @session_timeout)

    {:noreply, Map.put(state, :has_finished?, true)}
  end

  def handle_info(:session_timeout, state) do
    IO.inspect(self(), label: "session timeout")

    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(
      "game_play_#{pid_to_game_id(self())}",
      {:game_finished, state.game}
    )

    {:stop, :normal, state}
  end

  def pid_to_game_id(pid) do
    pid |> :erlang.term_to_binary() |> Base58.encode()
  end

  def game_id_to_pid(game_id) do
    game_id |> Base58.decode() |> :erlang.binary_to_term([:safe])
  end

  def game_has_finished?(pid) do
    %{has_finished?: has_finished?} = :sys.get_state(pid)
    has_finished?
  end

  defp has_a_player_won?(players) do
    players_alive = Enum.filter(players, fn player -> player.health != 0 end)
    Enum.count(players_alive) == 1
  end

  defp maybe_broadcast_game_finished_message(true, state) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast("game_play_#{pid_to_game_id(self())}", {:game_finished, state})

    Process.send_after(self(), :session_timeout, @session_timeout)
  end

  defp maybe_broadcast_game_finished_message(_false, state) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast("game_play_#{pid_to_game_id(self())}", {:attack, state})
  end

  defp broadcast_players_ping(player, ping) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.broadcast(
      "game_play_#{pid_to_game_id(self())}",
      {:update_ping, player, ping}
    )
  end
end
