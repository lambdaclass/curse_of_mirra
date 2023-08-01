using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDebugLog : MonoBehaviour
{
    public static void CustomLog(object message)
    {
        if (CustomLogs.allowCustomDebug)
        {
            Debug.Log(message);
        }
    }
}
