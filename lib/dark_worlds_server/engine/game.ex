defmodule DarkWorldsServer.Engine.Game do
  use Rustler, otp_app: :dark_worlds_server, crate: "gamestate"

  @enforce_keys [:players, :board]
  defstruct [:players, :board]

  def new(number_of_players: number_of_players, board: {width, height}) do
    new_game(number_of_players, width, height)
  end

  def new_game(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def move_player(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def attack_player(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def attack_aoe(_a, _b, _c, _d), do: :erlang.nif_error(:nif_not_loaded)
end
