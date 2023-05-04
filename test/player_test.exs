defmodule DarkWorldsServer.PlayerTest do
  use DarkWorldsServerWeb.ConnCase, async: true

  alias DarkWorldsServer.WsClient
  alias Plug.Conn

  describe "Move the player around" do
    @tag :move_up
    test "Move up", %{conn: conn} do
      session_id = create_session(conn)
      {:ok, _ws_pid} = ws_connect(session_id)
      board = WsClient.get_board(session_id)

      first_player_before_moving = WsClient.get_players(session_id) |> List.first()
      WsClient.move(1, :up)
      :timer.sleep(1_000)
      first_player_after_moving = WsClient.get_players(session_id) |> List.first()

      x_before_eq_after =
        first_player_before_moving.position.x == first_player_after_moving.position.x

      contiguous_position_is_wall_or_player =
        (Enum.at(board.grid, first_player_before_moving.position.x - 1)
         |> Enum.at(first_player_before_moving.position.y)) in [:wall, :player]

      movement = first_player_after_moving.position.x == first_player_before_moving.position.x - 1

      # first condition checks if player moved as expected
      # if player didn't move as expected, the next conditions check for cases where this output is valid (presence of a wall, player or end of the board)
      success =
        case x_before_eq_after do
          true when contiguous_position_is_wall_or_player -> true
          true when first_player_before_moving.position.x == 0 -> true
          true -> false
          false -> movement
        end

      assert success
    end

    @tag :move_down
    test "Move down", %{conn: conn} do
      session_id = create_session(conn)
      {:ok, _ws_pid} = ws_connect(session_id)
      board = WsClient.get_board(session_id)

      first_player_before_moving = WsClient.get_players(session_id) |> List.first()
      WsClient.move(1, :down)
      :timer.sleep(1_000)
      first_player_after_moving = WsClient.get_players(session_id) |> List.first()

      x_before_eq_after =
        first_player_before_moving.position.x == first_player_after_moving.position.x

      contiguous_position_is_wall_or_player =
        (Enum.at(board.grid, first_player_before_moving.position.x + 1)
         |> Enum.at(first_player_before_moving.position.y)) in [:wall, :player]

      movement = first_player_after_moving.position.x == first_player_before_moving.position.x + 1

      # first condition checks if player moved as expected
      # if player didn't move as expected, the next conditions check for cases where this output is valid (presence of a wall, player or end of the board)
      success =
        case x_before_eq_after do
          true when contiguous_position_is_wall_or_player -> true
          true when first_player_before_moving.position.x == 0 -> true
          true -> false
          false -> movement
        end

      assert success
    end

    @tag :move_left
    test "Move left", %{conn: conn} do
      session_id = create_session(conn)
      {:ok, _ws_pid} = ws_connect(session_id)
      board = WsClient.get_board(session_id)

      first_player_before_moving = WsClient.get_players(session_id) |> List.first()
      WsClient.move(1, :left)
      :timer.sleep(1_000)
      first_player_after_moving = WsClient.get_players(session_id) |> List.first()

      y_before_eq_after =
        first_player_before_moving.position.y == first_player_after_moving.position.y

      contiguous_position =
        Enum.at(board.grid, first_player_before_moving.position.x)
        |> Enum.at(first_player_before_moving.position.y - 1)

      contiguous_position_is_wall_or_player = contiguous_position in [:wall, :player]

      # first condition checks if player moved as expected
      # if player didn't move as expected, the next conditions check for cases where this output is valid (presence of a wall, player or end of the board)
      player_moves_unless_theres_an_obstacle =
        case y_before_eq_after do
          true when contiguous_position_is_wall_or_player ->
            true

          true when first_player_before_moving.position.x == 0 ->
            true

          true ->
            false

          false ->
            first_player_after_moving.position.y == first_player_before_moving.position.y - 1
        end

      assert player_moves_unless_theres_an_obstacle
    end

    @tag :move_right
    test "Move right", %{conn: conn} do
      session_id = create_session(conn)
      {:ok, _ws_pid} = ws_connect(session_id)
      board = WsClient.get_board(session_id)

      first_player_before_moving = WsClient.get_players(session_id) |> List.first()
      WsClient.move(1, :right)
      :timer.sleep(1_000)
      first_player_after_moving = WsClient.get_players(session_id) |> List.first()

      y_before_eq_after =
        first_player_before_moving.position.y == first_player_after_moving.position.y

      contiguous_position_is_wall_or_player =
        (Enum.at(board.grid, first_player_before_moving.position.x)
         |> Enum.at(first_player_before_moving.position.y + 1)) in [:wall, :player]

      movement = first_player_after_moving.position.y == first_player_before_moving.position.y + 1

      # first condition checks if player moved as expected
      # if player didn't move as expected, the next conditions check for cases where this output is valid (presence of a wall, player or end of the board)
      success =
        case y_before_eq_after do
          true when contiguous_position_is_wall_or_player -> true
          true when first_player_before_moving.position.x == 0 -> true
          true -> false
          false -> movement
        end

      assert success
    end
  end

  # describe "Perform attacks" do
  #   @tag :attack_aoe
  #   test "Attack AOE", %{conn: conn} do
  #     session_id = create_session(conn)
  #     {:ok, _ws_pid} = ws_connect(session_id)
  #     board = WsClient.get_board(session_id)

  #     victim_health_before_attack = WsClient.get_players(session_id) |> List.first()
  #     WsClient.move(2, :attack_aoe)
  #     :timer.sleep(1_000)
  #     victim_health_after_attack = WsClient.get_players(session_id) |> List.first()


  defp create_session(conn) do
    conn = Conn.put_req_header(conn, "content-type", "application/json")
    new_session = get(conn, ~p"/new_session", %{})
    new_session = json_response(new_session, 200)
    Map.get(new_session, "session_id")
  end

  defp ws_connect(session_id) do
    WsClient.start_link("ws://localhost:4002/play/#{session_id}")
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
end
