using UnityEngine;
using System.Linq;

public class DeathSplashEndMessage : MonoBehaviour
{
    private const string WINNER_MESSAGE = "THE KING OF ARABAN!";
    private const string LOSER_MESSAGE = "BETTER LUCK NEXT TIME!";

    private void Awake()
    {
        var endMessage = Utils.GetAlivePlayers().Count() == 1 ? WINNER_MESSAGE : LOSER_MESSAGE;
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = endMessage;
    }
}
