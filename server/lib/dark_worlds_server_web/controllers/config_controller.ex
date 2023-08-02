defmodule DarkWorldsServerWeb.ConfigController do
  use DarkWorldsServerWeb, :controller

  def index(conn, _params) do
    render(conn, :index)
  end

  def save_skills(conn, params) do
    list = params |> Map.drop(["_csrf_token"]) |> Map.values() |> Enum.sort_by(& &1["Id"])
    Utils.Config.write_config(list, :skills)
    redirect(conn, to: ~p"/config/skills")
  end

  def save_characters(conn, params) do
    list = params |> Map.drop(["_csrf_token"]) |> Map.values() |> Enum.sort_by(& &1["Id"])
    Utils.Config.write_config(list, :characters)
    redirect(conn, to: ~p"/config/characters")
  end
end
