defmodule Utils.Config do
  @spec read_config(atom) :: map | {:error, term}
  def read_config(type) do
    with path <- get_config_type_filepath(type),
         {:ok, body} <- File.read(path),
         {:ok, json} <- Jason.decode(body) do
      json["Items"]
      |> Map.new(&{&1["Name"], &1})
    else
      {:error, reason} -> {:error, reason}
    end
  end

  @spec write_config(map, atom) :: :ok | {:error, term}
  def write_config(config_map, type) do
    config_map = %{"Items" => config_map}

    case Jason.encode(config_map, pretty: true) do
      {:ok, json} ->
        type
        |> get_config_type_filepath
        |> File.write(json)

      error ->
        error
    end
  end

  defp get_config_type_filepath(:characters), do: Path.absname("../client/Assets/StreamingAssets/Characters.json")
end
