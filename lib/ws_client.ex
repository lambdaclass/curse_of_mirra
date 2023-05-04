defmodule DarkWorldsServer.WsClient do
  use WebSockex
  require Logger
  alias DarkWorldsServer.Communication

  def start_link(url) do
    WebSockex.start_link(url, __MODULE__, %{}, name: __MODULE__)
  end

  def get_board(session_id) do
    runner_pid = Communication.external_id_to_pid(session_id)
    GenServer.call(runner_pid, :get_board)
  end

  def get_players(session_id) do
    runner_pid = Communication.external_id_to_pid(session_id)
    GenServer.call(runner_pid, :get_players)
  end

  def move(player, :up), do: _move(player, "up")
  def move(player, :down), do: _move(player, "down")
  def move(player, :left), do: _move(player, "left")
  def move(player, :right), do: _move(player, "right")

  def attack(player, :up), do: _attack(player, "up")
  def attack(player, :down), do: _attack(player, "down")
  def attack(player, :left), do: _attack(player, "left")
  def attack(player, :right), do: _attack(player, "right")

  def attack_aoe(player, position) do
    %{
      "player" => player,
      "action" => "attack_aoe",
      "value" => %{"x" => position.x, "y" => position.y}
    }
    |> send_command()
  end

  defp _move(player, direction) do
    %{"player" => player, "action" => "move", "value" => direction}
    |> send_command()
  end

  defp _attack(player, direction) do
    %{"player" => player, "action" => "attack", "value" => direction}
    |> send_command()
  end

  def handle_frame({_type, _msg}, state) do
    {:ok, state}
  end

  def handle_cast({:send, {_type, msg} = frame}, state) do
    Logger.info("Sending frame with payload: #{msg}")
    {:reply, frame, state}
  end

  defp send_command(command) do
    pid = Process.whereis(__MODULE__)
    WebSockex.cast(pid, {:send, {:text, Jason.encode!(command)}})
  end
end
