defmodule DarkWorldsServer.Metrics.CustomMetricsGenerator do
  alias DarkWorldsServer.Engine.RequestTracker
  use GenServer
  require Logger

  @send_metric_interval_ms 60_000 # 60s

  #######
  # API #
  #######
  def start_link(_args) do
    GenServer.start_link(__MODULE__, [], name: __MODULE__)
  end

  #######################
  # GenServer callbacks #
  #######################
  @impl true
  def init(_args) do
    case Mix.env() do
      :prod ->
        Process.send_after(self(), :send_metrics, @send_metric_interval_ms)

      _ ->
        :nothing
    end

    {:ok, %{}}
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
    total_games = length(Map.keys(aggregate.msgs_per_game))

    NewRelic.report_custom_metric("Test/GamesCount", total_games)
  end

end
