defmodule DarkWorldsServer.Engine.Game do
  # use Rustler, otp_app: :dark_worlds_server, crate: "gamestate", default_features: true
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [:players, :board]
  defstruct [:players, :board]

  def new(old_config) do
    {:ok, engine_config_json} =
      Application.app_dir(:lambda_game_engine, "priv/config.json")
      |> File.read()

    LambdaGameEngine.new(Map.put(old_config, :engine_config, engine_config_json))
  end

  def move_player(a, b, c), do: LambdaGameEngine.move_player(a, b, c)

  def move_player_to_relative_position(game_state, player_id, relative_position),
    do: LambdaGameEngine.move_player_to_relative_position(game_state, player_id, relative_position)

  def move_with_joystick(game_state, player_id, x, y),
    do: LambdaGameEngine.move_with_joystick(game_state, player_id, x, y)

  def auto_attack(game_state, b, c), do: LambdaGameEngine.auto_attack(game_state, b, c)
  def attack_player(a, b, c), do: LambdaGameEngine.attack_player(a, b, c)
  def skill_1(a, b, c), do: LambdaGameEngine.skill_1(a, b, c)
  def skill_2(a, b, c), do: LambdaGameEngine.skill_2(a, b, c)
  def skill_3(a, b, c), do: LambdaGameEngine.skill_3(a, b, c)
  def skill_4(a, b, c), do: LambdaGameEngine.skill_4(a, b, c)
  def basic_attack(a, b, c), do: LambdaGameEngine.basic_attack(a, b, c)
  def world_tick(game_state, out_of_area_damage), do: LambdaGameEngine.world_tick(game_state, out_of_area_damage)
  def disconnect(game, id), do: LambdaGameEngine.disconnect(game, id)
  def spawn_player(game, player_id), do: LambdaGameEngine.spawn_player(game, player_id)
  def shrink_map(game, map_shrink_minimum_radius), do: LambdaGameEngine.shrink_map(game, map_shrink_minimum_radius)
  def spawn_loot(game), do: LambdaGameEngine.spawn_loot(game)
end
