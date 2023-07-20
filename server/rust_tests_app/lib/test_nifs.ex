defmodule TestNIFs do
  use Rustler, otp_app: :rust_tests, crate: "rustler_test", default_features: false
  def err(), do: :erlang.nif_error(:nif_not_loaded)
  def no_move_if_beyond_boundaries(), do: err()
  def attacking(), do: err()
  def cant_move_if_petrified(), do: err()
  def cant_attack_if_disarmed(), do: err()
end
