using System;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class CharacterListItem
    : MonoBehaviour,
        IPointerExitHandler,
        IPointerEnterHandler,
        IPointerUpHandler,
        IPointerDownHandler
{
    [SerializeField]
    public TextMeshProUGUI characterName;

    [SerializeField]
    public Image characterImage,
        characterIconState;

    [SerializeField]
    public GameObject soonLabel,
        characterOpacity,
        availableSoon;
    public int listPosition;
    private bool isInsideCard = false;
    private bool isRealesed = false;

    //Min difference of the touchStartPos and the current touch
    private const float MIN_DIFFERENCE = 6.0f;
    private Vector2 touchStartPos;
    public bool IsEnable = true;

    public void SetCharacterInfoStart(PointerEventData eventData)
    {
        var touchXDifference = Math.Abs(eventData.position.x - touchStartPos.x);
        var touchYDifference = Math.Abs(eventData.position.y - touchStartPos.y);
        if (
            isInsideCard
            && touchXDifference < MIN_DIFFERENCE
            && touchYDifference < MIN_DIFFERENCE
            && IsEnable
        )
        {
            this.GetComponent<MMLoadScene>().LoadScene();
            CharacterInfoManager.characterIndex = listPosition;
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
