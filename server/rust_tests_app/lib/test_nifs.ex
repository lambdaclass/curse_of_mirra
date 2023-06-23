defmodule TestNIFs do
  use Rustler, otp_app: :rust_tests, crate: "rustler_test", default_features: false
  def err(), do: :erlang.nif_error(:nif_not_loaded)
  def no_move_if_beyond_boundaries(), do: err()
  def no_move_if_occupied(), do: err()
  def attacking(), do: err()
  def movement(), do: err()
  def cant_move_if_petrified(), do: :erlang.nif_err(:nif_not_loaded)
  def cant_attack_if_disarmed(), do: err()
  def move_player_to_coordinates(), do: err()
end
