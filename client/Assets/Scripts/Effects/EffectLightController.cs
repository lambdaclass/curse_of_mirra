using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLightController : MonoBehaviour
{
    [SerializeField] private Light light = null;
    [SerializeField] private AnimationCurve light_intencity = null;
    [SerializeField] private Gradient light_color = null;
    [SerializeField] private float life_duration = 1.0f;

    private float lifetime_passed = 0.0f;
    private float life_percent = 0.0f;

    void Start()
    {
        lifetime_passed = 0.0f;
        life_percent = 0.0f;

        StartCoroutine(lightLifeProcces());
    }

    private IEnumerator lightLifeProcces()
    {
        while (lifetime_passed < life_duration)
        {
            life_percent = lifetime_passed / life_duration;

            light.intensity = light_intencity.Evaluate(life_percent);
            light.color = light_color.Evaluate(life_percent);

            yield return null;
            lifetime_passed += Time.deltaTime;
        }
        light.enabled = false;
    } 
}
