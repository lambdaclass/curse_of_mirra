using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void GoToCharacterInfo()
    {
        CharactersManager.Instance.GoToCharacter = characterNameString;
        this.GetComponent<MMLoadScene>().LoadScene();
    }
}
