using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIModelManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerModelContainer;

    [SerializeField]
    List<GameObject> playerModels;

    private List<string> enableCharacters = new List<string>();

    public void SetModel(CoMCharacter character = null)
    {
        List<string> avaibleModels = new List<string>();
        int index = 0;
        string name = "";
        if (character == null)
        {
            avaibleModels = playerModels.Select(el => el.name).Intersect(enableCharacters).ToList();
            index = Random.Range(0, avaibleModels.Count);
            name = avaibleModels[index];
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

    //This function add the " Variant" to each string and add it to a new list
    // With this we can match the Model variants for the UI model rendering
    public void SetupList(List<string> characters)
    {
        enableCharacters = characters;
    }

    public void RemoveCurrentModel()
    {
        if (playerModelContainer.transform.childCount > 0)
        {
            Destroy(playerModelContainer.transform.GetChild(0).gameObject);
        }
    }
}
