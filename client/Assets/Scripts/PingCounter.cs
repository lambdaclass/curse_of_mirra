using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Add this class to a gameObject with a Text component and it'll feed it the PING number in real time.
/// </summary>
public class PingCounter : MonoBehaviour
{
    protected Text _pingText;

    /// <summary>
    /// On Start(), we get the Text component and initialize our counter
    /// </summary>
    protected virtual void Start()
    {
        if (GetComponent<Text>() == null)
        {
            Debug.LogWarning("PINGCounter requires a GUIText component.");
            return;
        }
        _pingText = GetComponent<Text>();
    }

    /// <summary>
    /// On Update, we decrease our time_left counter, and if we've reached zero, we update our PING counter
    /// with the last PING received
    /// </summary>
    protected virtual void Update()
    {
        _pingText.text = "PING " + GameServerConnectionManager.Instance.currentPing;
    }
}
