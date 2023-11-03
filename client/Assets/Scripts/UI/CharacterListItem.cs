using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterListItem
    : MonoBehaviour,
        IPointerExitHandler,
        IPointerEnterHandler,
        IPointerUpHandler,
        IPointerDownHandler
{
    public int listPosition;
    public bool isInsideCard = false;
    public bool isRealesed = false;
    Vector2 touchStartPos;

    public void SetCharacterInfoStart(PointerEventData eventData)
    {
        if (isInsideCard && eventData.position == touchStartPos)
        {
            this.GetComponent<MMLoadScene>().LoadScene();
            CharacterInfoManager.selectedCharacterPosition = listPosition;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInsideCard = false;
        isRealesed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInsideCard = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetCharacterInfoStart(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchStartPos = eventData.position;
    }
}
