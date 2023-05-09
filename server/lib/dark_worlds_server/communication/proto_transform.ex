defmodule DarkWorldsServer.Communication.ProtoTransform do
  alias DarkWorldsServer.Engine.Position, as: EnginePosition
  alias DarkWorldsServer.Engine.Player, as: EnginePlayer
  alias DarkWorldsServer.Engine.ActionOk, as: EngineAction
  alias DarkWorldsServer.Communication.Proto.Position, as: ProtoPosition
  alias DarkWorldsServer.Communication.Proto.Player, as: ProtoPlayer
  alias DarkWorldsServer.Communication.Proto.GameStateUpdate
  alias DarkWorldsServer.Communication.Proto.ClientAction, as: ProtoAction
  alias DarkWorldsServer.Communication.Proto.UpdatePing

  @behaviour Protobuf.TransformModule

  @impl Protobuf.TransformModule
  def encode(%EnginePosition{} = position, ProtoPosition) do
    %{x: x, y: y} = position
    %ProtoPosition{x: x, y: y}
  end

  def encode(%EnginePlayer{} = player, ProtoPlayer) do
    %{id: id, health: health, position: position} = player
    %ProtoPlayer{id: id, health: health, position: position}
  end

  def encode(%{players: players}, GameStateUpdate) do
    %GameStateUpdate{players: players}
  end

  def encode({player_id, latency}, UpdatePing) do
    %UpdatePing{player_id: player_id, latency: latency}
  end

  def encode(%EngineAction{action: :move, value: direction}, ProtoAction) do
    %ProtoAction{action: :MOVE, direction: direction_encode(direction)}
  end

  def encode(%EngineAction{action: :attack, value: direction}, ProtoAction) do
    %ProtoAction{action: :ATTACK, direction: direction_encode(direction)}
  end

  def encode(%EngineAction{action: :ping}, ProtoAction) do
    %ProtoAction{action: :PING}
  end

  def encode(%EngineAction{action: :update_ping, value: latency}, ProtoAction) do
    %ProtoAction{action: :UPDATE_PING, latency: latency}
  end
  @impl Protobuf.TransformModule
  def decode(%ProtoPosition{} = position, ProtoPosition) do
    %{x: x, y: y} = position
    %EnginePosition{x: x, y: y}
  end

  def decode(%ProtoPlayer{} = player, ProtoPlayer) do
    %{id: id, health: health, position: position} = player
    %EnginePlayer{id: id, health: health, position: position}
  end

  def decode(%GameStateUpdate{players: players}, GameStateUpdate) do
    %{players: players}
  end

  def decode(%UpdatePing{player_id: player_id, latency: latency}, UpdatePing) do
    {player_id, latency}
  end

  def decode(%ProtoAction{action: :MOVE, direction: direction}, ProtoAction) do
    %EngineAction{action: :move, value: direction_decode(direction)}
  end

  def decode(%ProtoAction{action: :ATTACK, direction: direction}, ProtoAction) do
    %EngineAction{action: :attack, value: direction_decode(direction)}
  end

  def decode(%ProtoAction{action: :PING}, ProtoAction) do
    %EngineAction{action: :ping, value: :ping}
  end

  def decode(%ProtoAction{action: :UPDATE_PING, latency: latency}, ProtoAction) do
    %EngineAction{action: :update_ping, value: latency}
  end

  ###############################
  # Helpers for transformations #
  ###############################
  defp direction_encode(:up), do: :UP
  defp direction_encode(:down), do: :DOWN
  defp direction_encode(:left), do: :LEFT
  defp direction_encode(:right), do: :RIGHT

  defp direction_decode(:UP), do: :up
  defp direction_decode(:DOWN), do: :down
  defp direction_decode(:LEFT), do: :left
  defp direction_decode(:RIGHT), do: :right
end
