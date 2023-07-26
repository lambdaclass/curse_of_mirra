using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils
{
    public static IEnumerator WaitForGameCreation(string levelName)
    {
        yield return new WaitUntil(
            () => !string.IsNullOrEmpty(LobbyConnection.Instance.GameSession)
        );
        SceneManager.LoadScene(levelName);
    }

    public static Vector3 transformBackendPositionToFrontendPosition(Position position)
    {
        var x = (long)position.Y / 100f - 50.0f;
        var y = (-((long)position.X)) / 100f + 50.0f;
        return new Vector3(x, 1f, y);
    }

    public static float transformBackendRadiusToFrontendRadius(float radius)
    {
        return radius * 100f / 5000;
    }

    public static GameObject GetPlayer(ulong id)
    {
        return SocketConnectionManager.Instance.players.Find(
            el => el.GetComponent<Character>().PlayerID == id.ToString()
        );
    }

    public static Player GetGamePlayer(ulong gamePlayerId)
    {
        return SocketConnectionManager.Instance.gamePlayers.Find(gp => gp.Id == gamePlayerId);
    }

    public static IEnumerable<Player> GetAlivePlayers()
    {
        return SocketConnectionManager.Instance.gamePlayers.Where(
            player => player.Status == Status.Alive
        );
    }

    public static MMSimpleObjectPooler SimpleObjectPooler(
        string name,
        Transform parentTransform,
        string resource
    )
    {
        GameObject objectPoolerGameObject = new GameObject();
        objectPoolerGameObject.name = name;
        objectPoolerGameObject.transform.parent = parentTransform;
        MMSimpleObjectPooler objectPooler =
            objectPoolerGameObject.AddComponent<MMSimpleObjectPooler>();
        objectPooler.GameObjectToPool = Resources.Load(resource, typeof(GameObject)) as GameObject;
        objectPooler.PoolSize = 10;
        objectPooler.NestWaitingPool = true;
        objectPooler.MutualizeWaitingPools = true;
        objectPooler.PoolCanExpand = true;
        objectPooler.FillObjectPool();
        return objectPooler;
    }
}
