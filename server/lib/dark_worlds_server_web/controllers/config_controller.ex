defmodule DarkWorldsServerWeb.ConfigController do
  use DarkWorldsServerWeb, :controller

  def index(conn, _params) do
    render(conn, :index)
  end

  def save_game_settings(conn, params) do
    config = params |> Map.drop(["_csrf_token"])
    Utils.Config.write_config(config, :game_settings)
    redirect(conn, to: ~p"/config/game_settings")
  end

  def save_skills(conn, params) do
    config_list = params |> Map.drop(["_csrf_token"]) |> Map.values() |> Enum.sort_by(& &1["Id"])
    Utils.Config.write_config(config_list, :skills)
    redirect(conn, to: ~p"/config/skills")
  end

  def save_characters(conn, params) do
    config_list = params |> Map.drop(["_csrf_token"]) |> Map.values() |> Enum.sort_by(& &1["Id"])
    Utils.Config.write_config(config_list, :characters)
    redirect(conn, to: ~p"/config/characters")
  end
end
