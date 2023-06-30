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

  @tag :player
  describe "Move the player around" do
    @tag :player
    test "Move up", %{session_id: session_id} do
      grid = WsClient.get_grid(session_id)
      character_speed = (WsClient.get_players(session_id) |> List.first()).character.base_speed
      first_player_before_moving = WsClient.get_players(session_id) |> List.first()
      WsClient.move(1, :up)
      :timer.sleep(1_000)
      first_player_after_moving = WsClient.get_players(session_id) |> List.first()

      x_before_eq_after = first_player_before_moving.position.x == first_player_after_moving.position.x

      end_position_is_occupied_by_wall_or_player =
        Enum.at(grid, first_player_before_moving.position.x - character_speed)
        |> is_wall_or_player?()

      movement =
        first_player_after_moving.position.x ==
          first_player_before_moving.position.x - character_speed

      # If the player hasn't moved (x position is the same before and after the move command),
      # we check for valid cases where this is the expected outcome: when the target position is already occupied by
      # a wall or another player, or when the limit of the board is reached.
      # In the third case, we make the test fail if player hasn't moved and none of these valid cases are true
      # In the fourth case, we check if the player has moved as expected
      success =
        case x_before_eq_after do
          true when end_position_is_occupied_by_wall_or_player -> true
          true when first_player_before_moving.position.x < character_speed -> true
          true -> false
          false -> movement
        end

      assert success
    end

    @tag :player
    test "Move down", %{session_id: session_id} do
      grid = WsClient.get_grid(session_id)
      grid_height = length(grid)
      character_speed = (WsClient.get_players(session_id) |> List.first()).character.base_speed

      first_player_before_moving = WsClient.get_players(session_id) |> List.first()
      WsClient.move(1, :down)
      :timer.sleep(1_000)
      first_player_after_moving = WsClient.get_players(session_id) |> List.first()

      x_before_eq_after = first_player_before_moving.position.x == first_player_after_moving.position.x

      end_position_is_occupied_by_wall_or_player =
        Enum.at(grid, first_player_before_moving.position.x + character_speed)
        |> is_wall_or_player?()

      movement =
        first_player_after_moving.position.x ==
          first_player_before_moving.position.x + character_speed

      # If the player hasn't moved (x position is the same before and after the move command),
      # we check for valid cases where this is the expected outcome: when the target position is already occupied by
      # a wall or another player, or when the limit of the board is reached.
      # In the third case, we make the test fail if player hasn't moved and none of these valid cases are true
      # In the fourth case, we check if the player has moved as expected
      success =
        case x_before_eq_after do
          true when end_position_is_occupied_by_wall_or_player -> true
          true when first_player_before_moving.position.x + character_speed > grid_height -> true
          true -> false
          false -> movement
        end

      assert success
    end

    @tag :player
    test "Move left", %{session_id: session_id} do
      grid = WsClient.get_grid(session_id)
      character_speed = (WsClient.get_players(session_id) |> List.first()).character.base_speed

      first_player_before_moving = WsClient.get_players(session_id) |> List.first()
      WsClient.move(1, :left)
      :timer.sleep(1_000)
      first_player_after_moving = WsClient.get_players(session_id) |> List.first()

      y_before_eq_after = first_player_before_moving.position.y == first_player_after_moving.position.y

      end_position_is_occupied_by_wall_or_player =
        Enum.at(grid, first_player_before_moving.position.y - character_speed)
        |> is_wall_or_player?()

      movement =
        first_player_after_moving.position.y ==
          first_player_before_moving.position.y - character_speed

      # If the player hasn't moved (x position is the same before and after the move command),
      # we check for valid cases where this is the expected outcome: when the target position is already occupied by
      # a wall or another player, or when the limit of the board is reached.
      # In the third case, we make the test fail if player hasn't moved and none of these valid cases are true
      # In the fourth case, we check if the player has moved as expected
      success =
        case y_before_eq_after do
          true when end_position_is_occupied_by_wall_or_player -> true
          true when first_player_before_moving.position.y < character_speed -> true
          true -> false
          false -> movement
        end

      assert success
    end

    @tag :player
    test "Move right", %{session_id: session_id} do
      %{width: grid_width} = WsClient.get_board(session_id)
      grid = WsClient.get_grid(session_id)
      character_speed = (WsClient.get_players(session_id) |> List.first()).character.base_speed

      first_player_before_moving = WsClient.get_players(session_id) |> List.first()
      WsClient.move(1, :right)
      :timer.sleep(1_000)
      first_player_after_moving = WsClient.get_players(session_id) |> List.first()

      y_before_eq_after = first_player_before_moving.position.y == first_player_after_moving.position.y

      end_position_is_occupied_by_wall_or_player =
        Enum.at(grid, first_player_before_moving.position.y + character_speed)
        |> is_wall_or_player?()

      movement =
        first_player_after_moving.position.y ==
          first_player_before_moving.position.y + character_speed

      # If the player hasn't moved (x position is the same before and after the move command),
      # we check for valid cases where this is the expected outcome: when the target position is already occupied by
      # a wall or another player, or when the limit of the board is reached.
      # In the third case, we make the test fail if player hasn't moved and none of these valid cases are true
      # In the fourth case, we check if the player has moved as expected
      success =
        case y_before_eq_after do
          true when end_position_is_occupied_by_wall_or_player -> true
          true when first_player_before_moving.position.y + character_speed > grid_width -> true
          true -> false
          false -> movement
        end

      assert success
    end
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

  defp is_wall_or_player?({:player, _}), do: true
  defp is_wall_or_player?(:wall), do: true
  defp is_wall_or_player?(_), do: false

  defp create_session(config) do
    {:ok, session_id} = DarkWorldsServer.Engine.start_child(%{players: [1], game_config: config})
    DarkWorldsServer.Communication.pid_to_external_id(session_id)
  end

  defp ws_connect(session_id, player_id) do
    WsClient.start_link("ws://localhost:4002/play/#{session_id}/#{player_id}")
  end
end
