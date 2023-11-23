defmodule LoadTestWeb.Plugs.RequireAuth do
  import Plug.Conn
  import Phoenix.Controller

  alias LoadTestWeb.Router.Helpers, as: Routes

  def init(_params) do
  end

  def call(conn, _params) do
    if conn.assigns[:user_id] do
      conn
    else
      conn
      |> put_flash(:error, "You are not authorized to access requested page.")
      |> redirect(to: Routes.page_path(conn, :home))
      |> halt()
    end
  end
end
