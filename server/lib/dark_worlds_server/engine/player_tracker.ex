defmodule DarkWorldsServer.Engine.PlayerTracker do
  @table :player_tracker
  @persistent_term_key {__MODULE__, :runtime_id}

  def create_table() do
    runtime_id = DateTime.utc_now(Calendar.ISO) |> DateTime.to_unix(:millisecond)

    :persistent_term.put(@persistent_term_key, runtime_id)
    :ets.new(@table, [:set, :public, :named_table])
  end

  def add_player_game(player_id, game_pid) do
    runtime_id = :persistent_term.get(@persistent_term_key)
    true = :ets.insert(@table, {to_string(player_id), runtime_id, game_pid})
  end

  def get_player_game(player_id) do
    with runtime_id <- :persistent_term.get(@persistent_term_key),
         [{^player_id, ^runtime_id, game_pid}] <- :ets.lookup(@table, to_string(player_id)),
         true <- Process.alive?(game_pid) do
      game_pid
    else
      _ -> nil
    end
  end
end
