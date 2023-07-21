using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectIndicator : MonoBehaviour
{
    [SerializeField]
    GameObject cone;

    [SerializeField]
    GameObject arrow;

    public void ActivateIndicator(UIIndicatorType indicatorType)
    {
        switch (indicatorType)
        {
            case UIIndicatorType.Cone:
                cone.SetActive(true);
                break;
            case UIIndicatorType.Arrow:
                arrow.SetActive(true);
                break;
        }
    }
}
