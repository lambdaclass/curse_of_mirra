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
    public GameObject yesButton;

    [SerializeField]
    public GameObject noButton;

    [SerializeField]
    public GameObject okButton;

    public void HandleError(string title, string descriptionErr)
    {
        gameObject.SetActive(true);
        error.text = title;
        description.text = descriptionErr;
        if (error.text == "Error")
        {
            okButton.SetActive(true);
        }
        else if (error.text == "You have a game in progress")
        {
            yesButton.SetActive(true);
            noButton.SetActive(true);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
