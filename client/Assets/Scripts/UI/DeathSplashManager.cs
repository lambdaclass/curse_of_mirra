using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

public class DeathSplashManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI rankingText;

    [SerializeField]
    TextMeshProUGUI messageText;

    [SerializeField]
    TextMeshProUGUI amountOfKillsText;

    [SerializeField]
    GameObject defeatedByContainer;

    [SerializeField]
    TextMeshProUGUI defeater;

    [SerializeField]
    Image defeaterImage;

    [SerializeField]
    TextMeshProUGUI defeaterName;

    [SerializeField]
    TextMeshProUGUI defeaterAbility;

    [SerializeField]
    GameObject playerModelContainer;

    private const string WINNER_MESSAGE = "THE KING OF ARABAN!";
    private const string LOSER_MESSAGE = "BETTER LUCK NEXT TIME!";

    void OnEnable()
    {
        // Ranking
        var ranking = GetRanking();
        rankingText.text = "# " + ranking.ToString();
        // Message
        var endGameMessage = SocketConnectionManager.Instance.PlayerIsWinner(
            LobbyConnection.Instance.playerId
        )
            ? WINNER_MESSAGE
            : LOSER_MESSAGE;
        messageText.text = endGameMessage;
        // Kill count
        var killCount = GetKillCount();
        var killCountMessage = killCount == 1 ? " KILL" : " KILLS";
        amountOfKillsText.text = killCount.ToString() + killCountMessage;
        // This conditional should be activated when the info needed is ready
        /* if (!PlayerIsWinner())
        {
            defeatedByContainer.SetActive(true);
        } */
        // Defeated By
        defeater.text = GetDefeater();
        // Defeated By Image
        defeaterImage.sprite = GetDefeaterSprite();
        // Defeated By Name
        defeaterName.text = GetDefeaterCharacter();
        // Defeated By Ability
        defeaterAbility.text = GetDefeaterAbility();
        // Player model
        SetPlayerPrefab();
    }

    private int GetRanking()
    {
        if (SocketConnectionManager.Instance.PlayerIsWinner(LobbyConnection.Instance.playerId))
        {
            return 1;
        }
        return Utils.GetAlivePlayers().Count() + 1;
    }

    private ulong GetKillCount()
    {
        var playerId = LobbyConnection.Instance.playerId;
        var gamePlayer = Utils.GetGamePlayer(playerId);
        return gamePlayer.KillCount;
    }

    private string GetDefeater()
    {
        // TODO: get Defeater
        return "-";
    }

    private Sprite GetDefeaterSprite()
    {
        // TODO: get defeater sprite
        return null;
    }

    private string GetDefeaterCharacter()
    {
        // TODO: get defeater character
        return "-";
    }

    private string GetDefeaterAbility()
    {
        // TODO: get defeater ability
        return "-";
    }

    private void SetPlayerPrefab()
    {
        GameObject player = Utils.GetPlayer(LobbyConnection.Instance.playerId);
        if (player)
        {
            GameObject model = player.GetComponent<Character>().CharacterModel;

            GameObject playerModel = Instantiate(
                model,
                playerModelContainer.transform.position,
                playerModelContainer.transform.rotation,
                playerModelContainer.transform
            );
            // TODO: get model sizes to make then look the same
            if (playerModel.name.Contains("H4ck"))
            {
                playerModel.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            if (playerModel.name.Contains("Muflus"))
            {
                playerModel.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            }
            if (playerModel.name.Contains("Dagna"))
            {
                playerModel.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            if (SocketConnectionManager.Instance.PlayerIsWinner(LobbyConnection.Instance.playerId))
            {
                playerModel.GetComponent<Animator>().SetBool("Victory", true);
            }
            else
            {
                playerModel.GetComponent<Animator>().SetBool("Defeat", true);
            }
        }
    }
}
