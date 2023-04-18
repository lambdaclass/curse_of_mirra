defmodule DarkWorldsServerWeb.BoardLive.Index do
  use DarkWorldsServerWeb, :live_view

  alias DarkWorldsServer.Engine
  alias DarkWorldsServer.Engine.{Runner, Board}

  def mount(%{"game_id" => encoded_game_id}, _session, socket) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.subscribe("game_play")

    runner_pid = Base.decode64!(encoded_game_id) |> :erlang.binary_to_term([:safe])
    %Board{grid: grid} = Runner.get_board(runner_pid)
    players = Runner.get_players(runner_pid)

    {
      :ok,
      socket
      |> assign(runner_pid: runner_pid, grid: grid, players: players)
    }
  end

  def mount(_params, _session, socket) do
    DarkWorldsServer.PubSub
    |> Phoenix.PubSub.subscribe("game_play")

    {:ok, runner_pid} = Engine.start_child()
    %Board{grid: grid} = Runner.get_board(runner_pid)
    players = Runner.get_players(runner_pid)

    {:ok,
     socket
     |> assign(runner_pid: runner_pid, grid: grid, players: players)}
  end

  def handle_info({:move, %Board{grid: grid}}, socket) do
    {
      :noreply,
      socket
      |> assign(:grid, grid)
    }
  end

  def handle_info({:attack, players}, socket) do
    {
      :noreply,
      socket
      |> assign(:players, players)
    }
  end
end
