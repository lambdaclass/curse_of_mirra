defmodule DarkWorldsServer.Engine.BotPlayerTest do
  use ExUnit.Case, async: true
  alias DarkWorldsServer.Engine.BotPlayer

  describe "bot decisions" do
    test "Bot flees when in harm" do
      bot_with_out_of_zone_state = %{
        id: 1,
        effects: %{
          out_of_area: %{}
        }
      }

      state_map = %{
        myrra_state: %{
          players: [
            bot_with_out_of_zone_state
          ]
        }
      }

      assert :flee_from_zone == BotPlayer.decide_objective(state_map, 1)
    end

    test "Pick a random action when idle" do
      bot_state = %{
        id: 1,
        effects: %{}
      }

      state_map = %{
        myrra_state: %{
          players: [
            bot_state
          ]
        }
      }

      result_state = BotPlayer.decide_objective(state_map, 1)
      assert Enum.any?([:attack_enemy, :random_movement], fn s -> s == result_state end)
    end

    test "Dont move when bots are not enabled" do
      bot_state = %{
        id: 1,
        effects: %{}
      }

      state_map = %{
        bots_enabled: false,
        myrra_state: %{
          players: [
            bot_state
          ]
        }
      }

      assert :nothing == BotPlayer.decide_objective(state_map, 1)
    end

    test "Move when bots are not enabled" do
      bot_state = %{
        id: 1,
        effects: %{}
      }

      state_map = %{
        bots_enabled: true,
        myrra_state: %{
          players: [
            bot_state
          ]
        }
      }

      result_state = BotPlayer.decide_objective(state_map, 1)
      assert Enum.any?([:attack_enemy, :random_movement], fn s -> s == result_state end)
    end
  end
end
