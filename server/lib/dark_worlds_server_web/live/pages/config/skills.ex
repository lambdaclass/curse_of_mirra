defmodule DarkWorldsServerWeb.ConfigLive.Skills do
  use DarkWorldsServerWeb, :live_view
  @configType :skills

  def mount(_params, _session, socket) do
    config = Utils.Config.read_config(@configType)
    {:ok, assign(socket, config: to_form(config), skills: Map.keys(config))}
  end
end
