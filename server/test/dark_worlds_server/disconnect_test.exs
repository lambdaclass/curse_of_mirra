defmodule DarkWorldsServer.Test.Disconnect do
  use ExUnit.Case, async: true
  alias DarkWorldsServer.Engine.ActionOk
  alias DarkWorldsServer.Engine.Runner

  setup do
    {:ok, pid} =
      Runner.start_link(%{
        players: [1, 2, 3],
        game_config: %{
          runner_config: %{
            board_width: 1000,
            board_height: 1000,
            server_tickrate_ms: 30,
            game_timeout_ms: 1_200_000,
            map_shrink_wait_ms: 60_000,
            map_shrink_interval: 100,
            out_of_area_damage: 10
          },
          character_config: DarkWorldsServer.Test.characters_config(),
          skills_config: DarkWorldsServer.Test.skills_config()
        }
      })

    for i <- 1..3 do
      Runner.play(pid, i, %ActionOk{
        action: :select_character,
        value: %{player_id: i, character_name: "Muflus"},
        timestamp: nil
      })
    end

    ## Needed for the character selection
    Process.sleep(1_000)

    for i <- 1..3, do: Runner.join(pid, "client-id", i)
    %{pid: pid}
  end

  describe "Disconnect" do
    @tag :disconnect
    test "Disconnecting every player should stop the runner", %{pid: pid} do
      Process.monitor(pid)
      for id <- 1..3, do: Runner.disconnect(pid, id)
      assert_receive {:DOWN, _, _, ^pid, _}, 5_000
      assert not Process.alive?(pid)
    end

    @tag :disconnect
    test "Leaving one player does not stop the runner", %{pid: pid} do
      Process.monitor(pid)
      for id <- 1..2, do: Runner.disconnect(pid, id)
      # Sleep is not ideal.
      # Maybe there's a better way to do this?
      Process.sleep(5_000)
      assert Process.alive?(pid)
    end
  end
end
