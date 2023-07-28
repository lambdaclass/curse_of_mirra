defmodule DarkWorldsServerWeb.ConfigController do
  use DarkWorldsServerWeb, :controller

  def index(conn, _params) do
    render(conn, :index)
  end
end
