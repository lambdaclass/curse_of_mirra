defmodule DarkWorldsServer.Engine.PlayerTracker do
  @table :player_tracker

  def create_table() do
    :ets.new(@table, [:set, :public, :named_table])
  end

  def add_player_game(player_id, game_pid) do
    true = :ets.insert(@table, {player_id, game_pid})
  end

  def get_player_game(player_id) do
    with [{^player_id, game_pid}] <- :ets.lookup(@table, player_id),
         true <- Process.alive?(game_pid) do
      game_pid
    else
      _ -> nil
    end
  end
end
