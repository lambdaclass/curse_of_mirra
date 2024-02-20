using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainAnimations : MonoBehaviour
{
    [SerializeField]
    List<CanvasGroup> thunders,
        lavas;

    [SerializeField]
    CanvasGroup fog;

    void Start()
    {
        foreach (CanvasGroup thunder in thunders)
        {
            Sequence thunderSequence = DOTween.Sequence();
            thunderSequence
                .Append(thunder.DOFade(1, 0.1f))
                .Append(thunder.DOFade(0, 0.1f))
                .Append(thunder.DOFade(1, 0.1f))
                .Append(thunder.DOFade(0, 0.5f))
                .AppendInterval(Random.Range(1, 3f))
                .SetLoops(-1, LoopType.Restart);
        }

        Sequence fogSequence = DOTween.Sequence();
        fogSequence
            .Append(fog.DOFade(1, 2f))
            .Append(fog.DOFade(0.5f, 2f))
            .SetLoops(-1, LoopType.Restart);

        foreach (CanvasGroup lava in lavas)
        {
            Sequence lavaSequence = DOTween.Sequence();
            lavaSequence
                .Append(lava.DOFade(1, .5f))
                .Append(lava.DOFade(0, 2f))
                .Append(lava.DOFade(1, 1f))
                .Append(lava.DOFade(0, 2f))
                .AppendInterval(Random.Range(2, 4))
                .SetLoops(-1, LoopType.Restart);
        }
    }
}
