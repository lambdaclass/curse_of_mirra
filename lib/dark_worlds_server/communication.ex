defmodule DarkWorldsServer.Communication do
  alias DarkWorldsServer.Communication.Proto.UpdatePing
  alias DarkWorldsServer.Communication.Proto.GameStateUpdate
  alias DarkWorldsServer.Communication.Proto.ClientAction

  @doc """
  The Communication context
  """

  def encode!(%{players: players}) do
    %GameStateUpdate{players: players}
    |> GameStateUpdate.encode()
  end

  def encode!({player_id, latency}) do
    %UpdatePing{player_id: player_id, latency: latency}
    |> UpdatePing.encode()
  end

  def decode(value) do
    try do
      {:ok, ClientAction.decode(value)}
    rescue
      Protobuf.DecodeError -> {:error, :error_decoding}
    end
  end

  def pid_to_external_id(pid) when is_pid(pid) do
    pid |> :erlang.term_to_binary() |> Base58.encode()
  end

  def external_id_to_pid(external_id) do
    external_id |> Base58.decode() |> :erlang.binary_to_term([:safe])
  end

  def pubsub_game_topic(game_pid) when is_pid(game_pid) do
    "game_play_#{pid_to_external_id(game_pid)}"
  end
end
