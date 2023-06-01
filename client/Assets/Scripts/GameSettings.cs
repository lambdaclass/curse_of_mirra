using System.IO;
using Newtonsoft.Json;

/*
These clases are used to parse the game_settings.json data 
*/

public class GameSettings
{
    public string path { get; set; }

    private class boardSize{
        public uint width { get; set;}
        public uint height { get; set;}
    }

    private class Settings {
        public boardSize board_size;
        public uint server_tickrate_ms;
        public uint game_timeout_ms;
    }

    public GameConfig parseSettings(){
        string jsonText = File.ReadAllText(this.path);
        Settings settings = JsonConvert.DeserializeObject<Settings>(jsonText);
        BoardSize bSize = new BoardSize {Width = settings.board_size.width, Height = settings.board_size.height};

        GameConfig gameConfig = new GameConfig {
            BoardSize = bSize,
            ServerTickrateMs = settings.server_tickrate_ms,
            GameTimeoutMs = settings.game_timeout_ms
        };

        return gameConfig;
    }

    public static GameConfig defaultSettings(){
        BoardSize bSize = new BoardSize{Width = 1000, Height = 1000};

        GameConfig gameConfig = new GameConfig{
            BoardSize = bSize,
            ServerTickrateMs = 30,
            GameTimeoutMs = 1_200_000
        };

        return gameConfig;
    }
}
