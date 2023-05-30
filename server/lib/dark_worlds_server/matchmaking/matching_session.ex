defmodule DarkWorldsServer.Matchmaking.MatchingSession do
  use GenServer, restart: :transient
  alias DarkWorldsServer.Engine
  alias DarkWorldsServer.Matchmaking

  # 2 minutes
  @timeout_ms 2 * 60 * 1000

  #######
  # API #
  #######
  def start_link(_args) do
    GenServer.start_link(__MODULE__, [])
  end

  def add_player(player_id, session_pid) do
    GenServer.call(session_pid, {:add_player, player_id})
  end

  def remove_player(player_id, session_pid) do
    GenServer.call(session_pid, {:remove_player, player_id})
  end

  def list_players(session_pid) do
    GenServer.call(session_pid, :list_players)
  end

  def fetch_amount_of_players(session_pid) do
    GenServer.call(session_pid, :fetch_amount_of_players)
  end

  def start_game(session_pid) do
    GenServer.cast(session_pid, :start_game)
  end

  #######################
  # GenServer callbacks #
  #######################
  @impl GenServer
  def init(_args) do
    Process.send_after(self(), :check_timeout, @timeout_ms * 2)
    session_id = :erlang.term_to_binary(self()) |> Base58.encode()
    topic = Matchmaking.session_topic(session_id)
    {:ok, %{players: [], session_id: session_id, topic: topic}}
  end

  @impl GenServer
  def handle_call({:add_player, player}, _from, state) do
    players = state[:players]

    case Enum.member?(players, player) do
      true ->
        {:reply, :ok, state}

      false ->
        send(self(), {:player_added, player})
        {:reply, :ok, %{state | :players => [player | players]}}
    end
  end

  def handle_call({:remove_player, player}, _from, state) do
    players = state[:players]

    case List.delete(players, player) do
      ^players ->
        {:reply, :ok, state}

      remaining_players ->
        send(self(), {:player_removed, player})
        {:reply, :ok, %{state | :players => remaining_players}}
    end
  end

  def handle_call(:list_players, _from, state) do
    {:reply, state[:players], state}
  end

  def handle_call(:fetch_amount_of_players, _from, state) do
    {:reply, length(state[:players]), state}
  end

  @impl GenServer
  def handle_cast(:start_game, state) do
    {:ok, game_pid} = Engine.start_child(%{players: state.players})
    Phoenix.PubSub.broadcast!(DarkWorldsServer.PubSub, state[:topic], {:game_started, game_pid})
    {:stop, :normal, state}
  end

  @impl GenServer
  def handle_info({:player_added, player}, state) do
    Phoenix.PubSub.broadcast!(
      DarkWorldsServer.PubSub,
      state[:topic],
      {:player_added, player, state[:players]}
    )

    {:noreply, state}
  end

  def handle_info({:player_removed, player}, state) do
    Phoenix.PubSub.broadcast!(
      DarkWorldsServer.PubSub,
      state[:topic],
      {:player_removed, player, state[:players]}
    )

    {:noreply, state}
  end

  def handle_info(:pong, state) do
    {:noreply, state}
  end

  def handle_info(:timeout, state) do
    {:stop, :normal, state}
  end

  def handle_info(:check_timeout, state) do
    Phoenix.PubSub.broadcast!(DarkWorldsServer.PubSub, state[:topic], {:ping, self()})
    Process.send_after(self(), :check_timeout, @timeout_ms * 2)
    {:noreply, state, @timeout_ms}
  end
end
