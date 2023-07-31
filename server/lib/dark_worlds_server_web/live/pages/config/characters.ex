defmodule DarkWorldsServerWeb.ConfigLive.Characters do
  use DarkWorldsServerWeb, :live_view

  def mount(_params, _session, socket) do
    config = read_config()
    {:ok, assign(socket, :config, config)}
  end

  def handle_event("save", value, socket) do
    {:noreply, socket}
  end

  defp read_config() do
    with path <- Path.absname("../client/Assets/StreamingAssets/Characters.json"),
         {:ok, body} <- File.read(path),
         {:ok, json} <- Jason.decode(body) do
      {:ok, json}
      json["Items"] |> Map.new(fn v -> {v["Id"], v} end)
    else
      {:error, reason} -> {:error, reason}
    end
  end
end
