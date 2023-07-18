using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

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
        var x = (long)position.Y / 10f - 50.0f;
        var y = (-((long)position.X)) / 10f + 50.0f;
        return new Vector3(x, 1f, y);
    }

    public static GameObject GetPlayer(ulong id)
    {
        return SocketConnectionManager.Instance.players.Find(
            el => el.GetComponent<Character>().PlayerID == id.ToString()
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
