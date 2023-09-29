using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        text = LobbyConnection.Instance.serverIp;
    }
}
