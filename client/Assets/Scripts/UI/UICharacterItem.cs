using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICharacterItem : MonoBehaviour, IPointerDownHandler
{
    public CoMCharacter comCharacter;
    public Text name;
    public Image artWork;
    public bool selected = false;
    void Start()
    {
        artWork.sprite = comCharacter.artWork;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        selected = !selected;
        if (selected)
        {
            name.text = comCharacter.name;
            artWork.sprite = comCharacter.selectedArtwork;
            CustomLevelManager.prefab = comCharacter.prefab;
        }
        else
        {
            artWork.sprite = comCharacter.artWork;
        }
    }
}
