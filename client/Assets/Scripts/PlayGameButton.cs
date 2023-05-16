using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayGameButton playGameButton;

    void Start()
    {
        if (LobbyConnection.Instance.playerId == 1)
        {
            playGameButton = GetComponent<PlayGameButton>();
            playGameButton.gameObject.SetActive(true);
        }
        else
        {
            playGameButton = GetComponent<PlayGameButton>();
            playGameButton.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
}
