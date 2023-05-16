using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class BackButton : LevelSelector
{
    public override void GoToLevel()
    {
        base.GoToLevel();
        LobbyConnection.Instance.Init();
    }
}
