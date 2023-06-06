using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/*
These clases are used to parse the game_settings.json data 
*/

public class GameSettings
{
  public string path { get; set; }

  public ServerGameSettings parseSettings()
  {
    string jsonText = File.ReadAllText(this.path);
    JArray array = JArray.Parse(jsonText);

    ServerGameSettings serverSettings = new ServerGameSettings();
    
    foreach (JObject obj in array.Children<JObject>())
    {
      ServerGameSettingsItem item = JsonConvert.DeserializeObject<ServerGameSettingsItem>(obj.ToString());
      serverSettings.GameConfigItems.Add(item);
    }
    return serverSettings;
  }
}
