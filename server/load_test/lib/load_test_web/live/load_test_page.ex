defmodule LoadTestWeb.Live.LoadTestPage do
  @moduledoc false
  use Phoenix.LiveDashboard.PageBuilder

  @impl true
  def menu_link(_, _) do
    {:ok, "Load Test Page"}
  end

  @impl true
  def render(assigns) do
    ~H"""
    Hola
    """
  end
end
