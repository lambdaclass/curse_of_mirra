defmodule DarkWorldsServer.Repo.Migrations.AddGoogleTokenAndMakePasswordNullableForUser do
  use Ecto.Migration

  def change do
    alter table(:users) do
      add :google_token, :string
      modify :hashed_password, :string, null: true
    end
  end
end
