defmodule DarkWorldsServerWeb.BoardLive.Show do
  use DarkWorldsServerWeb, :live_view

  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.ActionOk
  alias DarkWorldsServer.Engine.RequestTracker
  alias DarkWorldsServer.Engine.Runner

  def mount(%{"game_id" => game_id} = params, _session, socket) do
    if connected?(socket) do
      mount_connected(params, socket)
    else
      {:ok, assign(socket, game_id: game_id, game_status: :pending)}
    end
  end

  defp mount_connected(%{"game_id" => game_id, "player_id" => player_id_str}, socket) do
    Phoenix.PubSub.subscribe(DarkWorldsServer.PubSub, "game_play_#{game_id}")
    runner_pid = Communication.external_id_to_pid(game_id)
    player_id = String.to_integer(player_id_str)

    state = Runner.get_game_state(runner_pid)
    players = state.game.players
    {board_width, board_height} = {state.game.board.height, state.game.board.width}

    {mode, player_id} =
      case Runner.join(runner_pid, player_id) do
        {:ok, player_id} -> {:player, player_id}
        {:error, :game_full} -> {:spectator, nil}
      end

    logged_players = Runner.get_logged_players(runner_pid)

    new_assigns = %{
      game_status: :ongoing,
      runner_pid: runner_pid,
      board_height: board_height,
      board_width: board_width,
      players: players_by_position(players),
      game_id: game_id,
      pings: %{},
      player_id: player_id,
      player_direction: :up,
      mode: mode,
      logged_players: logged_players
    }

    {:ok, assign(socket, new_assigns)}
  end

  def handle_event("action", %{"key" => key}, socket) do
    new_socket = maybe_send_action(socket, key)
    {:noreply, new_socket}
  end

  def handle_event("action", _, socket) do
    {:noreply, socket}
  end

  def handle_info({:game_update, game_state}, socket) do
    players = game_state.current_state.game.players
    {:noreply, assign(socket, :players, players_by_position(players))}
  end

  def handle_info({:game_finished, %{current_state: %{game: game}}}, socket) do
    {:noreply, assign(socket, game_status: :finished, players: game.players)}
  end

  # def handle_info({:update_ping, player, ping}, socket) do
  #   pings = socket.assigns.pings

  #   {
  #     :noreply,
  #     socket
  #     |> assign(:pings, Map.put(pings, player, ping))
  #   }
  # end

  def handle_info(_, socket) do
    {:noreply, socket}
  end

  defp players_by_position(players) do
    Enum.reduce(players, %{}, fn player, acc ->
      case player do
        %{health: health, position: %{x: x, y: y}} when health > 0 -> Map.put(acc, {x, y}, player)
        _ -> acc
      end
    end)
  end

  def maybe_send_action(socket, key) do
    case get_action(key, socket.assigns.player_direction) do
      :no_action ->
        socket

      %ActionOk{} = action ->
        RequestTracker.add_counter(socket.assigns.runner_pid, socket.assigns.player_id)
        Runner.play(socket.assigns.runner_pid, socket.assigns.player_id, action)
        assign(socket, :player_direction, action.value)
    end
  end

  def get_action("w", _), do: %ActionOk{action: :move, value: :up}
  def get_action("s", _), do: %ActionOk{action: :move, value: :down}
  def get_action("a", _), do: %ActionOk{action: :move, value: :left}
  def get_action("d", _), do: %ActionOk{action: :move, value: :right}
  def get_action("e", position), do: %ActionOk{action: :attack_aoe, value: position}
  def get_action(" ", direction), do: %ActionOk{action: :attack, value: direction}
  def get_action(_, _), do: :no_action
end
