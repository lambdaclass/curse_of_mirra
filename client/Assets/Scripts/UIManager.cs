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

    void Start()
    {
        SetEmptyListIndicator(true);
        StartCoroutine(InitializeList());
    }

    private IEnumerator InitializeList()
    {
        yield return new WaitForSeconds(0.5f);
        SetEmptyListIndicator(LobbyConnection.Instance.lobbiesList.Count == 0);
        EmptyList();
        GenerateList();
    }

    void SetEmptyListIndicator(bool state)
    {
        noLobbiesText.SetActive(state);
    }

    void EmptyList()
    {
        List<LobbiesListItem> lobbyItems = new List<LobbiesListItem>();
        lobbyItems.AddRange(lobbiesContainer.GetComponentsInChildren<LobbiesListItem>());
        lobbyItems.ForEach(lobbyItem =>
        {
            Destroy(lobbyItem.gameObject);
        });
    }

    void GenerateList()
    {
        List<string> lobbiesList = LobbyConnection.Instance.lobbiesList;
        lobbiesList.Reverse();
        lobbiesList.ForEach(el =>
        {
            GameObject item = (GameObject)Instantiate(lobbyItemPrefab, lobbiesContainer);
            string lastCharactersInID = el.Substring(el.Length - 5);
            item.GetComponent<LobbiesListItem>().setId(el, lastCharactersInID);
        });
    }

    public void RefreshLobbiesList()
    {
        StartCoroutine(InitializeList());
    }
}
