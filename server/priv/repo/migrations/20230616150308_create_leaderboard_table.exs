defmodule DarkWorldsServer.Repo.Migrations.CreateLeaderboardTable do
  use Ecto.Migration

  def change do
    create table(:leaderboard) do
      add :kills, :integer
      add :deaths, :integer
      add :lobby_id, :string
      add :user_id, references(:users, on_delete: :nothing), null: false
      
      timestamps()
    end
  end
end
