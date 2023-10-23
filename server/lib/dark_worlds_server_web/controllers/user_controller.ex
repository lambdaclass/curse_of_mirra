defmodule DarkWorldsServerWeb.UserController do
  use DarkWorldsServerWeb, :controller
  alias DarkWorldsServer.Accounts

  def current_character(conn, %{"user_id" => user_id}) do
    character_name =
      Accounts.get_user!(user_id)
      |> Map.get(:character_name)

    json(conn, %{character_name: character_name})
  end

  def select_character(conn, %{"character_name" => _character_name, "user_id" => user_id} = attrs) do
    user =
      Accounts.get_user!(user_id)
      |> Accounts.update_user_character_name(attrs)

    json(conn, %{user_id: user.id})
  end
end
