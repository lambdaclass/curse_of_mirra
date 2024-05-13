using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ConnectivityHealthCheck : MonoBehaviour
{
    CanvasGroup iconCanvas;
    float animationDuration = .4f;
    Coroutine displayCoroutine, hideCoroutine;
    Sequence pulseSequence;

    void Start()
    {
        iconCanvas = GetComponent<CanvasGroup>();
        pulseSequence = DOTween.Sequence();
        pulseSequence.Append(transform.DOScale(1.15f, animationDuration))
          .Append(transform.DOScale(1, animationDuration))
          .AppendInterval(1).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).Pause();
    }
    void Update()
    {
        if (LatencyAnalyzer.Instance.unstableConnection || LatencyAnalyzer.Instance.disconnect)
        {
            displayCoroutine = StartCoroutine(Display());
        }
    }
    IEnumerator Display()
    {
        pulseSequence.Play();
        iconCanvas.DOFade(1, animationDuration);
        yield return new WaitUntil(() => (!LatencyAnalyzer.Instance.unstableConnection && !LatencyAnalyzer.Instance.disconnect));
        StopCoroutine(displayCoroutine);
        hideCoroutine = StartCoroutine(Hide());
    }
    IEnumerator Hide()
    {
        pulseSequence.Pause();
        iconCanvas.DOFade(0, animationDuration);
        yield return new WaitForSeconds(1f);
        StopCoroutine(hideCoroutine);
    }
}
