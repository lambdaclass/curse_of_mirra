defmodule LoadTestWeb.GoogleAuthController do
  use LoadTestWeb, :controller

  @doc """
    `index/2` handles the callback from Google Auth API redirect.
  """
  def index(conn, %{"code" => code}) do
    {:ok, token} = ElixirAuthGoogle.get_token(code, LoadTestWeb.Endpoint.url())
    {:ok, profile} = ElixirAuthGoogle.get_user_profile(token.access_token)

    IO.inspect(token)
    IO.inspect(profile)

    conn
    |> put_flash(:info, "Welcome #{profile.given_name}!")
    |> put_session(:user_id, profile.email)
    |> redirect(to: "/")
  end 

  def signout(conn, _params) do
    conn
    |> configure_session(drop: true)
    |> redirect(to: "/")
  end
end
