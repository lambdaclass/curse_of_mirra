defmodule LoadTest.LobbyPlayer do
  @config_folder "../../client/Assets/StreamingAssets/"
  @doc """
  A socket representing a player inside a lobby
  """
  use WebSockex, restart: :transient
  require Logger
  use Tesla

  alias LoadTest.Communication.Proto.LobbyEvent
  alias LoadTest.Communication.Proto.GameConfig
  alias LoadTest.Communication.Proto.BoardSize
  alias LoadTest.PlayerSupervisor

  # TODO: remove this
  def characters_config() do
    @config_folder
    |> then(fn folder -> folder <> "Characters.json" end)
    |> then(&File.read!/1)
    |> Jason.decode!(keys: :atoms)
  end

  def start_link({player_number, max_duration}) do
    {:ok, response} = get(join_lobby_url())
    %{"lobby_id" => lobby_id} = response.body |> Jason.decode!()

    IO.inspect(label: "JOINING LOBBY WITH ID")

    ws_url = ws_url(lobby_id)

    WebSockex.start_link(ws_url, __MODULE__, %{
      player_number: player_number,
      lobby_id: lobby_id,
      max_duration: max_duration
    })
  end

  def handle_frame({_type, msg}, state) do
    case LobbyEvent.decode(msg) do
      %LobbyEvent{type: :GAME_STARTED, game_id: game_id, game_config: _config, server_hash: _server_hash} ->
        {:ok, pid} =
          PlayerSupervisor.spawn_game_player(state.player_number, game_id, state.max_duration)

        Process.send(pid, :play, [])
        {:close, {1000, ""}, state}

      _ ->
        {:ok, state}
    end
  end

  def handle_cast({:send, {_type, _msg} = frame}, state) do
    # Logger.info("Sending frame with payload: #{msg}")
    {:reply, frame, state}
  end

  defp ws_url(lobby_id) do
    host = PlayerSupervisor.server_host()

    case System.get_env("SSL_ENABLED") do
      "true" ->
        "wss://#{host}/matchmaking/#{lobby_id}"

      _ ->
        "ws://#{host}/matchmaking/#{lobby_id}"
    end
  end

  defp join_lobby_url() do
    host = PlayerSupervisor.server_host()

    case System.get_env("SSL_ENABLED") do
      "true" ->
        "https://#{host}/join_lobby"

      _ ->
        "http://#{host}/join_lobby"
    end
  end
end
