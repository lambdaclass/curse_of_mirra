defmodule Utils.Config do
  @streamingAssetsPath "../client/Assets/StreamingAssets/"

  @spec read_config(atom) :: map | {:error, term}
  def read_config(type) do
    with path <- get_config_type_filepath(type),
         {:ok, body} <- File.read(path),
         body <- remove_bom(body),
         {:ok, json} <- Jason.decode(body) do
      if type == :game_settings do
        json
      else
        json["Items"]
        |> Map.new(&{&1["Name"], &1})
      end
    else
      {:error, reason} ->
        {:error, reason}
    end
  end

  @spec write_config(map, atom) :: :ok | {:error, term}
  def write_config(config_map, type) do
    IO.inspect(config_map)
    config_map = if type == :game_settings, do: config_map, else: %{"Items" => config_map}

    case Jason.encode(config_map, pretty: true) do
      {:ok, json} ->
        type
        |> get_config_type_filepath
        |> File.write(json)

      error ->
        error
    end
  end

  defp get_config_type_filepath(:characters), do: Path.absname(@streamingAssetsPath <> "Characters.json")
  defp get_config_type_filepath(:game_settings), do: Path.absname(@streamingAssetsPath <> "GameSettings.json")
  defp get_config_type_filepath(:skills), do: Path.absname(@streamingAssetsPath <> "Skills.json")

  defp remove_bom(str), do: String.replace_prefix(str, "\uFEFF", "")
end
