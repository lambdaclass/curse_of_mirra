using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputText : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<InputField>().text = LobbyConnection.Instance.server_ip;
    }
}
