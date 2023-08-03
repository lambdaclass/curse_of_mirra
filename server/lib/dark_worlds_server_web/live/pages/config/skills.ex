defmodule DarkWorldsServerWeb.ConfigLive.Skills do
  use DarkWorldsServerWeb, :live_view
  @config_type :skills

  def mount(_params, _session, socket) do
    config = Utils.Config.read_config(@config_type)
    {:ok, assign(socket, config: to_form(config), skills: Map.keys(config))}
  end
end
