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

  def mount(params, session, socket) do
    config = Utils.Config.read_config(@config_type)
    {:ok, assign(socket, config: to_form(config), characters: Map.keys(config), new_character_name: "")}
  end

  def handle_event("new_character_name", %{"value" => new_name}, socket) do
    {:noreply, assign(socket, :new_character_name, new_name)}
  end

  def handle_event("add_character", %{"name" => new_name}, socket) do
    config = socket.assigns.config.params

    new_character_config = %{
      Map.new(@default_keys, fn key -> {key, ""} end)
      | "Name" => new_name,
        "Id" => map_size(config)
    }

    config = Map.put(config, new_name, new_character_config)

    socket =
      assign(socket,
        config: to_form(config),
        new_character_name: "",
        characters: socket.assigns.characters ++ [new_name]
      )

    {:noreply, socket}
  end
end
