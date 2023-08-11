using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionUI : MonoBehaviour
{
    public bool updated = false;
    public string selectedCharacterName;
    public string selectedPlayerCharacterName;

    public List<GameObject> GetAllChilds()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            list.Add(transform.GetChild(i).gameObject);
        }
        return list;
    }

    public void DeselectCharacters(string characterName)
    {
        List<GameObject> list = GetAllChilds();
        list.ForEach(el =>
        {
            if (el.GetComponent<UICharacterItem>().selected == true && el.name != characterName)
            {
                el.GetComponent<UICharacterItem>().selected = false;
                el.GetComponent<UICharacterItem>().artWork.sprite =
                    el.GetComponent<UICharacterItem>().comCharacter.artWork;
            }
        });
        updated = true;
    }

    public CoMCharacter GetSelectedCharacter(string characterName)
    {
        List<GameObject> list = GetAllChilds();
        return list.Find(
                el => el.GetComponent<UICharacterItem>().comCharacter.name == characterName
            )
            .GetComponent<UICharacterItem>()
            .comCharacter;
    }

    public void SendCharacterSelection()
    {
        PlayerCharacter characterSelected = new PlayerCharacter
        {
            PlayerId = (ulong)SocketConnectionManager.Instance.playerId,
            CharacterName = selectedCharacterName
        };
        ClientAction clientAction = new ClientAction
        {
            Action = Action.SelectCharacter,
            PlayerCharacter = characterSelected
        };
        SocketConnectionManager.Instance.SendAction(clientAction);
        selectedPlayerCharacterName = characterSelected.CharacterName;
    }
}
