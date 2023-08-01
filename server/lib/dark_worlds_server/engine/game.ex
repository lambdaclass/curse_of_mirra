defmodule DarkWorldsServer.Engine.Game do
  use Rustler, otp_app: :dark_worlds_server, crate: "gamestate", default_features: true
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [:players, :board]
  defstruct [:players, :board]

  def new(%{
        selected_players: selected_players,
        number_of_players: number_of_players,
        board: {width, height},
        build_walls: build_walls,
        characters: character_info,
        skills: skills_info
      })
      when is_list(character_info) do
    new_game(selected_players, number_of_players, width, height, build_walls, character_info, skills_info)
  end

  def new_game(
        _selected_players,
        _num_of_players,
        _width,
        _height,
        _build_walls,
        _characters_config_list,
        _skills_config_list
      ),
      do: :erlang.nif_error(:nif_not_loaded)

  def move_player(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)

  def move_player_to_relative_position(_game_state, _player_id, _relative_position),
    do: :erlang.nif_error(:nif_not_loaded)

  def move_with_joystick(_game_state, _player_id, _x, _y), do: :erlang.nif_error(:nif_not_loaded)
  def auto_attack(_game_state, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def attack_player(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def skill_1(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def skill_2(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def skill_3(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def skill_4(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def basic_attack(_a, _b, _c), do: :erlang.nif_error(:nif_not_loaded)
  def world_tick(_game_state, _out_of_area_damage), do: :erlang.nif_error(:nif_not_loaded)
  def disconnect(_game, _id), do: :erlang.nif_error(:nif_not_loaded)
  def spawn_player(_game, _player_id), do: :erlang.nif_error(:nif_not_loaded)
  def shrink_map(_game), do: :erlang.nif_error(:nif_not_loaded)
end
