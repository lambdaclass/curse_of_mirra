defmodule LoadTestWeb.PageController do
  use LoadTestWeb, :controller

  def home(conn, _params) do
    base_url = LoadTestWeb.Endpoint.url()
    oauth_google_url = ElixirAuthGoogle.generate_oauth_url(base_url)
    render(conn, "home.html",[oauth_google_url: oauth_google_url])
  end

end
