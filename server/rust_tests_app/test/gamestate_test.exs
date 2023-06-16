defmodule GameStateTesting do
  use ExUnit.Case

  test "No move if beyond boundaries" do
    assert {:ok, ""} = TestNIFs.no_move_if_beyond_boundaries()
  end

  # test "No move if occupied" do
  #   # assert {:ok, ""} = TestNIFs.no_move_if_occupied()
  # end

  # test "No move if wall" do
  #   assert {:ok, ""} = TestNIFs.no_move_if_wall()
  # end

  test "Attacking" do
    assert {:ok, ""} = TestNIFs.attacking()
  end

  test "Movement" do
    assert {:ok, ""} = TestNIFs.movement()
  end

  test "Cant move if petrified" do
    assert {:ok, ""} = TestNIFs.cant_move_if_petrified()
  end

  # test "Move player to coordinates" do
  #   assert {:ok, ""} = TestNIFs.move_player_to_coordinates()
  # end
end
