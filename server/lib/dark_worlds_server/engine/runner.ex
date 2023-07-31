defmodule DarkWorldsServer.Engine.Runner do
  use GenServer, restart: :transient
  require Logger
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.ActionOk
  alias DarkWorldsServer.Engine.BotPlayer
  alias DarkWorldsServer.Engine.Game

  @build_walls false
  # The game will be closed twenty minute after it starts
  @game_timeout 20 * 60 * 1000
  # The session will be closed one minute after the game has finished
  @session_timeout 60 * 1000
  # This is the amount of time between state updates in milliseconds
  @tick_rate_ms 20
  # This is the amount of time that players have to select a character
  @character_selection_timeout_ms 60 * 1000
  # This is the amount of time to check if all players are set
  @character_selection_check_ms 30
  # This is the amount of time to wait until the game starts, ofc we should change it
  @game_start_timer_ms 30
  # Amount of time to wait before starting to shrink the map
  @map_shrink_wait_ms 60_000
  # Amount of time between map shrinking
  @map_shrink_interval_ms 100

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

    Process.send_after(self(), :all_characters_set?, @character_selection_check_ms)
    Process.send_after(self(), :character_selection_time_out, @character_selection_timeout_ms)
    Process.send_after(self(), :check_player_amount, @player_check)

    {:ok,
     %{
       selected_characters: %{},
       max_players: length(opts.players),
       current_players: 0,
       players: opts.players,
       is_single_player?: length(opts.players) == 1,
       game_status: :character_selection,
       player_timestamps: %{},
       bot_handler_pid: nil,
       opts: opts
     }}
  end

  def handle_cast(_actions, %{game_status: :game_finished} = gen_server_state) do
    {:noreply, gen_server_state}
  end

  def handle_cast(
        {:play, _player,
         %ActionOk{
           action: :select_character,
           value: %{player_id: player_id, character_name: character_name}
         }},
        %{selected_characters: selected_characters} = gen_server_state
      ) do
    selected_characters = Map.put(selected_characters, player_id, character_name)

    broadcast_to_darkworlds_server({:selected_characters, selected_characters})

    {:noreply, %{gen_server_state | selected_characters: selected_characters}}
  end

  ## This will handle the case where players could send player movement actions or attacks
  ## When game is on character selection screen
  def handle_cast(
        {:play, _, _},
        %{game_status: :character_selection} = gen_server_state
      ) do
    {:noreply, gen_server_state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: action, value: value, timestamp: timestamp}},
        %{server_game_state: server_game_state} = gen_server_state
      )
      when action in [:move, :move_with_joystick] do
    {:ok, game} = do_move(action, server_game_state.game, player, value)

    server_game_state = %{server_game_state | game: game}

    gen_server_state =
      Map.put(gen_server_state, :server_game_state, server_game_state)
      |> set_timestamp_for_player(timestamp, player)

    {:noreply, gen_server_state}
  end

  def handle_cast(
        {:play, player, %ActionOk{action: :move, value: value, timestamp: timestamp}},
        %{server_game_state: %{game: game} = server_game_state} = gen_server_state
      ) do
    {:ok, game} =
      game
      |> Game.move_player(player, value)

    server_game_state = Map.put(server_game_state, :game, game)

    gen_server_state
    |> Map.put(:server_game_state, server_game_state)
    |> set_timestamp_for_player(timestamp, player)

    {:noreply, gen_server_state}
  end

  def handle_cast(
        {:play, player_id, %ActionOk{action: :teleport, value: position_transform, timestamp: timestamp}},
        %{next_state: next_state} = gen_server_state
      ) do
    game =
      next_state.game
      |> Game.move_player_to_relative_position(player_id, position_transform)

    next_state = Map.put(next_state, :game, game)

    gen_server_state =
      Map.put(gen_server_state, :next_state, next_state)
      |> set_timestamp_for_player(timestamp, player_id)

    {:noreply, gen_server_state}
  end

  def handle_cast(
        {:play, player_id, %ActionOk{action: action, value: value, timestamp: timestamp}},
        %{server_game_state: server_game_state} = gen_server_state
      )
      when action in [:basic_attack, :skill_1, :skill_2, :skill_3, :skill_4] do
    {:ok, game} = do_action(action, server_game_state.game, player_id, value)

    server_game_state = server_game_state |> Map.put(:game, game)

    gen_server_state =
      Map.put(gen_server_state, :server_game_state, server_game_state)
      |> set_timestamp_for_player(timestamp, player_id)

    {:noreply, gen_server_state}
  end

  def handle_cast({:play, _, %ActionOk{action: :add_bot}}, gen_server_state) do
    game_state = gen_server_state.server_game_state

    bot_id = gen_server_state.current_players + 1
    {:ok, new_game} = Game.spawn_player(game_state.game, bot_id)

    broadcast_to_darkworlds_server({:player_joined, bot_id})

    selected_characters = Map.put(gen_server_state.selected_characters, bot_id, "Muflus")

    broadcast_to_darkworlds_server({:selected_characters, selected_characters})

    bot_handler_pid =
      case gen_server_state[:bot_handler_pid] do
        nil ->
          {:ok, pid} = BotPlayer.start_link(self(), gen_server_state.tick_rate)
          pid

        bot_handler_pid ->
          bot_handler_pid
      end

    BotPlayer.add_bot(bot_handler_pid, bot_id)

    {:noreply,
     %{
       gen_server_state
       | server_game_state: %{game_state | game: new_game},
         current_players: gen_server_state.current_players + 1,
         selected_characters: selected_characters,
         bot_handler_pid: bot_handler_pid
     }}
  end

  def handle_cast({:play, _, %ActionOk{action: :disable_bots}}, gen_server_state) do
    bot_pid = gen_server_state[:bot_handler_pid]

    if bot_pid do
      BotPlayer.disable_bots(bot_pid)
    end

    {:noreply, gen_server_state}
  end

  def handle_cast({:play, _, %ActionOk{action: :enable_bots}}, gen_server_state) do
    bot_pid = gen_server_state[:bot_handler_pid]

    if bot_pid do
      BotPlayer.enable_bots(bot_pid)
    end

    {:noreply, gen_server_state}
  end

  def handle_cast(
        {:disconnect, player_id},
        %{client_game_state: game_state} = gen_server_state
      ) do
    current = gen_server_state.current_players - 1
    {:ok, game} = Game.disconnect(game_state.game, player_id)

    {:noreply, %{gen_server_state | client_game_state: %{game_state | game: game}, current_players: current}}
  end

  def handle_cast(
        {:disconnect, player_id},
        %{game_status: :character_selection} = gen_server_state
      ) do
    current = gen_server_state.current_players - 1
    selected_characters = Map.delete(gen_server_state.selected_characters, player_id)

    {:noreply, %{gen_server_state | current_players: current, selected_characters: selected_characters}}
  end

  def handle_call({:join, player_id}, _, gen_server_state) do
    if gen_server_state.current_players < gen_server_state.max_players do
      broadcast_to_darkworlds_server({:player_joined, player_id})

      {:reply, {:ok, player_id}, %{gen_server_state | current_players: gen_server_state.current_players + 1}}
    else
      {:reply, {:error, :game_full}, gen_server_state}
    end
  end

  def handle_call(:get_board, _from, gen_server_state) do
    {:reply, gen_server_state.client_game_state.game.board, gen_server_state}
  end

  def handle_call(:get_players, _from, gen_server_state) do
    {:reply, gen_server_state.client_game_state.game.players, gen_server_state}
  end

  def handle_call(:get_logged_players, _from, gen_server_state) do
    {:reply, gen_server_state.players, gen_server_state}
  end

  def handle_call(:get_state, _from, gen_server_state) do
    {:reply, gen_server_state.client_game_state, gen_server_state}
  end

  def handle_info(:all_characters_set?, gen_server_state) do
    {:noreply, all_characters_set?(gen_server_state)}
  end

  def handle_info(:character_selection_time_out, gen_server_state) do
    {:noreply, character_selection_time_out(gen_server_state)}
  end

  def handle_info(:start_game, gen_server_state) do
    opts = gen_server_state.opts
    selected_players = gen_server_state.selected_characters

    {:ok, game} = create_new_game(opts.game_config, gen_server_state.max_players, selected_players)

    Logger.info("#{DateTime.utc_now()} Starting runner, pid: #{inspect(self())}")

    tick_rate = Map.get(opts.game_config.runner_config, :server_tickrate_ms, @tick_rate_ms)

    # Finish game after @game_timeout seconds or the specified in the game_settings file
    Process.send_after(
      self(),
      :game_timeout,
      Map.get(opts.game_config, :game_timeout, @game_timeout)
    )

    map_shrink_wait_ms = Map.get(opts.game_config.runner_config, :map_shrink_wait_ms, @map_shrink_wait_ms)

    Process.send_after(self(), :update_state, tick_rate)
    Process.send_after(self(), :shrink_map, map_shrink_wait_ms)

    gen_server_state =
      gen_server_state
      |> Map.put(:client_game_state, %{game: game})
      |> Map.put(:server_game_state, %{game: game})
      |> Map.put(:game_status, :playing)
      |> Map.put(:winners, [])
      |> Map.put(:tick_rate, tick_rate)

    broadcast_to_darkworlds_server(
      {:finish_character_selection, selected_players, gen_server_state.client_game_state.game.players}
    )

    {:noreply, gen_server_state}
  end

  def handle_info(:check_player_amount, gen_server_state) do
    if gen_server_state.current_players > 0 do
      Process.send_after(self(), :check_player_amount, @player_check)
      {:noreply, gen_server_state}
    else
      Process.send_after(self(), :session_timeout, 500)
      {:noreply, Map.put(gen_server_state, :game_status, :game_finished)}
    end
  end

  def handle_info(:game_timeout, gen_server_state) do
    Process.send_after(self(), :session_timeout, @session_timeout)

    {:noreply, Map.put(gen_server_state, :game_status, :game_finished)}
  end

  def handle_info(:session_timeout, gen_server_state) do
    {:stop, :normal, gen_server_state}
  end

  def handle_info(:update_state, %{server_game_state: server_game_state} = gen_server_state) do
    gen_server_state = Map.put(gen_server_state, :client_game_state, server_game_state)

    game_status = has_a_player_won?(server_game_state.game.players, gen_server_state.is_single_player?)
    out_of_area_damage = gen_server_state.opts.game_config.runner_config.out_of_area_damage

    game =
      server_game_state.game
      |> Game.world_tick(out_of_area_damage)

    server_game_state = server_game_state |> Map.put(:game, game)

    gen_server_state =
      Map.put(gen_server_state, :server_game_state, server_game_state)
      |> Map.put(:game_status, game_status)

    decide_next_game_update(gen_server_state)
    |> broadcast_game_update()
  end

  def handle_info(:shrink_map, %{game_status: :game_finished} = gen_server_state),
    do: {:noreply, gen_server_state}

  def handle_info(:shrink_map, %{server_game_state: server_game_state} = gen_server_state) do
    map_shrink_interval_ms =
      Map.get(gen_server_state.opts.game_config.runner_config, :map_shrink_interval_ms, @map_shrink_interval_ms)

    Process.send_after(self(), :shrink_map, map_shrink_interval_ms)

    {:ok, game} = Game.shrink_map(server_game_state.game)
    gen_server_state = put_in(gen_server_state, [:server_game_state, :game], game)

    {:noreply, gen_server_state}
  end

  ####################
  # Internal helpers #
  ####################
  defp set_timestamp_for_player(gen_server_state, timestamp, player_id) do
    player_timestamps = gen_server_state.player_timestamps |> Map.put(player_id, timestamp)
    Map.put(gen_server_state, :player_timestamps, player_timestamps)
  end

  defp has_a_player_won?(_players, true = _is_single_player?), do: :playing

  defp has_a_player_won?(players, _is_single_player?) do
    players_alive =
      Enum.filter(players, fn player ->
        player.status == :alive
      end)

    if Enum.count(players_alive) == 1, do: :game_finished, else: :playing
  end

  defp decide_next_game_update(%{game_status: :game_finished} = gen_server_state) do
    [winner] =
      Enum.filter(gen_server_state.server_game_state.game.players, fn player ->
        player.status == :alive
      end)

    {:game_finished, gen_server_state, winner}
  end

  defp decide_next_game_update(%{game_status: :playing} = gen_server_state) do
    {:game_update, gen_server_state}
  end

  defp broadcast_game_update({:game_update, gen_server_state}) do
    broadcast_to_darkworlds_server({:game_update, gen_server_state})

    Process.send_after(self(), :update_state, gen_server_state.tick_rate)

    {:noreply, gen_server_state}
  end

  defp broadcast_game_update({:game_finished, gen_server_state, winner}) do
    # Needed to show the last tick that finished the game
    broadcast_to_darkworlds_server({:game_update, gen_server_state})

    broadcast_to_darkworlds_server({:game_finished, winner, gen_server_state})

    Process.send_after(self(), :session_timeout, @session_timeout)

    {:noreply, gen_server_state}
  end

  defp broadcast_to_darkworlds_server(message),
    do:
      Phoenix.PubSub.broadcast(
        DarkWorldsServer.PubSub,
        Communication.pubsub_game_topic(self()),
        message
      )

  defp create_new_game(
         %{
           runner_config: rg,
           character_config: %{Items: characters_info},
           skills_config: %{Items: skills_info}
         },
         players,
         selected_players
       ) do
    characters_info = config_atom_to_string(characters_info)
    skills_info = config_atom_to_string(skills_info)

    config = %{
      selected_players: selected_players,
      number_of_players: players,
      board: {rg.board_width, rg.board_height},
      build_walls: @build_walls,
      characters: characters_info,
      skills: skills_info
    }

    {:ok, _game} = Game.new(config)
  end

  defp all_characters_set?(state) do
    cond do
      Map.get(state, :game_status) == :playing ->
        nil

      Map.get(state, :selected_characters, %{}) |> map_size() == state[:max_players] ->
        Process.send_after(self(), :start_game, @game_start_timer_ms)

      true ->
        Process.send_after(self(), :all_characters_set?, @character_selection_check_ms)
    end

    state
  end

  defp character_selection_time_out(state) do
    selected_characters = state[:selected_characters]

    cond do
      state[:game_status] == :playing or state[:game_status] == :game_finished ->
        state

      not is_nil(selected_characters) and map_size(selected_characters) < state[:max_players] ->
        players_with_character = Enum.map(selected_characters, fn selected_char -> selected_char.player_id end)

        players_without_character =
          Enum.filter(state[:players], fn player_id ->
            player_id not in players_with_character
          end)

        selected_characters =
          Enum.reduce(players_without_character, selected_characters, fn player_id, map ->
            character_name = Enum.random(["H4ck", "Muflus", "Uma"])
            Map.put(map, player_id, character_name)
          end)

        Process.send_after(self(), :start_game, @game_start_timer_ms)

        %{state | selected_characters: selected_characters}

      true ->
        Process.send_after(self(), :start_game, @game_start_timer_ms)
        state
    end
  end

  defp do_move(:move_with_joystick, game, player, %{x: x, y: y}),
    do: Game.move_with_joystick(game, player, x, y)

  defp do_move(:move, game, player, value), do: Game.move_player(game, player, value)

  defp do_action(:basic_attack, game, player_id, value),
    do: Game.basic_attack(game, player_id, value)

  defp do_action(:skill_1, game, player_id, value), do: Game.skill_1(game, player_id, value)
  defp do_action(:skill_2, game, player_id, value), do: Game.skill_2(game, player_id, value)
  defp do_action(:skill_3, game, player_id, value), do: Game.skill_3(game, player_id, value)
  defp do_action(:skill_4, game, player_id, value), do: Game.skill_4(game, player_id, value)

  defp config_atom_to_string(config) do
    Enum.reduce(config, [], fn item, acc_config ->
      new_item =
        item
        |> Map.delete(:__unknown_fields__)
        |> Map.new(fn {key, val} -> {Atom.to_string(key), val} end)

      [new_item | acc_config]
    end)
  end
end
