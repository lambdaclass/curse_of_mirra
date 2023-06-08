# Game Settings

We have file called `game_settings.json` which will be used to set up our settings for the game. These settings are basically a key-value pair using a json file. At the moment we're allowing a few things to be configurable, but in the future we can discuss which things could be useful for us and add the needed logic to have it as a custom parameter.

In the beginning we're just supporting options server related, but we will iterate to have more options for the client.

## Settings Supported
We're currently supporting the following options:
```
{
  "board_size": {"width": 1000, "height": 1000}, # The size of the board
  "server_tickrate_ms": 30, # Measured in miliseconds, it represents how often the server is sending the updates to the client
  "game_timeout_ms": 20, # Measured in miliseconds, it represents how much time the game session lasts
}
```

## Adding a new parameter

In case you want to add a new parameter, there a few things to do:
- Add the new parameter in the `game_settings.json` file
- Go to the `GameSettings.cs` file and add the new attribute to the `Settings` class and if its a composed attribute, do something like the board_size attribute
- Add the attribute to the `GameConfig` struct in `messages.proto`.
- Run the `gen-protobuf` command (makefile phony)
- Add the needed logic in the server
