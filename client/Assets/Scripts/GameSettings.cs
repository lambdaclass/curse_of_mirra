using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
These clases are used to parse the game_settings.json data 
*/

public class GameSettings
{
  public string path { get; set; }

  [Serializable]
  public class ClientGameSettingsItem {
    public string Name;
    public string Value;
  }

  [Serializable]
  public class ClientGameSettings {
    public ClientGameSettingsItem[] items;
  }


  public ServerGameSettings parseSettings()
  {
    string jsonText = File.ReadAllText(this.path);
    ClientGameSettings parsedSettings = JsonUtility.FromJson<ClientGameSettings>(jsonText);

    ServerGameSettings serverGameSettings = new ServerGameSettings();
    foreach(var item in parsedSettings.items){
      serverGameSettings.GameConfigItems.Add(new ServerGameSettingsItem {
        Name = item.Name,
        Value = item.Value
      });
    }
    return serverGameSettings;
  }
}
