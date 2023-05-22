defmodule DarkWorldsServer.AccountsFixtures do
  @moduledoc """
  This module defines test helpers for creating
  entities via the `DarkWorldsServer.Accounts` context.
  """
  alias DarkWorldsServer.Accounts.User
  alias DarkWorldsServer.Repo

  def unique_user_email(), do: "user#{System.unique_integer()}@example.com"
  def valid_user_password(), do: "hello world!"

  def valid_user_attributes(attrs) do
    Enum.into(attrs, %{
      email: unique_user_email(),
      password: valid_user_password(),
      username: get_new_username()
    })
  end

  def user_fixture(attrs \\ %{}) do
    {:ok, user} =
      attrs
      |> valid_user_attributes()
      |> DarkWorldsServer.Accounts.register_user()

    user
  end

  def extract_user_token(fun) do
    {:ok, captured_email} = fun.(&"[TOKEN]#{&1}[TOKEN]")
    [_, token | _] = String.split(captured_email.text_body, "[TOKEN]")
    token
  end

  defp get_new_username() do
    amount_of_users = Repo.aggregate(User, :count)
    "user_no_#{amount_of_users}"
  end
end
