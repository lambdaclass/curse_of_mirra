using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using NativeWebSocket;

public static class ServerUtils
{
    public static string MakeHTTPUrl(string path)
    {
        List<String> servers = new List<String>
        {
            "localhost",
            "10.150.20.186",
            "168.119.71.104",
            "176.9.26.172"
        };
        if (servers.Any((ip) => SelectServerIP.GetServerIp() == ip))
        {
            return "http://" + SelectServerIP.GetServerIp() + ":4000" + path;
        }
        else
        {
            return "https://" + SelectServerIP.GetServerIp() + path;
        }
    }

    public static string MakeGatewayHTTPUrl(string path)
    {
        return SelectServerIP.GetGatewayIp() + path;
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

    public static string GetGatewayToken()
    {
        string gatewayJwtPref = SelectServerIP.GetGatewayJwtPref();
        return PlayerPrefs.GetString(gatewayJwtPref);
    }

    public static void SetGatewayToken(string value)
    {

        string gatewayJwtPref = SelectServerIP.GetGatewayJwtPref();
        PlayerPrefs.SetString(gatewayJwtPref, value);
    }

    public static void SetUserId(string value) {
        PlayerPrefs.SetString("user_id", value);
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

    public static IEnumerator SetSelectedCharacter(
        string characterName,
        Action<UserCharacterResponse> successCallback,
        Action<string> errorCallback
    )
    {
        string url = MakeHTTPUrl("/users-characters/" + GetClientId() + "/edit");
        string parametersJson = "{\"selected_character\": \"" + characterName + "\"}";
        byte[] byteArray = Encoding.UTF8.GetBytes(parametersJson);
        using (UnityWebRequest webRequest = UnityWebRequest.Put(url, byteArray))
        {
            // webRequest.certificateHandler = new AcceptAllCertificates();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (webRequest.downloadHandler.text.Contains("INEXISTENT_USER"))
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

    public static IEnumerator CreateUser(
        Action<UserCharacterResponse> successCallback,
        Action<string> errorCallback
    )
    {
        string url = MakeHTTPUrl("/users-characters/new");
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("device_client_id", GetClientId()));
        formData.Add(new MultipartFormDataSection("selected_character", "Muflus"));

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

    public static IEnumerator GetUser(
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
                UserCharacterResponse response = JsonUtility.FromJson<UserCharacterResponse>(
                    webRequest.downloadHandler.text
                );
                successCallback?.Invoke(response);
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

     public static IEnumerator GetTokenIdValidation(
        string tokenID,
        Action<string> successCallback,
        Action<string> errorCallback
    )
    {
        // You can replace central-europe-testing.curseofmirra.com with some ngrok for
        // testing purposes.
        string url = MakeGatewayHTTPUrl("/auth/google/token/" + tokenID + "/" + GetClientId());

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Successfully logged in
                Debug.Log("RESPOND"  + webRequest.downloadHandler.text);
                var response = webRequest.downloadHandler.text;
                successCallback?.Invoke(response);
                webRequest.Dispose();
            }
            else
            {
                errorCallback?.Invoke(webRequest.error);
            }
        }
    }

    public static IEnumerator CreateGuestUser(
        Action<string> successCallback,
        Action<string> errorCallback
    )
    {
        string url = MakeGatewayHTTPUrl("/curse/users");

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, ""))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            CreateGuestUserRequest createGuestUserRequest = new CreateGuestUserRequest();
            createGuestUserRequest.client_id = GetClientId();
            string jsonString = JsonUtility.ToJson(createGuestUserRequest);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);

            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Successfully logged in
                Debug.Log("RESPOND"  + webRequest.downloadHandler.text);
                var response = webRequest.downloadHandler.text;
                successCallback?.Invoke(response);
                webRequest.Dispose();
            }
            else
            {
                errorCallback?.Invoke(webRequest.error);
            }
        }
    }

    public static IEnumerator RefreshToken(
        Action<string> successCallback,
        Action<string> errorCallback
    )
    {
        string url = MakeGatewayHTTPUrl("/auth/refresh-token");

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, ""))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest();
            refreshTokenRequest.client_id = GetClientId();
            refreshTokenRequest.gateway_jwt = GetGatewayToken();
            string jsonString = JsonUtility.ToJson(refreshTokenRequest);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);

            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Successfully logged in
                Debug.Log("RESPOND"  + webRequest.downloadHandler.text);
                var response = webRequest.downloadHandler.text;
                successCallback?.Invoke(response);
                webRequest.Dispose();
            }
            else
            {
                errorCallback?.Invoke(webRequest.error);
            }
        }
    }

    public static IEnumerator ClaimDailyReward(
        Action<string> successCallback,
        Action<string> errorCallback
    )
    {
        string url = MakeGatewayHTTPUrl($"/curse/users/" + PlayerPrefs.GetString("user_id") + "/claim_daily_reward");
     
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    successCallback?.Invoke(webRequest.downloadHandler.text);
                    break;
                default:
                    errorCallback?.Invoke(webRequest.error);
                    break;
            }
        }
    }

    public static bool isGatewayTokenValid()
    {
        string gatewayToken = GetGatewayToken();
        string claimsEncoded = gatewayToken.Split(".")[1];
        GatewayTokenClaims claims = DecodeBase64UrlSafeString<GatewayTokenClaims>(claimsEncoded);
        // Consider invalid 30 minutes (1800 seconds) before expiration
        return (claims.exp - 1800) > DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public static WebSocket CreateWebSocket(string url)
    {
        if (!isGatewayTokenValid())
        {
            RefreshToken(
                rawResponse => {
                    TokenResponse response = JsonUtility.FromJson<TokenResponse>(rawResponse);
                    ServerUtils.SetGatewayToken(response.gateway_jwt);
                },
                error => {
                    Debug.Log("Error refreshing token: " + error);
                }
            );
        }

        string gateway_jwt = ServerUtils.GetGatewayToken();
        string urlWithJwt = url + "?gateway_jwt=" + gateway_jwt;
        return new WebSocket(urlWithJwt);
    }

    [Serializable]
    private class CreateGuestUserRequest
    {
        public string client_id;
    }

    [Serializable]
    private class RefreshTokenRequest
    {
        public string client_id;
        public string gateway_jwt;
    }

    [Serializable]
    private class GatewayTokenClaims
    {
        public string dev;
        public long exp;
        public string sub;
    }

    [Serializable]
    public class TokenResponse
    {
        public string user_id;
        public string gateway_jwt;
    }

    private static T DecodeBase64UrlSafeString<T>(string base64UrlSafeString)
    {
        // Replace URL safe characters with Base64 characters
        string base64 = base64UrlSafeString.Replace('-', '+').Replace('_', '/');

        // Add padding if necessary
        int padding = 4 - (base64.Length % 4);
        if (padding != 4)
        {
            base64 = base64.PadRight(base64.Length + padding, '=');
        }

        // Convert Base64 string to byte array
        byte[] data = Convert.FromBase64String(base64);

        // Convert byte array to JSON string
        string json = Encoding.UTF8.GetString(data);

        // Deserialize JSON string to object
        return JsonUtility.FromJson<T>(json);
    }
}
