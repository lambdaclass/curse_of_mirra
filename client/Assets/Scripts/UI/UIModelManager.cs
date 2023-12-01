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
        List<string> availableModels = new List<string>();
        int index = 0;
        string name = "";
        if (character == null)
        {
            //We intersect all the characters model and the enable ones.
            availableModels = playerModels
                .Select(el => el.name)
                .Intersect(enabledCharacters)
                .ToList();
            index = Random.Range(0, availableModels.Count);
            name = availableModels[index];
        }
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
