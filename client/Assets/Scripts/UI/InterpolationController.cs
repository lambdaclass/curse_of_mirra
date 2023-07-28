using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterpolationController : MonoBehaviour
{
    private Slider deltaInterpolationSlider;

    void Awake()
    {
        deltaInterpolationSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    public void changeDeltaInterpolationTime()
    {
        SocketConnectionManager.Instance.eventsBuffer.deltaInterpolationTime = (long)
            deltaInterpolationSlider.value;
    }
}
