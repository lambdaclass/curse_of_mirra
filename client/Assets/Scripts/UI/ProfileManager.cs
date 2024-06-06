using TMPro; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI playerName;

    void Start()
    {
        playerName.text = PlayerPrefs.GetString("playerName");
    }
}
