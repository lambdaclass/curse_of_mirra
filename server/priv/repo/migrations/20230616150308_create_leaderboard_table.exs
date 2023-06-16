defmodule DarkWorldsServer.Repo.Migrations.CreateLeaderboardTable do
  use Ecto.Migration

  def change do
    create table(:leaderboard) do
      add :kills, :integer
      add :deaths, :integer
      add :lobby_id, :string
      # FIXME this should reference the used id bound to the player
      add :user_id, :integer
      
      timestamps()
    end
  end
end
