using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using MoreMountains.TopDownEngine;

public class Utils
{
    public static readonly Color healthBarGreen = new Color32(8, 233, 9, 255);
    public static readonly Color healthBarRed = new Color32(255, 26, 0, 255);
    public static readonly Color healthBarPoisoned = new Color32(66, 168, 0, 255);
    public static readonly Color burstLoadsBarCharging = new Color32(110, 110, 110, 255);

    private const string LOBBIES_BACKGROUND_MUSIC = "LobbiesBackgroundMusic";

    public static Vector3 transformBackendOldPositionToFrontendPosition(Position position)
    {
        var x = (float)position?.X / 100f;
        var y = (float)position?.Y / 100f;
        return new Vector3(x, 1f, y);
    }

    // Used to transform units like radius and range.
    public static float TransformBackenUnitToClientUnit(float unit)
    {
        return unit / 100f;
    }

    public static GameObject GetPlayer(ulong id)
    {
        return GameServerConnectionManager
            .Instance
            .players
            .Find(el => el.GetComponent<CustomCharacter>().PlayerID == id.ToString());
    }

    public static Entity GetCrate(ulong id)
    {
        return GameServerConnectionManager
            .Instance
            .gameCrates
            .Find(crate => crate.Id == id);
    }

    public static CustomCharacter GetCharacter(ulong id)
    {
        return GetPlayer(id).GetComponent<CustomCharacter>();
    }

    public static Entity GetGamePlayer(ulong id)
    {
        Entity player = null;
        if (
            GameServerConnectionManager.Instance.gamePlayers != null
            && GameServerConnectionManager.Instance.gamePlayers.Count > 0
        )
        {
            player = GameServerConnectionManager.Instance?.gamePlayers.Find(el => el.Id == id);
        }
        return player;
    }

    public static IEnumerable<Entity> GetAlivePlayers()
    {
        return GameServerConnectionManager
            .Instance
            .gamePlayers
            .Where(player => player.Player.Health > 0);
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

    public static void GoToCharacterInfo(string characterName, string sceneName)
    {
        CharactersManager.Instance.SetGoToCharacter(characterName);
        SceneManager.LoadScene(sceneName);
    }

    public static void BackToLobbyFromGame(string goToScene)
    {
        GameObject.Destroy(GameObject.Find(LOBBIES_BACKGROUND_MUSIC));
        BackToLobbyAndCloseConnection(goToScene);
    }

    public static void BackToLobbyAndCloseConnection(string goToScene)
    {
        // Websocket connection is closed as part of Init() destroy;
        GameServerConnectionManager.Instance.Init();
        DestroySingletonInstances();
        Back(goToScene);
    }

    private static void DestroySingletonInstances()
    {
        if (GameManager.Instance != null)
        {
            GameObject.Destroy(GameManager.Instance.gameObject);
        }
    }

    public static void Back(string goToScene)
    {
        SceneManager.LoadScene(goToScene);
    }

    public static IEnumerator WaitForBattleCreation(string currentSceneName, string battleSceneName, string action_action)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == currentSceneName);
        ServerConnection.Instance.JoinGame(action_action);
        yield return new WaitUntil(
            () =>
                !string.IsNullOrEmpty(ServerConnection.Instance.LobbySession)
                && !string.IsNullOrEmpty(SessionParameters.GameId)
        );
        SceneManager.LoadScene(battleSceneName);
    }
}
