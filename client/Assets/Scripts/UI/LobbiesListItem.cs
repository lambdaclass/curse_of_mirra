using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbiesListItem : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI idContainer;
    public string idHash;

    public void setId(string id, string lastCharacters)
    {
        idHash = id;
        idContainer.text = lastCharacters;
    }
}
