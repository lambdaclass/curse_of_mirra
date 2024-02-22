using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerCounter : MonoBehaviour
{
    [SerializeField] GameObject loadingIcon;

    // Start is called before the first frame update
    void Start()
    {
        LoadingAnimation();
    }

  void LoadingAnimation()
    {
        Sequence loadingSequence = DOTween.Sequence();
        loadingSequence
            .Append(loadingIcon.transform.DORotate(new Vector3(0, 0, -180), 0.5f))
            .Append(loadingIcon.transform.DORotate(new Vector3(0, 0, -360), 0.5f))
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.InOutQuart); 
    }
  
}
