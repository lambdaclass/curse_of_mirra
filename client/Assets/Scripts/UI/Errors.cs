using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Errors : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI error;

    [SerializeField]
    public TextMeshProUGUI description;

    [SerializeField]
    public List<GameObject> options;

    public void HandleError(string title, string descriptionErr)
    {
        gameObject.SetActive(true);
        error.text = title;
        description.text = descriptionErr;
        if (error.text == "Error")
        {
            options[2].SetActive(true);
        }
        else if (error.text == "You have a game in progress")
        {
            options[0].SetActive(true);
            options[1].SetActive(true);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
