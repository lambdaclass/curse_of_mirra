defmodule DarkWorldsServerWeb.ConfigController do
  use DarkWorldsServerWeb, :controller

  def config(conn, _params) do
    {:ok, config} = read_config()
    config = config |> Map.get("Items") |> hd() |> IO.inspect()
    render(assign(conn, :config, config), :config)
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
