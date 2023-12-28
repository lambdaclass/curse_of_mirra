using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EndGameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject finalSplash;

    [SerializeField]
    public TextMeshProUGUI rankingText;
    private const int WINNER_POS = 1;
    private const int SECOND_PLACE_POS = 2;

    void OnEnable()
    {
        ShowRankingDisplay();
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
}
