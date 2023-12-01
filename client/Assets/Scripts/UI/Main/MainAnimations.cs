using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using MoreMountains.Tools;

public class MainAnimations : MonoBehaviour
{
    [SerializeField]
    CanvasGroup thunders;

    [SerializeField]
    GameObject firstRocksSet;

    [SerializeField]
    GameObject secondRocksSet;

    [SerializeField]
    GameObject thirdRocksSet;

    [SerializeField]
    GameObject fourthRocksSet;

    void Start()
    {
        Sequence thunderSequence = DOTween.Sequence();
        thunderSequence
            .AppendInterval(1)
            .Append(thunders.DOFade(1, 0.1f))
            .Append(thunders.DOFade(0, 0.25f))
            .Append(thunders.DOFade(1, 0.1f))
            .AppendInterval(Random.Range(0.2f, 1))
            .Append(thunders.DOFade(0, 0.25f))
            .AppendInterval(Random.Range(1, 4))
            .SetLoops(-1, LoopType.Restart);

        Sequence firstRocksSequence = DOTween.Sequence();
        firstRocksSequence
            .Append(
                firstRocksSet.transform.DOLocalMoveY(
                    GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta.y,
                    10,
                    false
                )
            )
            .SetLoops(-1, LoopType.Restart);
        Sequence secondRocksSequence = DOTween.Sequence();
        secondRocksSequence
            .AppendInterval(5)
            .Append(
                secondRocksSet.transform.DOLocalMoveY(
                    GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta.y,
                    8,
                    false
                )
            )
            .SetLoops(-1, LoopType.Restart);
        Sequence thirdRocksSequence = DOTween.Sequence();
        thirdRocksSequence
            .AppendInterval(6)
            .Append(
                thirdRocksSet.transform.DOLocalMoveY(
                    GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta.y,
                    7,
                    false
                )
            )
            .SetLoops(-1, LoopType.Restart);
        Sequence fourthRocksSequence = DOTween.Sequence();
        fourthRocksSequence
            .AppendInterval(7)
            .Append(
                fourthRocksSet.transform.DOLocalMoveY(
                    GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta.y,
                    9,
                    false
                )
            )
            .SetLoops(-1, LoopType.Restart);
    }
}
