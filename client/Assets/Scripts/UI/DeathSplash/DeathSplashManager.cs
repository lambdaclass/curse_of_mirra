using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class DeathSplashManager : MonoBehaviour
{
    [SerializeField]
    GameObject finalSplash;

    [SerializeField]
    TextMeshProUGUI rankingText;

    [SerializeField]
    TextMeshProUGUI amountOfKillsText;

    [SerializeField]
    GameObject characterModelContainer;

    [SerializeField]
    List<GameObject> characterModels;

    // Data to be added from front and back

    [SerializeField]
    TextMeshProUGUI defeaterPlayerName;

    [SerializeField]
    TextMeshProUGUI defeaterCharacterName;

    [SerializeField]
    Image defeaterImage;

    private const int WINNER_POS = 1;
    private const int SECOND_PLACE_POS = 2;
    GameObject player;
    GameObject modelClone;

    public void SetDeathSplashCharacter()
    {
        player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);
        GameObject characterModel = characterModels.Single(
            characterModel =>
                characterModel.name.Contains(
                    player.GetComponent<CustomCharacter>().CharacterModel.name
                )
        );
        modelClone = Instantiate(characterModel, characterModelContainer.transform);
    }

    void OnEnable()
    {
        ShowRankingDisplay();
        ShowMatchInfo();
        ShowCharacterAnimation();
    }

    void ShowRankingDisplay()
    {
        var ranking = GetRanking();
        rankingText.text += " # " + ranking.ToString();
    }

    private int GetRanking()
    {
        bool isWinner = SocketConnectionManager.Instance.PlayerIsWinner(
            SocketConnectionManager.Instance.playerId
        );

        // FIXME This is a temporal for the cases where the winner dies simultaneously
        // FIXME with other/s player/s
        if (isWinner)
        {
            return WINNER_POS;
        }
        if (Utils.GetAlivePlayers().Count() == 0)
        {
            return SECOND_PLACE_POS;
        }
        return Utils.GetAlivePlayers().Count() + 1;
    }

    void ShowMatchInfo()
    {
        // Kill count
        var killCount = GetKillCount();
        var killCountMessage = killCount == 1 ? " KILL" : " KILLS";
        amountOfKillsText.text = killCount.ToString() + killCountMessage;
        // Defeated By
        //defeaterPlayerName.text = GetDefeaterPlayerName();
        // Defeated By Image
        //defeaterImage.sprite = GetDefeaterSprite();
        // Defeated By Name
        //defeaterCharacterName.text = GetDefeaterCharacterName();
    }

    private ulong GetKillCount()
    {
        var playerId = SocketConnectionManager.Instance.playerId;
        var gamePlayer = Utils.GetGamePlayer(playerId);
        return gamePlayer.KillCount;
    }

    private string GetDefeaterPlayerName()
    {
        // TODO: get Defeater player name
        return "-";
    }

    private Sprite GetDefeaterSprite()
    {
        // TODO: get defeater sprite
        return null;
    }

    private string GetDefeaterCharacterName()
    {
        // TODO: get defeater character name
        return "-";
    }

    private void ShowCharacterAnimation()
    {
        if (player)
        {
            if (
                SocketConnectionManager.Instance.PlayerIsWinner(
                    SocketConnectionManager.Instance.playerId
                )
            )
            {
                modelClone.GetComponentInChildren<Animator>().SetBool("Victory", true);
            }
            else
            {
                modelClone.GetComponentInChildren<Animator>().SetBool("Defeat", true);
            }
        }
    }

    public void ShowEndGameScreen()
    {
        // TODO: get image from lobby
        finalSplash.SetActive(true);
    }
}
