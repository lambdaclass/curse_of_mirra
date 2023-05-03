defmodule DarkWorldsServer.Communication.ProtoTransform do
  alias DarkWorldsServer.Engine.Position, as: EnginePosition
  alias DarkWorldsServer.Engine.Player, as: EnginePlayer
  alias DarkWorldsServer.Communication.Proto.Position, as: ProtoPosition
  alias DarkWorldsServer.Communication.Proto.Player, as: ProtoPlayer
  alias DarkWorldsServer.Communication.Proto.GameStateUpdate

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
end
