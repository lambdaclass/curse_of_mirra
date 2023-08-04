defmodule DarkWorldsServerWeb.ConfigLive.Skills do
  use DarkWorldsServerWeb, :live_view
  @config_type :skills
  @default_keys [
    "Name",
    "DoFunc",
    "ButtonType",
    "Cooldown",
    "Damage",
    "Status",
    "Duration",
    "Projectile",
    "Minion"
  ]

  def mount(_params, _session, socket) do
    config = Utils.Config.read_config(@config_type)
    {:ok, assign(socket, config: to_form(config), skills: Map.keys(config), new_skill_name: "", new_key_name: "")}
  end

  def handle_event("new_skill_name", %{"value" => new_name}, socket) do
    {:noreply, assign(socket, :new_skill_name, new_name)}
  end

  def handle_event("new_key_name", %{"value" => new_name}, socket) do
    {:noreply, assign(socket, :new_key_name, new_name)}
  end

  def handle_event("add_skill", %{"name" => name}, socket) do
    config = socket.assigns.config.params

    # Get the currently existing keys for other skills if any
    keys =
      if map_size(config) == 0, do: @default_keys, else: Map.keys(config |> Enum.at(0) |> elem(1))

    name = if name == "", do: "Skill ##{map_size(config) + 1}"
    new_skill_config = %{Map.new(keys, fn key -> {key, ""} end) | "Name" => name}

    config = Map.put(config, name, new_skill_config)

    socket =
      assign(socket,
        config: to_form(config),
        skills: socket.assigns.skills ++ [name],
        new_skill_name: ""
      )

    {:noreply, socket}
  end

  def handle_event("remove_skill", %{"name" => name}, socket) do
    config =
      socket.assigns.config.params
      |> Map.drop([name])

    socket =
      assign(socket,
        config: to_form(config),
        skills: List.delete(socket.assigns.skills, name)
      )

    {:noreply, socket}
  end

  # TODO: Tell user to fill input
  def handle_event("add_key", %{"name" => ""}, socket), do: {:noreply, socket}

  def handle_event("add_key", %{"name" => name}, socket) do
    config =
      socket.assigns.config.params
      |> Map.new(fn {char_name, char_config} -> {char_name, Map.put(char_config, name, "")} end)

    {:noreply, assign(socket, config: to_form(config), new_key_name: "")}
  end

  def handle_event("remove_key", %{"name" => name}, socket) do
    config =
      socket.assigns.config.params
      |> Map.new(fn {char_name, char_config} -> {char_name, Map.drop(char_config, [name])} end)

    {:noreply, assign(socket, config: to_form(config))}
  end
end
