defmodule DarkWorldsServer.PlayerTest do
  use DarkWorldsServerWeb.ConnCase, async: true
  alias DarkWorldsServer.WsClient

  setup_all do
    %{game_config: config} = DarkWorldsServer.Test.game_config()
    player_id = 1
    session_id = create_session(config)
    {:ok, _ws_pid} = ws_connect(session_id, player_id)

    WsClient.set_character_muflus(player_id, session_id)
    Process.sleep(1_000)

    %{session_id: session_id}
  end

  # Gets the position of anything that would impede a player from moving into a cell in the grid,
  # be it a wall or another player
  def get_wall_coordinates(board_matrix) do
    board_matrix
    |> Enum.with_index()
    |> Enum.flat_map(fn {row, x} ->
      row
      |> Enum.with_index()
      |> Enum.filter(fn {cell, _} -> cell == :wall end)
      |> Enum.map(fn {_, y} -> {x, y} end)
    end)
  end

  defp create_session(config) do
    {:ok, session_id} = DarkWorldsServer.Engine.start_child(%{players: [1], game_config: config})
    DarkWorldsServer.Communication.pid_to_external_id(session_id)
  end

  defp ws_connect(session_id, player_id) do
    WsClient.start_link("ws://localhost:4002/play/#{session_id}/#{player_id}")
  end
end
