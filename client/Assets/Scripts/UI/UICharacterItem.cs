using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICharacterItem : MonoBehaviour, IPointerDownHandler
{
    public CoMCharacter comCharacter;
    public Text name;
    public Image artWork;
    public bool selected = false;

    void Start()
    {
        artWork.sprite = comCharacter.artWork;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SocketConnectionManager.Instance.isConnectionOpen())
        {
            selected = true;
            if (selected)
            {
                name.text = comCharacter.name;
                artWork.sprite = comCharacter.selectedArtwork;
                SendCharacterSelection();
                transform.parent
                    .GetComponent<CharacterSelectionUI>()
                    .DeselectCharacters(comCharacter.name);
            }
            else
            {
                artWork.sprite = comCharacter.artWork;
            }
        }
    }

    public void SendCharacterSelection()
    {
        PlayerCharacter characterSelected = new PlayerCharacter
        {
            PlayerId = (ulong)SocketConnectionManager.Instance.playerId,
            CharacterName = name.text
        };
        ClientAction clientAction = new ClientAction
        {
            Action = Action.SelectCharacter,
            PlayerCharacter = characterSelected
        };

        SocketConnectionManager.Instance.SendAction(clientAction);
    }
}
