using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils
{
    public static readonly Color healthBarCyan = new Color32(34, 142, 239, 255);
    public static readonly Color healthBarRed = new Color32(219, 0, 134, 255);
    public static readonly Color healthBarPoisoned = new Color32(66, 168, 0, 255);

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

    public static List<CoMCharacter> GetOnlyAvailableCharacterInfo(List<CoMCharacter> comCharacters)
    {
        var result = new List<CoMCharacter>();
        //This can be totally improved and simplified to one step

        //First we get the names of the avaible characters+
        var avaibleCharacterInfo = comCharacters
            .Select(el => el.name)
            .Intersect(MainScreenManager.enabledCharactersName);

        //Then we get the characterInfo from those names
        for (int i = 0; i < comCharacters.Count(); i++)
        {
            for (int j = 0; j < avaibleCharacterInfo.Count(); j++)
            {
                if (comCharacters[i].name.ToUpper() == avaibleCharacterInfo.ToList()[j].ToUpper())
                {
                    result.Add(comCharacters[i]);
                }
            }
        }

        return result;
    }
}
