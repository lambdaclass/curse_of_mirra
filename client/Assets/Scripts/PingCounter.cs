using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Add this class to a gameObject with a Text component and it'll feed it the PING number in real time.
/// </summary>
public class PingCounter : MonoBehaviour
{
    /// the frequency at which the PING counter should update
    public float updateInterval = 5f;
    protected float _timeLeftToUpdate;
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
        _timeLeftToUpdate = updateInterval;
    }

    /// <summary>
    /// On Update, we decrease our time_left counter, and if we've reached zero, we update our PING counter
    /// with the last PING received
    /// </summary>
    protected virtual void Update()
    {
        _timeLeftToUpdate = _timeLeftToUpdate - Time.deltaTime;
        if (_timeLeftToUpdate <= 0.0)
        {
            _timeLeftToUpdate = updateInterval;
            _pingText.text = "PING " + SocketConnectionManager.Instance.currentPing.ToString() + " ms";
        }
    }
}
