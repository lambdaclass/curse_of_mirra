using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public static readonly Color burstLoadsBarCharging = new Color32(110, 110, 110, 255);

    public static Vector3 transformBackendOldPositionToFrontendPosition(Position position)
    {
        var x = (long)position?.X / 100f;
        var y = (long)position?.Y / 100f;
        return new Vector3(x, 1f, y);
    }

    // public static Vector3 transformBackendPositionToFrontendPosition(Game.Position position)
    // {
    //     var x = (long)position?.x / 100f;
    //     var y = (long)position?.y / 100f;
    //     return new Vector3(x, 1f, y);
    // }

    // public static float transformBackendRadiusToFrontendRadius(float radius)
    // {
    //     return radius * 100f / 5000;
    // }

    public static GameObject GetPlayer(ulong id)
    {
        return GameServerConnectionManager
            .Instance
            .players
            .Find(el => el.GetComponent<CustomCharacter>().PlayerID == id.ToString());
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

    // public static List<T> ToList<T>(RepeatedField<T> repeatedField)
    // {
    //     var list = new List<T>();
    //     foreach (var item in repeatedField)
    //     {
    //         list.Add(item);
    //     }
    //     return list;
    // }

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
