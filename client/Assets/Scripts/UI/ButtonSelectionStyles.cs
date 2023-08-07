using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelectionStyles : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    Sprite notSelectedButton;

    [SerializeField]
    Sprite selectedButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = selectedButton;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = notSelectedButton;
    }
}
