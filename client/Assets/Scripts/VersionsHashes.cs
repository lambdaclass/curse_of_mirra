using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VersionsHashes : MonoBehaviour
{
	public void Start()
	{
		if(GetComponent<Text>() == null)
		{
			Debug.LogWarning ("VersionsHashes requires a GUIText component.");
			return;
		}

        string server_hash = "Server hash \t" + SocketConnectionManager.Instance.serverHash;
        string client_hash = "Client hash \t" + GitInfo.GetGitHash();
		GetComponent<Text>().text = server_hash + "\n" + client_hash;
	}

	public void Update()
	{
	}
}
