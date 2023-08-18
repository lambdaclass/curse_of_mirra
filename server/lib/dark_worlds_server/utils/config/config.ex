defmodule Utils.Config do
  @config_path "./priv/config/"

  @spec read_config(atom) :: map | {:error, term}
  def read_config(type) do
    with path <- get_config_type_filepath(type),
         {:ok, body} <- File.read(path),
         body <- remove_bom(body),
         {:ok, json} <- Jason.decode(body) do
      json |> IO.inspect(label: :json)
    else
      {:error, reason} ->
        {:error, reason}
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
end
