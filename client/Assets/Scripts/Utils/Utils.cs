using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Google.Protobuf.Collections;

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

    public static void ChangeCharacterMaterialColor(Character character, Color color)
    {
        for (int i = 0; i < character.CharacterModel.transform.childCount; i++)
        {
            Renderer renderer = character.CharacterModel.transform
                .GetChild(i)
                .GetComponent<Renderer>();
            if (renderer)
            {
                renderer.material.color = color;
            }
        }
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
}
