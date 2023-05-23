using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class SelectPlayerCharacter : CharacterSelector
{

    public static Character prefab;

    public void SelectPrefab()
    {
        print("PLAYER " + LobbyConnection.Instance.playerId + "Selected " + this.CharacterPrefab.name);
        // CustomLevelManager.prefab = this.CharacterPrefab;
        prefab = this.CharacterPrefab;
    }
}
