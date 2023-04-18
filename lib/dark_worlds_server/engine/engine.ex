defmodule DarkWorldsServer.Engine do
  @moduledoc """
  Game Engine Supervisor
  """
  use DynamicSupervisor

  alias DarkWorldsServer.Engine.Runner

  def start_link(args) do
    DynamicSupervisor.start_link(__MODULE__, args, name: __MODULE__)
  end

  def start_child() do
    DynamicSupervisor.start_child(__MODULE__, Runner)
  end

  @impl true
  def init(_opts) do
    DynamicSupervisor.init(strategy: :one_for_one)
  end
end
