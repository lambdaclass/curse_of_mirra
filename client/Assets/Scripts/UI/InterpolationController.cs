using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterpolationController : MonoBehaviour
{
    private Slider deltaInterpolationSlider;

    [SerializeField]
    private TextMeshProUGUI interpolationText;

    void Awake()
    {
        deltaInterpolationSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    public void changeDeltaInterpolationTime()
    {
        interpolationText.text =
            "Interpolation Time: " + ((long)deltaInterpolationSlider.value).ToString() + "ms";
        SocketConnectionManager.Instance.eventsBuffer.deltaInterpolationTime = (long)
            deltaInterpolationSlider.value;
    }
}
