defmodule DarkWorldsServerWeb.ConfigLive.Characters do
  use DarkWorldsServerWeb, :live_view
  @configType :characters

  def mount(_params, _session, socket) do
    config = Utils.Config.read_config(@configType)
    {:ok, assign(socket, config: to_form(config), characters: Map.keys(config))}
  end
end
