using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDebugLog : MonoBehaviour
{
    public static void CustomDebug(object message)
    {
        if (CustomLogs.allowCustomDebug)
        {
            Debug.Log(message);
        }
    }
}
