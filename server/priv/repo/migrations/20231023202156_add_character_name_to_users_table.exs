defmodule DarkWorldsServer.Repo.Migrations.AddCharacterNameToUsersTable do
  use Ecto.Migration

  def change do
    alter table(:users) do
      add(:character_name, :string)
    end
  end
end
