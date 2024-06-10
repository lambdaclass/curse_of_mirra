using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PoolsManager : MonoBehaviour
{
    public static List<Entity> gamePools = new List<Entity>();
    public static Dictionary<string, GameObject> poolsVFXs = new Dictionary<string, GameObject>();

    public static void UpdatePools(List<Entity> pools)
    {
        var finishedPoolIds = gamePools.Select(gamePools => gamePools.Id).Except(pools.Select(pool => pool.Id));
        foreach(var finishedPoolId in finishedPoolIds)
        {
            DestroyPool(finishedPoolId);
        }
        gamePools = pools;
    }

    private static void DestroyPool(ulong finishedPoolId)
    {
        if(poolsVFXs.TryGetValue($"{finishedPoolId}_1", out var poolVFX))
        {
            Destroy(poolVFX);
        }
    }
}
