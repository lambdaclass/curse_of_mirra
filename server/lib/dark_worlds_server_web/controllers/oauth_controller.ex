defmodule DarkWorldsServerWeb.OauthController do
  import Plug.Conn

  use DarkWorldsServerWeb, :controller
  plug Ueberauth

  alias DarkWorldsServerWeb.UserAuth

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

    case DarkWorldsServer.Accounts.get_or_create_user(params) do
      {:ok, user} ->
        conn
        |> put_flash(:info, "WelcomeBack")
        |> UserAuth.log_in_user(user)
        # json(conn, %{status: :ok, email: user_mail, google_token: google_token})

      {:error, _changeset} ->
        json(conn, %{status: :error, email: user_mail, google_token: google_token})
    end
  end
end
