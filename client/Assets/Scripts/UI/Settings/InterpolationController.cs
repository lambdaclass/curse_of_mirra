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
        long value = (long)(deltaInterpolationSlider.value * 100);
        interpolationText.text = value.ToString() + " ms";
        GameServerConnectionManager.Instance.eventsBuffer.deltaInterpolationTime = value;
    }
}
