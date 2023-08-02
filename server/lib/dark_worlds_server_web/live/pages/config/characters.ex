defmodule DarkWorldsServerWeb.ConfigLive.Characters do
  use DarkWorldsServerWeb, :live_view

  def mount(_params, _session, socket) do
    config = read_config()
    {:ok, assign(socket, config: to_form(config), characters: Map.keys(config))}
  end

  defp read_config() do
    with path <- Path.absname("../client/Assets/StreamingAssets/Characters.json"),
         {:ok, body} <- File.read(path),
         {:ok, json} <- Jason.decode(body) do
      {:ok, json}
      json["Items"] |> Map.new(fn v -> {v["Name"], v} end)
    else
      {:error, reason} -> {:error, reason}
    end
  end
end
