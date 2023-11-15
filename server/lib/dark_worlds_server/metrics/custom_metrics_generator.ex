defmodule DarkWorldsServer.Metrics.CustomMetricsGenerator do
  alias DarkWorldsServer.Engine.RequestTracker
  use GenServer
  require Logger

  @send_metric_interval_ms 30_000 # 30s

  #######
  # API #
  #######
  def start_link(_args) do
    GenServer.start_link(__MODULE__, [], name: __MODULE__)
  end

  #######################
  # GenServer callbacks #
  #######################
  if Mix.env() == :prod do
    @impl true
    def init(_args) do
      Process.send_after(self(), :send_metrics, @send_metric_interval_ms)
      {:ok, %{}}
    end
  else
    @impl true
    def init(_args) do
      {:ok, %{}}
    end
  end

  @impl true
  def handle_info(:send_metrics, state) do
    Process.send_after(self(), :send_metrics, @send_metric_interval_ms)

    report_metrics_to_new_relic()

    {:noreply, state}
  end

  def handle_info(msg, state) do
    Logger.error(%{message: "Unexpected message", unexpected_message: msg})
    {:noreply, state}
  end

  ####################
  # Internal helpers #
  ####################

  defp report_metrics_to_new_relic() do
    aggregate = RequestTracker.aggregate_table()
    current_games = DarkWorldsServer.Engine.list_runners_pids()
    total_games = current_games |> Enum.count()

    {total_messages, total_players} = aggregate.msgs_per_game
    |> Enum.filter(fn {game_pid, _} -> game_pid in current_games end)
    |> Enum.reduce({0, 0}, fn {_, %{total: total, msgs_per_player: msgs_per_player}}, {msg_count, player_count} ->
      {msg_count + total, player_count + length(Map.keys(msgs_per_player))}
    end)

    NewRelic.report_custom_metric("CurrentGamesCount", total_games)
    NewRelic.report_custom_metric("CurrentPlayersCount", total_players)
    NewRelic.report_custom_metric("CurrentMessagesCount", total_messages)
  end

end
