using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Tools;

public class ToggleButton : MonoBehaviour
{
    [SerializeField]
    Sprite notSelectedButton;

    [SerializeField]
    Sprite selectedButton;

    [SerializeField]
    TextMeshProUGUI state;

    [SerializeField]
    PlayerMovement playerMovement;

    public void ToggleClientPrediction()
    {
        if (playerMovement && playerMovement.useClientPrediction)
        {
            ToggleOn();
        }
        else
        {
            ToggleOff();
        }
    }

    public void ToggleOn()
    {
        GetComponent<Image>().sprite = selectedButton;
        state.text = "On";
    }

    public void ToggleOff()
    {
        GetComponent<Image>().sprite = notSelectedButton;
        state.text = "Off";
    }
}
