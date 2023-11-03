defmodule DarkWorldsServerWeb.MatchmakingLive.Index do
  use DarkWorldsServerWeb, :live_view
  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Matchmaking

  def mount(_params, _session, socket) do
    case connected?(socket) do
      false ->
        {:ok, socket}

      true ->
        Process.send_after(self(), :fetch_sessions, 10_000)
        {:ok, assign(socket, :game_session_ids, Matchmaking.fetch_sessions())}
    end
  end

  def handle_event("create_session", _params, socket) do
    session_pid = Matchmaking.create_session()
    session_id = Communication.pid_to_external_id(session_pid)
    {:noreply, push_navigate(socket, to: ~p"/matchmaking/#{session_id}/player")}
  end

  def handle_info(:fetch_sessions, socket) do
    Process.send_after(self(), :fetch_sessions, 10_000)
    {:noreply, assign(socket, :game_session_ids, Matchmaking.fetch_sessions())}
  end
end
