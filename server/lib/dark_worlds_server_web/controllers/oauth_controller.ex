defmodule DarkWorldsServerWeb.OauthController do
  import Plug.Conn

  use DarkWorldsServerWeb, :controller
  plug Ueberauth

  def callback(
        %{
          assigns: %{
            ueberauth_auth: %{} = _oauth_map
          }
        } = conn,
        %{
          "provider" => "google"
        }
      ) do
    conn
  end
end
