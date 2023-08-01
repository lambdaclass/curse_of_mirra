using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class CustomLogs : MonoBehaviour
{
    [SerializeField]
    int maxLines = 50;

    [SerializeField]
    TextMeshProUGUI debugLogText;

    Queue<string> queue = new Queue<string>();

    public bool debugPrint = false;
    public static bool allowCustomDebug = false;

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        //if we want a max messages length descomment this :)
        // if (queue.Count >= maxLines || !debugPrint)
        // {
        //     queue.Dequeue();
        //     debugLogText.text = " ";
        // }
        if (debugPrint)
        {
            queue.Enqueue(logString);

            var builder = new StringBuilder();
            foreach (string st in queue)
            {
                builder.Append(type.ToString() + ": ").Append(st).Append("\n");
            }

            debugLogText.text = builder.ToString();
        }
    }

    public void SetDebugPrint()
    {
        debugPrint = !debugPrint;
        this.gameObject.SetActive(debugPrint);
    }

    public void UseCustomLogs()
    {
        allowCustomDebug = !allowCustomDebug;
    }
}
