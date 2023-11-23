defmodule LoadTestWeb.Plugs.SetUser do
  import Plug.Conn
  import Phoenix.Controller

  alias LoadTestWeb.Router.Helpers, as: Routes

  def init(_params) do
  end

  def call(conn, _params) do
    user_id = get_session(conn, :user_id)

    conn
    |> assign(:user_id, user_id)
  end
end
