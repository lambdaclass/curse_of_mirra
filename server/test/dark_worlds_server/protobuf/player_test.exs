defmodule DarkWorldsServer.ProtoBufTest.Player do
  use DarkWorldsServer.DataCase
  use ExUnit.Case, async: true

  import DarkWorldsServer.AccountsFixtures

  alias DarkWorldsServer.Accounts
  alias DarkWorldsServer.Communication.Proto.Player, as: ProtoPlayer
  alias DarkWorldsServer.Engine.Player
  alias DarkWorldsServer.Engine.Position

  describe "Player encoding and decoding" do
    test "Health below 0 is encodable" do
      now = DateTime.utc_now() |> DateTime.to_unix()
      [status] = Enum.take_random([:ALIVE, :DEAD], 1)

      player = %ProtoPlayer{
        id: 1,
        health: -(2 ** 63 - 1),
        position: %Position{x: 1, y: 2},
        last_melee_attack: now,
        status: status,
        action: :NOTHING
      }

      expected = %Player{
        id: 1,
        health: -(2 ** 63 - 1),
        position: %Position{x: 1, y: 2},
        last_melee_attack: now,
        status: status,
        action: :nothing
      }

      decoded =
        player
        |> ProtoPlayer.encode()
        |> ProtoPlayer.decode()

      assert decoded == expected
    end
  end
end
