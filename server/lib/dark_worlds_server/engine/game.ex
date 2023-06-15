defmodule DarkWorldsServer.Engine.Game do
  use Rustler, otp_app: :dark_worlds_server, crate: "gamestate", default_features: true
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [:players, :board]
  defstruct [:players, :board]

  def new(%{number_of_players: number_of_players, board: {width, height}, build_walls: build_walls}) do
    new_game(number_of_players, width, height, build_walls)
  end

  def new_game(_a, _b, _c, _d), do: :erlang.nif_error(:nif_not_loaded)
  def move_player(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def move_with_joystick(_game_state, _player_id, _x, _y), do: :erlang.nif_error(:nif_not_loaded)
  def auto_attack(_game_state, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def attack_player(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def skill_1(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def basic_attack(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def get_grid(_a), do: :erlang.nif_error(:nif_not_loaded)
  def get_non_empty(_a), do: :erlang.nif_error(:nif_not_loaded)
  def world_tick(_game_state), do: :erlang.nif_error(:nif_not_loaded)
  def disconnect(_game, _id), do: :erlang.nif_error(:nif_not_loaded)
  def new_round(_game, _players), do: :erlang.nif_error(:nif_not_loaded)
  def spawn_player(_game, _player_id), do: :erlang.nif_error(:nif_not_loaded)
end
