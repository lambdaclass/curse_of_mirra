using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    // Start is called before the first frame update
    int characterSelected = -1;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<SelectPlayerCharacter>().selected)
            {
                transform.GetChild(i).GetComponent<MMTouchButton>().DisableButton();
                if (characterSelected != -1 && characterSelected != i)
                {
                    SetSelectedCharacter(characterSelected);
                }
                characterSelected = i;
            }
        }
    }

    void SetSelectedCharacter(int selected)
    {
        transform.GetChild(selected).GetComponent<SelectPlayerCharacter>().selected = false;
        transform.GetChild(selected).GetComponent<MMTouchButton>().EnableButton();
    }
}
