defmodule DarkWorldsServerWeb.BoardLive.Index do
  use DarkWorldsServerWeb, :live_view

  alias DarkWorldsServer.Engine
  alias DarkWorldsServer.Engine.{Runner, Board}

  def mount(%{"game_id" => game_id}, _session, socket) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.subscribe("game_play_#{game_id}")

    runner_pid = game_id |> Runner.game_id_to_pid()
    %Board{grid: grid} = Runner.get_board(runner_pid)
    players = Runner.get_players(runner_pid)

    {
      :ok,
      socket
      |> assign(runner_pid: runner_pid, grid: grid, players: players, game_id: game_id)
    }
  end

  def mount(_params, _session, socket) do
    {:ok, socket}
  end

  def handle_params(%{"game_id" => game_id}, _url, socket) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.subscribe("game_play_#{game_id}")

    runner_pid = game_id |> Runner.game_id_to_pid()
    %Board{grid: grid} = Runner.get_board(runner_pid)
    players = Runner.get_players(runner_pid)

    {:noreply, socket |> assign(runner_pid: runner_pid, grid: grid, players: players)}
  end

  def handle_params(_params, _url, socket) do
    {:noreply, socket}
  end

  def handle_info({:move, %Board{grid: grid}}, socket) do
    {
      :noreply,
      socket
      |> assign(:grid, grid)
    }
  end

  def handle_info({:attack, game}, socket) do
    {
      :noreply,
      socket
      |> assign(:players, game.players)
      |> assign(:grid, game.board.grid)
    }
  end

  def handle_info({:game_finished, game}, socket) do
    {
      :noreply,
      socket
      |> assign(:players, game.players)
    }
  end

  def handle_event("update_ping", %{"ping" => ping}, socket) do
    {:noreply, assign(socket, :ping, ping)}
  end
end
