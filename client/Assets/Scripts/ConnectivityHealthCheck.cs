using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ConnectivityHealthCheck : MonoBehaviour
{
    CanvasGroup iconCanvas;
    public float animationDuration = 1f;
    Coroutine displayCoroutine, hideCoroutine;

    void Start()
    {
        iconCanvas = GetComponent<CanvasGroup>();
        transform.DOScale(1.2f, animationDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    void Update()
    {
        if (PingAnalyzer.Instance.unstableConnection)
        {
            displayCoroutine = StartCoroutine(Display());
        }
    }
    IEnumerator Display()
    {
        iconCanvas.DOFade(1, animationDuration);
        yield return new WaitUntil(() => !PingAnalyzer.Instance.unstableConnection);
        StopCoroutine(displayCoroutine);
        hideCoroutine = StartCoroutine(Hide());
    }
    IEnumerator Hide()
    {
        iconCanvas.DOFade(0, animationDuration);
        yield return new WaitForSeconds(1f);
        StopCoroutine(hideCoroutine);
    }
}
