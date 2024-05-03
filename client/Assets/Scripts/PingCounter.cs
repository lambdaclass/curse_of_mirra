using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Add this class to a gameObject with a Text component and it'll feed it the PING number in real time.
/// </summary>
public class PingCounter : MonoBehaviour
{
    protected Text pingText;
    private string pingValue;

    /// <summary>
    /// On Start(), we get the Text component and initialize our counter
    /// </summary>
    void Start()
    {
        pingText = GetComponent<Text>();
        pingValue = PingAnalyzer.Instance.pingValue;
        pingText.text = "PING " + pingValue;
    }

    /// <summary>
    /// On Update, we decrease our time_left counter, and if we've reached zero, we update our PING counter
    /// with the last PING received
    /// </summary>
    void Update()
    {
        pingValue = PingAnalyzer.Instance.pingValue;
        if (pingValue != pingText.text)
        {
            pingText.text = "PING " + pingValue;
        }
    }
}
