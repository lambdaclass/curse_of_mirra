using System.IO;
using Google.Protobuf;

/*
These clases are used to parse the game_settings.json data
*/

public class GameSettings
{
    public string path { get; set; }

    public static ServerGameSettings parseSettings(){
        JsonParser parser = new JsonParser(new JsonParser.Settings(100000));
        string jsonGameSettingsText = File.ReadAllText(@"../data/GameSettings.json");
        RunnerConfig parsedRunner = parser.Parse<RunnerConfig>(jsonGameSettingsText);
        
        string jsonCharacterSettingsText = File.ReadAllText(@"../data/Characters.json");
        CharacterConfig characters = parser.Parse<CharacterConfig>(jsonCharacterSettingsText); 

        ServerGameSettings settings = new ServerGameSettings{
          RunnerConfig = parsedRunner,
          CharacterConfig = characters
        };
        return settings;
    }
}
