using System;
using System.Collections;
using System.IO;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Networking;

/*
These classes are used to parse the game_settings.json data
*/

public class GameSettings
{
    public string path { get; set; }

    public static IEnumerator ParseSettingsCoroutine(Action<ServerGameSettings> callback)
    {
        JsonParser parser = new JsonParser(new JsonParser.Settings(100000)); //GameSettings

        string gameSettingsPath = Path.Combine(
            Application.streamingAssetsPath,
            "GameSettings.json"
        );
        string jsonGameSettingsText = string.Empty;
        string jsonCharacterSettingsText = string.Empty;
        GetRequest(
            gameSettingsPath,
            result =>
            {
                jsonGameSettingsText = result;

                string charactersSettingsPath = Path.Combine(
                    Application.streamingAssetsPath,
                    "Characters.json"
                );
                GetRequest(
                    charactersSettingsPath,
                    charactersResult =>
                    {
                        jsonCharacterSettingsText = charactersResult;

                        RunnerConfig parsedRunner = parser.Parse<RunnerConfig>(
                            jsonGameSettingsText.TrimStart('\uFEFF')
                        );
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
        UnityWebRequest webRequest;
#if UNITY_EDITOR
        webRequest = UnityWebRequest.Get("file://" + uri);
#else
        webRequest = UnityWebRequest.Get(uri);
#endif
        webRequest.SetRequestHeader("Content-Type", "application/json");

        webRequest.SendWebRequest().completed += operation =>
        {
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                callback?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                callback?.Invoke(string.Empty);
            }
            webRequest.Dispose();
        };
    }
}
