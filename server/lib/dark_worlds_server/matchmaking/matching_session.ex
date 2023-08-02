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

  def add_player(player_id, player_name, session_pid) do
    GenServer.call(session_pid, {:add_player, player_id, player_name})
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

  def start_game(game_config, session_pid) do
    GenServer.cast(session_pid, {:start_game, game_config})
  end

  #######################
  # GenServer callbacks #
  #######################
  @impl GenServer
  def init(_args) do
    Process.send_after(self(), :check_timeout, @timeout_ms * 2)
    session_id = :erlang.term_to_binary(self()) |> Base58.encode()
    topic = Matchmaking.session_topic(session_id)
    {:ok, %{players: %{}, host_player_id: nil, session_id: session_id, topic: topic}}
  end

  @impl GenServer
  def handle_call({:add_player, player_id, player_name}, _from, state) do
    players = state[:players]

    case Map.has_key?(players, player_id) do
      true ->
        {:reply, :ok, state}

      false ->
        send(self(), {:player_added, player_id, player_name})

        players = Map.put(players, player_id, player_name)
        host_player_id = state.host_player_id || player_id
        {:reply, :ok, %{state | players: players, host_player_id: host_player_id}}
    end
  end

  def handle_call({:remove_player, player_id}, _from, state) do
    players = state[:players]

    case Map.delete(players, player_id) do
      ^players ->
        {:reply, :ok, state}

      empty_map when map_size(empty_map) == 0 ->
        {:stop, :normal, :ok, %{state | :players => %{}}}

      remaining_players ->
        send(self(), {:player_removed, player_id})

        host_player_id =
          case state.host_player_id do
            ^player_id -> Map.keys(remaining_players) |> Enum.random()
            _ -> state.host_player_id
          end

        {:reply, :ok, %{state | :players => remaining_players, host_player_id: host_player_id}}
    end
  end

  def handle_call(:list_players, _from, state) do
    {:reply, Map.keys(state.players), state}
  end

  def handle_call(:fetch_amount_of_players, _from, state) do
    {:reply, Enum.count(state.players), state}
  end

  @impl GenServer
  def handle_cast({:start_game, game_config}, state) do
    {:ok, game_pid} = Engine.start_child(%{players: Map.keys(state.players), game_config: game_config})
    Phoenix.PubSub.broadcast!(DarkWorldsServer.PubSub, state[:topic], {:game_started, game_pid, game_config})
    {:stop, :normal, state}
  end

  @impl GenServer
  def handle_info({:player_added, player_id, player_name}, state) do
    Phoenix.PubSub.broadcast!(
      DarkWorldsServer.PubSub,
      state[:topic],
      {:player_added, player_id, player_name, state.host_player_id, state.players}
    )

    {:noreply, state}
  end

  def handle_info({:player_removed, player_id}, state) do
    Phoenix.PubSub.broadcast!(
      DarkWorldsServer.PubSub,
      state[:topic],
      {:player_removed, player_id, state.host_player_id, state.players}
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
