defmodule DarkWorldsServerWeb.UserControllerTest do
  use DarkWorldsServerWeb.ConnCase, async: true

  import DarkWorldsServer.AccountsFixtures

  setup do
    %{user: user_with_character_name_fixture()}
  end

  describe "Character name" do
    test "gets the current character name", %{conn: conn, user: user} do
      conn = get(conn, ~p"/characters/#{user.id}", %{})
      response = json_response(conn, 200)
      assert Map.has_key?(response, "character_name")
    end

    test "select character name", %{conn: conn, user: user} do
      conn = post(conn, ~p"/characters/#{user.id}/select_character", %{"character_name" => "H4ck"})
      response = json_response(conn, 200)
      assert response == user.character_name
    end
  end
end
