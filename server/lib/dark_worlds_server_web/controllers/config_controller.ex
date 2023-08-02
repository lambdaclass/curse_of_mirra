defmodule DarkWorldsServerWeb.ConfigController do
  use DarkWorldsServerWeb, :controller

  def index(conn, _params) do
    render(conn, :index)
  end

  def save_characters(conn, params) do
    list = params |> Map.drop(["_csrf_token"]) |> Map.values() |> Enum.sort_by(& &1["Id"])
    write_config(%{"Items" => list})
    render(conn, :index)
  end

  defp write_config(config_map) do
    case Jason.encode(config_map, pretty: true) do
      {:ok, json} ->
        File.write(Path.absname("../client/Assets/StreamingAssets/Characters.json"), json)

      error ->
        error
    end
  end
end
