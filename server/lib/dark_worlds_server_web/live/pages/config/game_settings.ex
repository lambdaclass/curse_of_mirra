defmodule DarkWorldsServerWeb.ConfigLive.GameSettings do
  use DarkWorldsServerWeb, :live_view
  @configType :game_settings

  def mount(_params, _session, socket) do
    config = Utils.Config.read_config(@configType)
    {:ok, assign(socket, config: to_form(config))}
  end
end
