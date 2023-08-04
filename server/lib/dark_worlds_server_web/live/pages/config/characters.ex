defmodule DarkWorldsServerWeb.ConfigLive.Characters do
  use DarkWorldsServerWeb, :live_view
  @config_type :characters
  @default_keys [
    "Active",
    "BaseSpeed",
    "BodySize",
    "Class",
    "Faction",
    "Id",
    "Name",
    "SkillActive1",
    "SkillActive2",
    "SkillBasic",
    "SkillDash",
    "SkillUltimate"
  ]

  def mount(_params, _session, socket) do
    config = Utils.Config.read_config(@config_type)
    {:ok, assign(socket, config: to_form(config), characters: Map.keys(config), new_character_name: "")}
  end

  def handle_event("new_character_name", %{"value" => new_name}, socket) do
    {:noreply, assign(socket, :new_character_name, new_name)}
  end

  def handle_event("add_character", %{"name" => new_name}, socket) do
    config = socket.assigns.config.params

    id = map_size(config) |> Integer.to_string()
    name = if new_name == "", do: id, else: new_name

    new_character_config = %{
      Map.new(@default_keys, fn key -> {key, ""} end)
      | "Name" => name,
        "Id" => id
    }

    config = Map.put(config, name, new_character_config)

    socket =
      assign(socket,
        config: to_form(config),
        characters: socket.assigns.characters ++ [name],
        new_character_name: ""
      )

    {:noreply, socket}
  end

  def handle_event("remove_character", %{"name" => name}, socket) do
    config =
      socket.assigns.config.params
      |> Map.drop([name])

    socket =
      assign(socket,
        config: to_form(config),
        characters: List.delete(socket.assigns.characters, name)
      )

    {:noreply, socket}
  end
end
