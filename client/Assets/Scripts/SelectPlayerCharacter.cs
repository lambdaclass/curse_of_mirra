using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class SelectPlayerCharacter : CharacterSelector
{
    public static Character prefab;

    public bool selected = false;
    [SerializeField] LobbyPlayerList playersList;

    public void SelectPrefab()
    {
        print("PLAYER " + LobbyConnection.Instance.playerId + "Selected " + this.CharacterPrefab.name);
        // CustomLevelManager.prefab = this.CharacterPrefab;
        prefab = this.CharacterPrefab;
        selected = true;
        playersList.GetPlayerCharacter(LobbyConnection.Instance.playerId).SetCharacterText(this.CharacterPrefab.name);
    }
}
