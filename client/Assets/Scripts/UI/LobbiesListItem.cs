using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

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

    public void Disable()
    {
        GetComponent<MMTouchButton>().DisableButton();
        StartCoroutine(RestoreButton());
    }

    IEnumerator RestoreButton()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<MMTouchButton>().EnableButton();
    }

    public void ConnectToLobby()
    {
        LobbiesManager.Instance.ConnectToLobby(idHash);
    }
}
