defmodule DarkWorldsServerWeb.OauthController do
  import Plug.Conn

  use DarkWorldsServerWeb, :controller
  plug Ueberauth

  def callback(
        %{
          assigns: %{
            ueberauth_auth: %{
              info: %{email: user_mail},
              credentials: %{token: google_token}
            }
          }
        } = conn,
        %{
          "provider" => "google"
        }
      ) do
    params = %{
      email: user_mail,
      google_token: google_token
    }

    case DarkWorldsServer.Accounts.create_user(params) do
      {:ok, _user} ->
        json(conn, %{status: :ok, email: user_mail, google_token: google_token})

      {:error, _changeset} ->
        json(conn, %{status: :error, email: user_mail, google_token: google_token})
    end
  end
end
