defmodule DarkWorldsServerWeb.SessionControllerTest do
  use DarkWorldsServerWeb.ConnCase, async: true

  alias Plug.Conn

  describe "GET /new_lobby" do
    @tag :session
    test "Session is created succesfully", %{conn: conn} do
      conn = Conn.put_req_header(conn, "content-type", "application/json")
      conn = get(conn, ~p"/new_lobby", %{})

      response = json_response(conn, 200)
      assert Map.has_key?(response, "lobby_id")
    end
  end
end
