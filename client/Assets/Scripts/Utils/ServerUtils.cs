using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class ServerUtils
{
    public static string MakeHTTPUrl(string path)
    {
        if (
            SelectServerIP.GetServerIp().Contains("localhost")
            || SelectServerIP.GetServerIp().Contains("10.150.20.186")
            || SelectServerIP.GetServerIp().Contains("168.119.71.104")
            || SelectServerIP.GetServerIp().Contains("176.9.26.172")
        )
        {
            return "http://" + SelectServerIP.GetServerIp() + ":4000" + path;
        }
        else
        {
            return "https://" + SelectServerIP.GetServerIp() + path;
        }
    }

    public static string GetClientId()
    {
        if (!PlayerPrefs.HasKey("client_id"))
        {
            Guid g = Guid.NewGuid();
            PlayerPrefs.SetString("client_id", g.ToString());
        }

        return PlayerPrefs.GetString("client_id");
    }

    public static IEnumerator GetSelectedCharacter(
        Action<UserCharacterResponse> successCallback,
        Action<string> errorCallback
    )
    {
        string url = MakeHTTPUrl("/users-characters/" + GetClientId());

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (webRequest.downloadHandler.text.Contains("NOT_FOUND"))
                {
                    errorCallback?.Invoke("USER_NOT_FOUND");
                }
                else
                {
                    UserCharacterResponse response = JsonUtility.FromJson<UserCharacterResponse>(
                        webRequest.downloadHandler.text
                    );
                    successCallback?.Invoke(response);
                }
                webRequest.Dispose();
            }
            else
            {
                string errorDescription;
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ProtocolError:
                        errorDescription = "Something unexpected happened";
                        errorCallback.Invoke(errorDescription);
                        break;
                    case UnityWebRequest.Result.ConnectionError:
                        errorDescription = "Connection Error";
                        errorCallback.Invoke(errorDescription);
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        errorDescription = "Data processing error.";
                        errorCallback.Invoke(errorDescription);
                        break;
                    default:
                        errorDescription = "Unhandled error.";
                        errorCallback.Invoke(errorDescription);
                        break;
                }

                errorCallback?.Invoke(errorDescription);
            }
        }
    }

    public static IEnumerator CreateUser(
        Action<UserCharacterResponse> successCallback,
        Action<string> errorCallback
    )
    {
        string url = MakeHTTPUrl("/users-characters/new");
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("device_client_id", GetClientId()));
        formData.Add(new MultipartFormDataSection("selected_character", "muflus"));

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, formData))
        {
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (webRequest.downloadHandler.text.Contains("USER_ALREADY_TAKEN"))
                    {
                        errorCallback?.Invoke(webRequest.downloadHandler.text);
                    }
                    else
                    {
                        UserCharacterResponse response =
                            JsonUtility.FromJson<UserCharacterResponse>(
                                webRequest.downloadHandler.text
                            );
                        successCallback?.Invoke(response);
                    }
                    break;
                default:
                    errorCallback?.Invoke(webRequest.downloadHandler.error);
                    break;
            }
        }
    }
}
