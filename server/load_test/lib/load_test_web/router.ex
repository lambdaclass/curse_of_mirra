defmodule LoadTestWeb.Router do
  use LoadTestWeb, :router

  pipeline :browser do
    plug :accepts, ["html"]
    plug :fetch_session
    plug :fetch_live_flash
    plug :put_root_layout, html: {LoadTestWeb.Layouts, :root}
    plug :protect_from_forgery
    plug :put_secure_browser_headers
    plug LoadTestWeb.Plugs.SetUser
  end

  pipeline :protected do
    plug LoadTestWeb.Plugs.RequireAuth
  end

  pipeline :api do
    plug :accepts, ["html"]
    plug :accepts, ["json"]
  end

  scope "/", LoadTestWeb do
    pipe_through :browser

    get "/", PageController, :home
    get "/auth/google/callback", GoogleAuthController, :index
    get "signout", GoogleAuthController, :signout
  end

  # Other scopes may use custom stacks.
  # scope "/api", LoadTestWeb do
  #   pipe_through :api
  # end

  # If you want to use the LiveDashboard in production, you should put
  # it behind authentication and allow only admins to access it.
  # If your application does not have an admins-only section yet,
  # you can use Plug.BasicAuth to set up some basic authentication
  # as long as you are also using SSL (which you should anyway).
  import Phoenix.LiveDashboard.Router

  scope "/dev" do
    pipe_through :browser
    pipe_through :protected

    live_dashboard "/dashboard", metrics: LoadTestWeb.Telemetry
  end
end
