defmodule DarkWorldsServer.Test.Disconnect do
  use ExUnit.Case, async: true
  alias DarkWorldsServer.Engine.Runner

  setup do
    {:ok, pid} = Runner.start_link(%{players: []})
    for i <- 1..3, do: Runner.join(pid, i)
    %{pid: pid}
  end

  describe "Disconnect" do
    test "Disconnecting every player should stop the runner", %{pid: pid} do
      Process.monitor(pid)
      for id <- 1..3, do: Runner.disconnect(pid, id)
      assert_receive {:DOWN, _, _, ^pid, _}, 5_000
      assert not Process.alive?(pid)
    end

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
