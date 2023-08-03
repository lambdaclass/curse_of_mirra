using UnityEngine;

public class DeathSplashKillCount : MonoBehaviour
{
    private void OnEnable()
    {
        var killCount = GetKillCount();
        var killCountMessage = killCount == 1 ? " KILL" : " KILLS";
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text =
            killCount.ToString() + killCountMessage;
    }

    private ulong GetKillCount()
    {
        var playerId = LobbyConnection.Instance.playerId;
        var gamePlayer = Utils.GetGamePlayer(playerId);
        return gamePlayer.KillCount;
    }
}
