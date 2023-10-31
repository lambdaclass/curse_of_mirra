defmodule DarkWorldsServer.Repo.Migrations.AddNewFieldsToUsersSchema do
  use Ecto.Migration

  def change do
    alter table(:users) do
      add :selected_character, :string, default: "h4ck"
      add :device_client_id, :string
      add :total_kills, :integer, default: 0
      add :total_wins, :integer, default: 0
      add :most_used_character, :string, default: "h4ck"
      add :experience, :float, default: 0
    end

    create unique_index(:users, :device_client_id)
  end
end
