using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class SelectPlayerCharacter : CharacterSelector
{
    public static Character prefab;
    [SerializeField] LobbyManager lobbyManager;

    public bool selected = false;

    public void SelectPrefab()
    {
        print("PLAYER " + LobbyConnection.Instance.playerId + "Selected " + this.CharacterPrefab.name);
        // CustomLevelManager.prefab = this.CharacterPrefab;
        print("click");
        prefab = this.CharacterPrefab;
        lobbyManager.characterSelected = !lobbyManager.characterSelected;
        selected = true;
    }
}
