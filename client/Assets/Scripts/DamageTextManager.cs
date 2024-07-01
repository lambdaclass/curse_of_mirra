using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageTextManager : MonoBehaviour
{
    public float timeDestroy = 1f;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] CanvasGroup canvasGroup;
    void Start()
    {
        StartCoroutine(StartDamageText());
    }

    IEnumerator StartDamageText() {
        yield return StartCoroutine(StartAnimation());
        yield return new WaitForSeconds(2f); 
        Destroy(gameObject);
    }

    IEnumerator StartAnimation() {
        rectTransform.localScale = Vector3.one; 
        rectTransform.DOScale(0.8f, .5f); 
        canvasGroup.DOFade(1f, .2f);
        yield return new WaitForSeconds(.5f);
    }
}
