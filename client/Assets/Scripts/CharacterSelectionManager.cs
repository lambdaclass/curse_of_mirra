using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CharacterSelectionManager : LevelSelector
{
    // [SerializeField] GameObject playButton;
    // [SerializeField] CharacterSelectionUI characterList;
    // [SerializeField] CharacterSelectionList playerCharacterList;

    // public bool createdItem = false;
    // public bool updated = false;

    // void Start()
    // {
    //     if (playButton != null)
    //     {
    //         if (LobbyConnection.Instance.playerId == 1)
    //         {
    //             playButton.SetActive(true);
    //         }
    //         else
    //         {
    //             playButton.SetActive(false);
    //         }
    //     }
    // }

    // void Update()
    // {
    //     if (characterList.selected == true && createdItem == false)
    //     {
    //         createdItem = true;
    //         playerCharacterList.createPlayerItems(LobbyConnection.Instance.playerId);
    //     }
    // }

    // public void GameStart()
    // {
    //     StartCoroutine(CreateGame());
    //     StartCoroutine(Utils.WaitForGameCreation(this.LevelName));
    // }

    // public IEnumerator CreateGame()
    // {
    //     yield return LobbyConnection.Instance.StartGame();
    // }
}
