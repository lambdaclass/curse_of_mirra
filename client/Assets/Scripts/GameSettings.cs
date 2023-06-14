using System;
using System.Collections;
using System.IO;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Networking;

/*
These clases are used to parse the game_settings.json data
*/

public class GameSettings
{
    public string path { get; set; }

    // public static ServerGameSettings parseSettings()
    // {
    //     JsonParser parser = new JsonParser(new JsonParser.Settings(100000)); //GameSettings

    //     string jsonGameSettingsText = File.ReadAllText(
    //         Path.Combine(Application.streamingAssetsPath, "GameSettings.json")
    //     );
    //     RunnerConfig parsedRunner = parser.Parse<RunnerConfig>(jsonGameSettingsText);
    //     string jsonCharacterSettingsText = File.ReadAllText(
    //         Path.Combine(Application.streamingAssetsPath, "Characters.json")
    //     );
    //     CharacterConfig characters = parser.Parse<CharacterConfig>(jsonCharacterSettingsText);

    //     ServerGameSettings settings = new ServerGameSettings
    //     {
    //         RunnerConfig = parsedRunner,
    //         CharacterConfig = characters
    //     };
    //     return settings;
    // }

    // public static ServerGameSettings parseSettings()
    // {
    //     JsonParser parser = new JsonParser(new JsonParser.Settings(100000)); //GameSettings

    //     string gameSettingsPath = Path.Combine(
    //         Application.streamingAssetsPath,
    //         "GameSettings.json"
    //     );
    //     GetRequest(gameSettingsPath);
    //     Debug.Log("After get request");
    //     RunnerConfig parsedRunner = parser.Parse<RunnerConfig>(jsonGameSettingsText);

    //     string charactersSettingsPath = Path.Combine(
    //         Application.streamingAssetsPath,
    //         "Characters.json"
    //     );
    //     GetRequest(charactersSettingsPath);
    //     CharacterConfig characters = parser.Parse<CharacterConfig>(jsonCharacterSettingsText);

    //     ServerGameSettings settings = new ServerGameSettings
    //     {
    //         RunnerConfig = parsedRunner,
    //         CharacterConfig = characters
    //     };
    //     return settings;
    // }

    // static IEnumerator GetRequest(string uri, out string jsonText)
    // {
    //     using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    //     {
    //         webRequest.SetRequestHeader("Content-Type", "application/json");
    //         jsonText = string.Empty;
    //         yield return webRequest.SendWebRequest();
    //         switch (webRequest.result)
    //         {
    //             case UnityWebRequest.Result.Success:
    //                 jsonText = webRequest.downloadHandler.text;
    //                 break;
    //             default:
    //                 jsonText = string.Empty;
    //                 break;
    //         }
    //     }
    // }

    public static IEnumerator ParseSettingsCoroutine(Action<ServerGameSettings> callback)
    {
        JsonParser parser = new JsonParser(new JsonParser.Settings(100000)); //GameSettings

        string gameSettingsPath = Path.Combine(
            Application.streamingAssetsPath,
            "GameSettings.json"
        );
        string jsonGameSettingsText = string.Empty;
        string jsonCharacterSettingsText = string.Empty;
        Debug.Log("Before first get request");
        GetRequest(
            gameSettingsPath,
            result =>
            {
                jsonGameSettingsText = result;

                string charactersSettingsPath = Path.Combine(
                    Application.streamingAssetsPath,
                    "Characters.json"
                );
                Debug.Log("Before second get request");
                GetRequest(
                    charactersSettingsPath,
                    charactersResult =>
                    {
                        jsonCharacterSettingsText = charactersResult;

                        Debug.Log("Before first parse");
                        Debug.Log(jsonGameSettingsText);
                        RunnerConfig parsedRunner = parser.Parse<RunnerConfig>(
                            jsonGameSettingsText.TrimStart('\uFEFF')
                        );
                        Debug.Log("Before second parse");
                        CharacterConfig characters = parser.Parse<CharacterConfig>(
                            jsonCharacterSettingsText.TrimStart('\uFEFF')
                        );

                        ServerGameSettings settings = new ServerGameSettings
                        {
                            RunnerConfig = parsedRunner,
                            CharacterConfig = characters
                        };

                        callback?.Invoke(settings);
                    }
                );
            }
        );

        while (
            string.IsNullOrEmpty(jsonGameSettingsText)
            || string.IsNullOrEmpty(jsonCharacterSettingsText)
        )
        {
            yield return null;
        }
    }

    static void GetRequest(string uri, Action<string> callback)
    {
        Debug.Log("Getting request for: " + uri);
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        webRequest.SetRequestHeader("Content-Type", "application/json");

        webRequest.SendWebRequest().completed += operation =>
        {
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("success: " + webRequest.downloadHandler.text);
                callback?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log("error: " + webRequest.error);
                callback?.Invoke(string.Empty);
            }
            webRequest.Dispose();
        };
    }
}
