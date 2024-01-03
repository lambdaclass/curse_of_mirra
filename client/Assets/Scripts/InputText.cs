using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputText : MonoBehaviour
{
    public static string text;

    [SerializeField]
    TMP_InputField inputField;

    void Start()
    {
        inputField.text = text;
    }

    public void Init()
    {
        text = ServerConnection.Instance.serverIp;
    }
}
