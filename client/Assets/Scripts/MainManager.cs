using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : LevelSelector
{
    public static MainManager Instance;

    void Start()
    {
        Instance = this;
    }

}
