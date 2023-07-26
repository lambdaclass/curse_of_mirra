using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbiesListItem : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI idContainer;
    public string idHash;

    public void setId(string id)
    {
        idHash = id;
        idContainer.text = id;
    }
}
