defmodule DarkWorldsServer.Engine.BotPlayerTest do
  use ExUnit.Case, async: true
  alias DarkWorldsServer.Engine.{BotPlayer, ActionOk, Runner}

  setup do
    {:ok, pid} =
      Runner.start_link(%{
        players: [1, 2, 3, 4],
        game_config: %{
          runner_config: %{
            board_width: 0,
            board_height: 0,
            server_tickrate_ms: 30,
            game_timeout_ms: 1_200_000,
            map_shrink_wait_ms: 0,
            map_shrink_interval: 0,
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
    {:ok, bot_player_pid} =
      BotPlayer.start_link(pid, 30)
      # Add a bot to the server
      Runner.play(pid, 4, %ActionOk{action: :add_bot, timestamp: nil, value: nil})


      ## Needed for the character selection
      Process.sleep(1_000)

      for i <- 1..4, do: Runner.join(pid, "client-id", i)

      %{runner_pid: pid, bot_player_pid: bot_player_pid}
  end

  describe "bot decisions" do
    test "Bot flees when in harm",%{runner_pid: _pid, bot_player_pid: _bot_player_pid} do
      true
    end
  end
end
