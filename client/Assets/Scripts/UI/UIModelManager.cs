using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIModelManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerModelContainer;

    [Tooltip("All UI character models")]
    [SerializeField]
    List<GameObject> playerModels;

    [Tooltip("Enable characters name to be used")]
    private List<string> enabledCharacters = new List<string>();

    public void SetModel(CoMCharacter character = null)
    {
        print("selected character: " + LobbyConnection.Instance.SelectedCharacterName);
        int index;
        switch(LobbyConnection.Instance.SelectedCharacterName) {
            case "Muflus":
                index = 0;
                break;
            case "H4ck":
                index = 1;
                break;
            case "DAgna":
                index = 2;
                break;
            case "Uma":
                index = 3;
                break;
            default:
                index = Random.Range(0, playerModels.Count);
                break;
        }
        string name = playerModels[index].name;
        GameObject playerModel =
            character != null
                ? character.UIModel
                : playerModels.Single(playerModel => playerModel.name == name);
        GameObject modelClone = Instantiate(
            playerModel,
            playerModelContainer.transform.position,
            playerModel.transform.rotation,
            playerModelContainer.transform
        );
    }

    public void SetupList(List<string> characters)
    {
        enabledCharacters = characters;
    }

    public void RemoveCurrentModel()
    {
        if (playerModelContainer.transform.childCount > 0)
        {
            Destroy(playerModelContainer.transform.GetChild(0).gameObject);
        }
    }
}
