using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Utils
{
    public static readonly Color healthBarCyan = new Color32(34, 142, 239, 255);
    public static readonly Color healthBarRed = new Color32(219, 0, 134, 255);
    public static readonly Color healthBarPoisoned = new Color32(66, 168, 0, 255);
    class AcceptAllCertificates : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
    public static IEnumerator WaitForGameCreation(string levelName)
    {
        yield return new WaitUntil(
            () => !string.IsNullOrEmpty(LobbyConnection.Instance.GameSession)
        );
        SceneManager.LoadScene(levelName);
    }

    public static Vector3 transformBackendPositionToFrontendPosition(Position position)
    {
        var x = (long)position?.Y / 100f - 50.0f;
        var y = (-((long)position?.X)) / 100f + 50.0f;
        return new Vector3(x, 1f, y);
    }

    public static float transformBackendRadiusToFrontendRadius(float radius)
    {
        return radius * 100f / 5000;
    }

    public static GameObject GetPlayer(ulong id)
    {
        return SocketConnectionManager.Instance.players.Find(
            el => el.GetComponent<CustomCharacter>().PlayerID == id.ToString()
        );
    }

    public static CustomCharacter GetCharacter(ulong id)
    {
        return GetPlayer(id).GetComponent<CustomCharacter>();
    }

    public static Player GetGamePlayer(ulong id)
    {
        Player player = null;
        if (
            SocketConnectionManager.Instance.gamePlayers != null
            && SocketConnectionManager.Instance.gamePlayers.Count > 0
        )
        {
            player = SocketConnectionManager.Instance?.gamePlayers.Find(el => el.Id == id);
        }
        return player;
    }

    public static IEnumerable<Player> GetAlivePlayers()
    {
        return SocketConnectionManager.Instance.gamePlayers.Where(
            player => player.Status == Status.Alive
        );
    }

    public static List<CustomCharacter> GetAllCharacters()
    {
        List<CustomCharacter> result = new List<CustomCharacter>();
        Utils
            .GetAlivePlayers()
            .ToList()
            .ForEach(player => result.Add(Utils.GetCharacter(player.Id)));

        return result;
    }

    public static Player GetNearestPlayer(Position toCompare)
    {
        ulong aux_X = 0;
        ulong aux_Y = 0;
        Player nearest_player = null;
        SocketConnectionManager.Instance.gamePlayers.ForEach(player =>
        {
            if (aux_Y == 0 && aux_Y == 0)
            {
                aux_X = toCompare.X - player.Position.X;
                aux_Y = toCompare.Y - player.Position.Y;
                nearest_player = player;
            }
            else
            {
                if (
                    aux_X > (toCompare.X - player.Position.X)
                    && aux_Y > (toCompare.Y - player.Position.Y)
                )
                {
                    aux_X = toCompare.X - player.Position.X;
                    nearest_player = player;
                }
            }
        });

        // return SocketConnectionManager.Instance.gamePlayers.Find(
        //     player => player;
        // );
        return nearest_player;
    }

    public static MMSimpleObjectPooler SimpleObjectPooler(
        string name,
        Transform parentTransform,
        GameObject objectToPool
    )
    {
        GameObject objectPoolerBuilder = new GameObject();
        objectPoolerBuilder.name = name;
        objectPoolerBuilder.transform.parent = parentTransform;
        MMSimpleObjectPooler objectPooler =
            objectPoolerBuilder.AddComponent<MMSimpleObjectPooler>();
        objectPooler.GameObjectToPool = objectToPool;
        objectPooler.PoolSize = 10;
        objectPooler.NestWaitingPool = true;
        objectPooler.MutualizeWaitingPools = true;
        objectPooler.PoolCanExpand = true;
        objectPooler.FillObjectPool();
        return objectPooler;
    }

    public static List<T> ToList<T>(RepeatedField<T> repeatedField)
    {
        var list = new List<T>();
        foreach (var item in repeatedField)
        {
            list.Add(item);
        }
        return list;
    }

    public static Gradient GetHealthBarGradient(Color color)
    {
        return new Gradient()
        {
            colorKeys = new GradientColorKey[2]
            {
                new GradientColorKey(color, 0),
                new GradientColorKey(color, 1f)
            },
            alphaKeys = new GradientAlphaKey[2]
            {
                new GradientAlphaKey(1, 0),
                new GradientAlphaKey(1, 1)
            }
        };
    }

    public static string MakeHTTPUrl(string path)
    {
        // if (SelectServerIP.GetServerIp().Contains("localhost"))
        // {
        //     return "http://" + SelectServerIP.GetServerIp() + ":4000" + path;
        // }
        // else if (SelectServerIP.GetServerIp().Contains("10.150.20.186"))
        // {
        //     return "http://" + SelectServerIP.GetServerIp() + ":4000" + path;
        // }
        // else
        // {
        //     return "https://" + SelectServerIP.GetServerIp() + path;
        // }

        return "http://" + "localhost" + ":4000" + path;
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
        string url = Utils.MakeHTTPUrl("/users-characters/" + GetClientId());
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if(webRequest.downloadHandler.text.Contains("NOT_FOUND")) {
                        errorCallback?.Invoke("NOT_FOUND");
                    } else {
                        UserCharacterResponse response = JsonUtility.FromJson<UserCharacterResponse>(
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

    public static IEnumerator SetSelectedCharacter(
        string characterName,
        Action<UserCharacterResponse> successCallback, 
        Action<string> errorCallback
    ) {
        string url = Utils.MakeHTTPUrl("/users-characters/" + GetClientId() + "/edit");
        string parametersJson = "{\"selected_character\": \"" + characterName + "\"}";
        byte[] byteArray = Encoding.UTF8.GetBytes(parametersJson);
        using (UnityWebRequest webRequest = UnityWebRequest.Put(url, byteArray))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if(webRequest.downloadHandler.text.Contains("INEXISTENT_USER")) {
                        errorCallback?.Invoke(webRequest.downloadHandler.text);
                    } else {
                        UserCharacterResponse response = JsonUtility.FromJson<UserCharacterResponse>(
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

    public static IEnumerator CreateUser(
        Action<UserCharacterResponse> successCallback, 
        Action<string> errorCallback
    ) {
        string url = Utils.MakeHTTPUrl("/users-characters/new");
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("device_client_id", GetClientId()));
        formData.Add(new MultipartFormDataSection("selected_character", "muflus"));

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, formData))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();

            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if(webRequest.downloadHandler.text.Contains("USER_ALREADY_TAKEN")) {
                        errorCallback?.Invoke(webRequest.downloadHandler.text);
                    } else {
                        UserCharacterResponse response = JsonUtility.FromJson<UserCharacterResponse>(
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
