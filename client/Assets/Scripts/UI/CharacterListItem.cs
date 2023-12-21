using System;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CharacterListItem : MonoBehaviour
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
    public string characterNameString;
    public bool IsEnable = true;

    void Start()
    {
        StartCoroutine(GoToCharacterInfo());
    }

    IEnumerator GoToCharacterInfo()
    {
        yield return new WaitUntil(
            () => this.GetComponent<ButtonAnimationsMMTouchButton>().executeRelease && IsEnable
        );
        this.GetComponent<MMLoadScene>().LoadScene();
    }
}
