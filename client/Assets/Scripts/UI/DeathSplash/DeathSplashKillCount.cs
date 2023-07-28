using UnityEngine;

public class DeathSplashKillCount : MonoBehaviour
{
    private void Awake()
    {
        var killCount = GetKillCount();
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = killCount.ToString() + " KILLS";
    }

    private ulong GetKillCount()
    {
        var playerId = LobbyConnection.Instance.playerId;
        var gamePlayer = Utils.GetGamePlayer(playerId);
        return gamePlayer.KillCount;
    }
}
