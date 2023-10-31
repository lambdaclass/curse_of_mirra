defmodule DarkWorldsServerWeb.CharacterController do
  use DarkWorldsServerWeb, :controller

  alias DarkWorldsServer.Accounts
  alias DarkWorldsServer.Accounts.User

  @alphabet "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"

  def player_character_name(conn, %{"username" => username}) do
    user = DarkWorldsServer.Accounts.get_user_by_username(username)
    json(conn, user_response(user))
  end

  def set_player_character_name(conn, %{"username" => username, "character_name" => character_name}) do
    user_params = create_user_data(username, character_name)

    case Accounts.register_user(user_params) do
      {:ok, user} ->
        json(conn, user_response(user))

      {:error, _changeset} ->
        json(conn, %{username: "ALREADY_TAKEN", character_name: "ALREADY_TAKEN"})
    end
  end

  def update_player_character_name(conn, %{"username" => username, "character_name" => character_name} = params) do
    json(conn, params)
  end

  def user_response(nil) do
    %{
      username: "NOT_FOUND",
      character_name: "NOT_FOUND"
    }
  end

  def user_response(%User{username: username, character_name: character_name}) do
    %{
      username: username,
      character_name: character_name
    }
  end

  defp create_user_data(username, character_name) do
    provisional_password = generate_provisional_password()
    current_users_count = Accounts.get_users_count()

    %{
      email: "test_#{current_users_count}@mail.com",
      password: provisional_password,
      username: username,
      character_name: character_name
    }
  end

  defp generate_provisional_password do
    @alphabet
    |> String.graphemes()
    |> Enum.take_random(16)
    |> Enum.join("")
  end
end
