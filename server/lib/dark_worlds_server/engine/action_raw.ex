defmodule DarkWorldsServer.Engine.ActionRaw do
  use DarkWorldsServer.Communication.Encoder

  @enforce_keys [:action, :value]
  defstruct [:action, :value]

  alias DarkWorldsServer.Communication
  alias DarkWorldsServer.Engine.Position

  def from_json(json) do
    case Communication.decode(json) do
      {:ok, data} ->
        {:ok,
         %__MODULE__{
           action: data["action"] |> encode_action(),
           value: data["value"] |> encode_value()
         }}

      {:error, error} ->
        {:error, error}
    end
  end

  def encode_action("move"), do: {:ok, :move}
  def encode_action("attack"), do: {:ok, :attack}
  def encode_action("attack_aoe"), do: {:ok, :attack_aoe}
  def encode_action("basic_attack"), do: {:ok, :basic_attack}
  def encode_action(_other), do: {:error, :invalid}

  def encode_value("up"), do: {:ok, :up}
  def encode_value("down"), do: {:ok, :down}
  def encode_value("left"), do: {:ok, :left}
  def encode_value("right"), do: {:ok, :right}
  def encode_value(%{"x" => x, "y" => y}), do: {:ok, %Position{x: x, y: y}}
  def encode_value(value) when is_integer(value), do: {:ok, value}
  def encode_value(_other), do: {:error, :invalid}
end
