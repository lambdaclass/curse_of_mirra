defmodule LoadTest.Repo do
  use Ecto.Repo,
    otp_app: :load_test,
    adapter: Ecto.Adapters.Postgres
end
