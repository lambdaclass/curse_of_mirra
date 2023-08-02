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
    bool gamesEmpty = true;

    // Update is called once per frame
    void Update()
    {
        if (lobbiesEmpty && LobbyConnection.Instance.lobbiesList.Count > 0)
        {
            noLobbiesText.SetActive(false);
            GenerateList(LobbyConnection.Instance.lobbiesList, lobbyItemPrefab, lobbiesContainer);
            lobbiesEmpty = false;
        }
        noLobbiesText.SetActive(lobbiesEmpty);
    }

    public void GenerateList(List<string> itemList, Object itemPrefab, Transform container)
    {
        itemList.ForEach(el =>
        {
            GameObject item = (GameObject)Instantiate(itemPrefab, container);
            item.transform.SetAsFirstSibling();
            string lastCharactersInID = el.Substring(el.Length - 5);
            item.GetComponent<LobbiesListItem>().setId(el, lastCharactersInID);
        });
    }
}
