using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;

public class DisplayPlayerName : MonoBehaviour
{
    [SerializeField]
    CustomCharacter character;

    void Start()
    {
        // GetComponent<TextMeshPro>().text = "Player " + character.PlayerID;
        Debug.Log(Utils.GetGamePlayer((ulong)decimal.Parse(character.PlayerID)));
        GetComponent<TextMeshPro>().text = LobbyConnection.Instance.playersIdName[
            (ulong)decimal.Parse(character.PlayerID)
        ];
    }

    void Update()
    {
        bool isAlive = character.GetComponent<Health>().CurrentHealth > 0;
        this.gameObject.SetActive(isAlive);
    }
}
