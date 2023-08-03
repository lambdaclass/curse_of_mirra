defmodule DarkWorldsServerWeb.ConfigLive.GameSettings do
  use DarkWorldsServerWeb, :live_view
  @config_type :game_settings

  def mount(_params, _session, socket) do
    config = Utils.Config.read_config(@config_type)
    {:ok, assign(socket, config: to_form(config))}
  end
end
