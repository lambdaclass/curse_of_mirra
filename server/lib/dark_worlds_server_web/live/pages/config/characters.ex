defmodule DarkWorldsServerWeb.ConfigLive.Characters do
  use DarkWorldsServerWeb, :live_view

  def mount(params, _session, socket) do
    {:ok, config} = read_config()
    config = config |> Map.get("Items") |> hd() |> IO.inspect()
    {:ok, assign(socket, :config, config)}
  end

  defp read_config() do
    with path <- Path.absname("../client/Assets/StreamingAssets/Characters.json"),
         {:ok, body} <- File.read(path),
         {:ok, json} <- Jason.decode(body) do
      {:ok, json}
    else
      {:error, reason} -> {:error, reason}
    end
  end
end
