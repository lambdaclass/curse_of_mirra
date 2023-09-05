using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using MoreMountains.TopDownEngine;

public class DeathSplashManager : MonoBehaviour
{
    [SerializeField]
    GameObject backgroundEndGame;

    [SerializeField]
    TextMeshProUGUI title;

    [SerializeField]
    TextMeshProUGUI winnerName;

    [SerializeField]
    TextMeshProUGUI winnerCharacter;

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
    GameObject player;
    GameObject playerModel;
    GameObject modelClone;

    public void SetDeathSplashPlayer()
    {
        player = Utils.GetPlayer(LobbyConnection.Instance.playerId);
        GameObject playerModel = player.GetComponent<Character>().CharacterModel;
        modelClone = Instantiate(
            playerModel,
            playerModelContainer.transform.position,
            playerModelContainer.transform.rotation,
            playerModelContainer.transform
        );
    }

    void OnEnable()
    {
        ShowRankingDisplay();
        ShowMessage();
        ShowMatchInfo();
        ShowPlayerAnimation();
        ShowEndGameScreen();
    }

    void ShowRankingDisplay()
    {
        var ranking = GetRanking();
        rankingText.text = "# " + ranking.ToString();
    }

    private int GetRanking()
    {
        if (SocketConnectionManager.Instance.PlayerIsWinner(LobbyConnection.Instance.playerId))
        {
            return 1;
        }
        return Utils.GetAlivePlayers().Count() + 1;
    }

    void ShowMessage()
    {
        var endGameMessage = SocketConnectionManager.Instance.PlayerIsWinner(
            LobbyConnection.Instance.playerId
        )
            ? WINNER_MESSAGE
            : LOSER_MESSAGE;
        messageText.text = endGameMessage;
    }

    void ShowMatchInfo()
    {
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

    private void ShowPlayerAnimation()
    {
        if (player)
        {
            List<SkinnedMeshRenderer> skinnedMeshFilter = new List<SkinnedMeshRenderer>();
            modelClone.GetComponentsInChildren(skinnedMeshFilter);
            foreach (var meshFilter in skinnedMeshFilter)
            {
                meshFilter.GetComponent<Renderer>().material.shader = Shader.Find(
                    "Universal Render Pipeline/Lit"
                );
            }
            for (int i = 0; i < modelClone.transform.childCount; i++)
            {
                Renderer renderer = modelClone.transform.GetChild(i).GetComponent<Renderer>();
                if (renderer)
                {
                    renderer.material.color = Color.white;
                }
            }
            // TODO: get model sizes to make them look the same
            if (modelClone.name.Contains("H4ck"))
            {
                modelClone.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            if (modelClone.name.Contains("Muflus"))
            {
                modelClone.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            }
            if (modelClone.name.Contains("Dagna"))
            {
                modelClone.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            if (SocketConnectionManager.Instance.PlayerIsWinner(LobbyConnection.Instance.playerId))
            {
                modelClone.GetComponent<Animator>().SetBool("Victory", true);
            }
            else
            {
                modelClone.GetComponent<Animator>().SetBool("Defeat", true);
            }
        }
    }

    private void ShowEndGameScreen()
    {
        // TODO: get image from lobby
        if (SocketConnectionManager.Instance.GameHasEnded())
        {
            backgroundEndGame.SetActive(true);
            // TODO: get player name
            winnerName.text =
                "Player " + SocketConnectionManager.Instance.winnerPlayer.Item1.Id.ToString();
            winnerCharacter.text = SocketConnectionManager
                .Instance
                .winnerPlayer
                .Item1
                .CharacterName;
            if (SocketConnectionManager.Instance.PlayerIsWinner(LobbyConnection.Instance.playerId))
            {
                title.text = "Victory";
            }
            else
            {
                title.text = "Defeat";
            }
        }
        else
        {
            backgroundEndGame.SetActive(false);
        }
    }
}
