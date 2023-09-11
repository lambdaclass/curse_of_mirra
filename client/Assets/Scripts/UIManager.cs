using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Transform lobbiesContainer;

    [SerializeField]
    GameObject lobbyItemPrefab;

    [SerializeField]
    GameObject noLobbiesText;

    [SerializeField]
    private AssetReference reference;

    bool lobbiesEmpty = true;
    bool gamesEmpty = true;

    void Start()
    {
        noLobbiesText.transform.GetChild(0).GetComponent<Image>().sprite = Addressables
            .LoadAssetAsync<Sprite>(reference)
            .Result;
        noLobbiesText.SetActive(lobbiesEmpty);
    }

    // Update is called once per frame
    void Update()
    {
        if (lobbiesEmpty && LobbyConnection.Instance.lobbiesList.Count > 0)
        {
            noLobbiesText.SetActive(false);
            GenerateList(LobbyConnection.Instance.lobbiesList, lobbyItemPrefab, lobbiesContainer);
            lobbiesEmpty = false;
        }
    }

    public void GenerateList(List<string> itemList, Object itemPrefab, Transform container)
    {
        itemList.Reverse();
        itemList.ForEach(el =>
        {
            GameObject item = (GameObject)Instantiate(itemPrefab, container);
            string lastCharactersInID = el.Substring(el.Length - 5);
            item.GetComponent<LobbiesListItem>().setId(el, lastCharactersInID);
        });
    }
}
