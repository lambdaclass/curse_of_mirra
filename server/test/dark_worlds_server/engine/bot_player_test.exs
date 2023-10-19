defmodule DarkWorldsServer.Engine.BotPlayerTest do
  use ExUnit.Case, async: true
  alias DarkWorldsServer.Engine.BotPlayer

  describe "bot decisions" do
    test "Bot flees when in harm" do
      bot_with_out_of_zone_state = %{
        effects: %{
          out_of_area: %{}
        }
      }

      assert :flee_from_zone == BotPlayer.decide_state(bot_with_out_of_zone_state, %{})
    end

    test "Pick a random action when idle" do
      bot_in_idle = %{
        effects: %{}
      }

      result_state = BotPlayer.decide_state(bot_in_idle, %{})
      assert Enum.any?([:attack_enemy, :random_movement], fn s -> s == result_state end)
    end

    test "Dont move when bots are not enabled" do
      bot_state = %{
        effects: %{}
      }

      state_with_disabled_bots = %{
        bots_enabled: false
      }

      assert :nothing == BotPlayer.decide_state(bot_state, state_with_disabled_bots)
    end

    test "Move when bots are not enabled" do
      bot_state = %{
        effects: %{}
      }

      state_with_enable_bots = %{
        bots_enabled: true
      }

      result_state = BotPlayer.decide_state(bot_state, state_with_enable_bots)
      assert Enum.any?([:attack_enemy, :random_movement], fn s -> s == result_state end)
    end
  end
end
