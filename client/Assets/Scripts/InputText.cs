using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputText : MonoBehaviour
{
    public static string text;
    [SerializeField] InputField inputField;

    void Start()
    {
        inputField.text = text;
    }
    public void Init()
    {
        text = LobbyConnection.Instance.server_ip;
    }
}
