defmodule DarkWorldsServer.Repo.Migrations.DeleteUserConstraintOnLeaderboardUserId do
  use Ecto.Migration

  def change do
    alter table(:leaderboard) do
        remove :user_id, references(DarkWorldsServer.Accounts.User)
        add :user_id, :integer
    end
  end
end
