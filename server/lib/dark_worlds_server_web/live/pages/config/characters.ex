defmodule DarkWorldsServerWeb.ConfigLive.Characters do
  use DarkWorldsServerWeb, :live_view
  @config_type :characters

  def mount(_params, _session, socket) do
    config = Utils.Config.read_config(@config_type)
    {:ok, assign(socket, config: to_form(config), characters: Map.keys(config))}
  end
end
