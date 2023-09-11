using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Transform lobbiesContainer;

    [SerializeField]
    GameObject lobbyItemPrefab;

    [SerializeField]
    GameObject noLobbiesText;

    bool lobbiesEmpty = true;
    public List<string> lobbiesList;
    private IEnumerator init;

    void Start()
    {
        noLobbiesText.SetActive(lobbiesEmpty);
        init = InitializeList();
        StartCoroutine(init);
    }

    private IEnumerator InitializeList()
    {
        yield return new WaitUntil(() => LobbyConnection.Instance.lobbiesList.Count > 0);
        GenerateList();
        Debug.Log("hi");
    }

    void GenerateList()
    {
        lobbiesList = new List<string>();
        lobbiesList = LobbyConnection.Instance.lobbiesList;

        noLobbiesText.SetActive(false);
        lobbiesList.Reverse();
        lobbiesList.ForEach(el =>
        {
            GameObject item = (GameObject)Instantiate(lobbyItemPrefab, lobbiesContainer);
            string lastCharactersInID = el.Substring(el.Length - 5);
            item.GetComponent<LobbiesListItem>().setId(el, lastCharactersInID);
        });
        lobbiesEmpty = false;
    }

    public void RefreshLobbiesList()
    {
        init = InitializeList();
        StartCoroutine(init);
        List<LobbiesListItem> lobbyItems = new List<LobbiesListItem>();
        lobbyItems.AddRange(lobbiesContainer.GetComponentsInChildren<LobbiesListItem>());
        lobbyItems.ForEach(lobbyItem =>
        {
            Destroy(lobbyItem.gameObject);
        });
    }
}
