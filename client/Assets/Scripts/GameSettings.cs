using System.IO;
using Google.Protobuf;
using UnityEngine;

/*
These clases are used to parse the game_settings.json data
*/

public class GameSettings
{
    public string path { get; set; }

    public static ServerGameSettings parseSettings()
    {
        JsonParser parser = new JsonParser(new JsonParser.Settings(100000));//GameSettings

        string jsonGameSettingsText = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "GameSettings.json"));
        //Debug.Log("" + jsonGameSettingsText);
        // string jsonGameSettingsText = File.ReadAllText(@"../data/GameSettings.json");
        RunnerConfig parsedRunner = parser.Parse<RunnerConfig>(jsonGameSettingsText);
        //string jsonCharacterSettingsText = File.ReadAllText(@"../data/Characters.json");
        string jsonCharacterSettingsText = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Characters.json"));
        CharacterConfig characters = parser.Parse<CharacterConfig>(jsonCharacterSettingsText);

        ServerGameSettings settings = new ServerGameSettings
        {
            RunnerConfig = parsedRunner,
            CharacterConfig = characters
        };
        return settings;
    }
}
