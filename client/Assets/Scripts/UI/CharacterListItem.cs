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
    public bool IsEnable = true;
}
