using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButtonHandler : MonoBehaviour
{
    [SerializeField]
    CharacterSelectionUI characterSelectionUI;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void HandleButton()
    {
        if (
            characterSelectionUI.selectedPlayerCharacterName
            == characterSelectionUI.selectedCharacterName
        )
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
}
