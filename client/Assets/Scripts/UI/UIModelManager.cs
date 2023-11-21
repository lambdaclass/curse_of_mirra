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

    public void SetModel(CoMCharacter character = null)
    {
        int index = Random.Range(0, playerModels.Count);
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

    public void RemoveCurrentModel()
    {
        if (playerModelContainer.transform.childCount > 0)
        {
            Destroy(playerModelContainer.transform.GetChild(0).gameObject);
        }
    }
}
