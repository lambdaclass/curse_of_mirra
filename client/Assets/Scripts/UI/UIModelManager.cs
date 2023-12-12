using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIModelManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerModelContainer;

    public void SetModel(CoMCharacter character = null)
    {
        GameObject playerModel =
            character != null
                ? character.UIModel
                : CharactersList.Instance.AvailableCharacters
                    .Find(
                        character =>
                            character.name.ToLower()
                            == LobbyConnection.Instance.selectedCharacterName.ToLower()
                    )
                    .UIModel;
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
