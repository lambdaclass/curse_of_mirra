defmodule DarkWorldsServer.Test do
  @config_folder Application.compile_env!(:dark_worlds_server, :config_folder)
  def characters_config() do
    @config_folder
    |> then(fn folder -> folder <> "Characters.json" end)
    |> then(&File.read!/1)
    |> Jason.decode!(keys: :atoms)
  end

  def testing_config() do
    runner_config = %{
      board_width: 1000,
      board_height: 1000,
      server_tickrate_ms: 30,
      game_timeout_ms: 1_200_000
    }

    character_config = characters_config()
    {runner_config, character_config}
  end

  def game_config() do
    {runner_config, character_config} = testing_config()
    game_config(runner_config, character_config)
  end

  def game_config(runner_config, character_config) do
    %{
      game_config: %{
        runner_config: runner_config,
        character_config: character_config
      }
    }
  end
end
