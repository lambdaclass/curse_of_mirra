defmodule DarkWorldsServer.Repo.Migrations.AddCharacterNameToUsersSchema do
  use Ecto.Migration

  def change do
    alter table(:users) do
      add :character_name, :string, default: "h4ck"
    end
  end
end
