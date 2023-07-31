using UnityEngine;
using UnityEngine.UI;

public class VersionsHashes : MonoBehaviour
{
    public bool serverHash;

    public void Start()
    {
        if (GetComponent<Text>() == null)
        {
            Debug.LogWarning("VersionsHashes requires a GUIText component.");
            return;
        }

        string hash = serverHash
            ? "Server #" + SocketConnectionManager.Instance.serverHash
            : "Client #" + GitInfo.GetGitHash();
        GetComponent<Text>().text = hash;
    }
}
