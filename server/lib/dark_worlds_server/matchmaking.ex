defmodule DarkWorldsServer.Matchmaking do
  @moduledoc """
  The Matchmaking context
  """

  alias DarkWorldsServer.Matchmaking.MatchingSession
  alias DarkWorldsServer.Matchmaking.MatchingSupervisor

  def create_session() do
    {:ok, pid} = MatchingSupervisor.start_child()
    pid
  end

  def add_player(player_id, player_name, session_pid) do
    MatchingSession.add_player(player_id, player_name, session_pid)
  end

  def remove_player(player_id, session_pid) do
    MatchingSession.remove_player(player_id, session_pid)
  end

  def next_id(session_pid) do
    case MatchingSession.list_players(session_pid) do
      [] ->
        1

      ids ->
        expected = MapSet.new(1..length(ids))
        actual = MapSet.new(ids)

        missing_ids =
          MapSet.difference(expected, actual)
          |> MapSet.to_list()

        case missing_ids do
          [] -> length(ids) + 1
          [missing_id | _] -> missing_id
        end
    end
  end

  def fetch_amount_of_players(session_pid) do
    MatchingSession.fetch_amount_of_players(session_pid)
  end

  def fetch_sessions() do
    MatchingSupervisor.children_pids()
  end

  def session_topic(session_id) do
    "matching_session_#{session_id}"
  end

  def start_game(game_config, session_pid) do
    MatchingSession.start_game(game_config, session_pid)
  end
end
