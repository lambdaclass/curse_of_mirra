defmodule DarkWorldsServer.ProtoBufTest.Player do
  use DarkWorldsServer.DataCase
  use ExUnit.Case, async: true

  alias DarkWorldsServer.Communication.Proto.Player, as: ProtoPlayer
  alias DarkWorldsServer.Engine.Player
  alias DarkWorldsServer.Engine.Position
  alias DarkWorldsServer.Engine.RelativePosition

  describe "Player encoding and decoding" do
    test "Health below 0 is encodable" do
      [status] = Enum.take_random([:ALIVE, :DEAD], 1)

      player = %ProtoPlayer{
        id: 1,
        health: -(2 ** 63 - 1),
        position: %Position{x: 1, y: 2},
        status: status,
        action: :NOTHING,
        aoe_position: %Position{x: 1, y: 1},
        kill_count: 0,
        death_count: 0,
        basic_skill_cooldown_left: %{high: 0, low: 0, __unknown_fields__: []},
        skill_1_cooldown_left: %{high: 0, low: 0, __unknown_fields__: []},
        skill_2_cooldown_left: %{high: 0, low: 0, __unknown_fields__: []},
        skill_3_cooldown_left: %{high: 0, low: 0, __unknown_fields__: []},
        skill_4_cooldown_left: %{high: 0, low: 0, __unknown_fields__: []},
        character_name: "Name",
        effects: %{},
        direction: %RelativePosition{x: 1, y: 1},
        body_size: 100
      }

      expected = %Player{
        id: 1,
        health: -(2 ** 63 - 1),
        position: %Position{x: 1, y: 2},
        status: lower(status),
        action: :nothing,
        aoe_position: %Position{x: 1, y: 1},
        kill_count: 0,
        death_count: 0,
        basic_skill_cooldown_left: %{high: 0, low: 0, __unknown_fields__: []},
        skill_1_cooldown_left: %{high: 0, low: 0, __unknown_fields__: []},
        skill_2_cooldown_left: %{high: 0, low: 0, __unknown_fields__: []},
        skill_3_cooldown_left: %{high: 0, low: 0, __unknown_fields__: []},
        skill_4_cooldown_left: %{high: 0, low: 0, __unknown_fields__: []},
        character_name: "Name",
        effects: %{},
        direction: %RelativePosition{x: 1, y: 1},
        body_size: 100
      }

      decoded =
        player
        |> ProtoPlayer.encode()
        |> ProtoPlayer.decode()

      assert decoded == expected
    end
  end

  defp lower(:DEAD), do: :dead
  defp lower(:ALIVE), do: :alive
end
