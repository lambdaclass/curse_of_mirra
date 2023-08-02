defmodule DarkWorldsServerWeb.ConfigController do
  use DarkWorldsServerWeb, :controller

  def index(conn, _params) do
    render(conn, :index)
  end

  def save_characters(conn, params) do
    list = params |> Map.drop(["_csrf_token"]) |> Map.values() |> Enum.sort_by(& &1["Id"])
    Utils.Config.write_config(list, :characters)
    render(conn, :index)
  end
end
