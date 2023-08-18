defmodule Utils.Config do
  @config_path "./priv/config/"

  @spec read_config(atom) :: map | {:error, term}
  def read_config(type) do
    with path <- get_config_type_filepath(type),
         {:ok, body} <- File.read(path),
         body <- remove_bom(body),
         {:ok, json} <- Jason.decode(body) do
      json
    else
      {:error, reason} ->
        {:error, reason}
    end
  end

  @spec read_config_for_client(atom) :: map | {:error, term}
  def read_config_for_client(type) do
    case read_config(type) do
      {:error, reason} -> {:error, reason}
      json -> keys_to_atom(json)
    end
  end

  @spec write_config(list, atom) :: :ok | {:error, term}
  def write_config(config_list, type) do
    case Jason.encode(config_list, pretty: true) do
      {:ok, json} ->
        type
        |> get_config_type_filepath
        |> File.write(json)

      error ->
        error
    end
  end

  defp get_config_type_filepath(:characters), do: Path.absname(@config_path <> "Characters.json")
  defp get_config_type_filepath(:game_settings), do: Path.absname(@config_path <> "GameSettings.json")
  defp get_config_type_filepath(:skills), do: Path.absname(@config_path <> "Skills.json")

  defp remove_bom(str), do: String.replace_prefix(str, "\uFEFF", "")

  defp keys_to_atom(map_list) when is_list(map_list) do
    Enum.map(map_list, &keys_to_atom/1)
  end

  defp keys_to_atom(map) when is_map(map) do
    Map.new(map, fn
      map when is_map(map) -> keys_to_atom(map)
      {k, v} -> {String.to_atom(k), v}
    end)
  end
end
