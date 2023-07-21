using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UICharacterItem : MonoBehaviour, IPointerDownHandler
{
    public CoMCharacter comCharacter;
    public TextMeshProUGUI name;
    public Image artWork;
    public bool selected = false;

    [SerializeField]
    public Image skillBasicSprite;

    [SerializeField]
    public Image skill1Sprite;

    [SerializeField]
    public Image skill2Sprite;

    [SerializeField]
    public Image skill3Sprite;

    [SerializeField]
    public Image skill4Sprite;

    void Start()
    {
        if (isActive())
        {
            artWork.sprite = comCharacter.artWork;
        }
        else
        {
            print("llegó");
            artWork.sprite = comCharacter.blockArtwork;
        }
    }

    public bool isActive()
    {
        var charactersList = LobbyConnection.Instance.serverSettings.CharacterConfig.Items;
        foreach (var character in charactersList)
        {
            if (comCharacter.name == character.Name)
            {
                return int.Parse(character.Active) == 1;
            }
        }
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SocketConnectionManager.Instance.isConnectionOpen())
        {
            if (isActive())
            {
                selected = true;
                artWork.sprite = comCharacter.selectedArtwork;
                name.text = comCharacter.name;
                skillBasicSprite.sprite = comCharacter.skillBasicSprite;
                skill1Sprite.sprite = comCharacter.skill1Sprite;
                skill2Sprite.sprite = comCharacter.skill2Sprite;
                skill3Sprite.sprite = comCharacter.skill3Sprite;
                skill4Sprite.sprite = comCharacter.skill4Sprite;
                SendCharacterSelection();
                transform.parent
                    .GetComponent<CharacterSelectionUI>()
                    .DeselectCharacters(comCharacter.name);
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
