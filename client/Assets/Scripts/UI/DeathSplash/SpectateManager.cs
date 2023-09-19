using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SpectateManager : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    GameObject dataSplashContainer;

    [SerializeField]
    GameObject exitButton;

    [SerializeField]
    GameObject spectateTextIndicator;

    public void UnsetSpectateMode()
    {
        if (SocketConnectionManager.Instance.GameHasEnded())
        {
            dataSplashContainer.SetActive(true);
            exitButton.SetActive(false);
            spectateTextIndicator.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dataSplashContainer.SetActive(false);
        exitButton.SetActive(true);
        spectateTextIndicator.SetActive(true);
    }
}
