using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class CustomGUIManager : MonoBehaviour
{
    const float MIN_VIGNETTE_INTENSITY = .3f;
    const float MAX_VIGNETTE_INTENSITY = .4f;
    const float MIN_ZONE_TEXTURE_INTENSITY = .0f;
    const float MAX_ZONE_TEXTURE_INTENSITY = .3f;

    [SerializeField]
    Volume volume;

    [SerializeField]
    Image zoneDamageFeedbackContainer;

    [SerializeField]
    Image zoneDamageFeedbackTextue;

    Vignette vignette;
    bool playerInZone = false;
    bool zoneDamageFeedbackisFaded = true;
    float fadeDuration = .5f;
    float zoneContainerOpacity = 0;
    float lastVignetteOpacity = 0;

    void Start()
    {
        volume.profile.TryGet(out vignette);
    }

    void Update()
    {
        if (playerInZone)
        {
            float pingpong = Mathf.PingPong((Time.time), 1);

            UpdateVignette(pingpong);
            UpdateZoneFeedbackTexture(pingpong);
        }
        else if (vignette.intensity.value > 0)
        {
            vignette.intensity.value = Mathf.Min(lastVignetteOpacity, zoneContainerOpacity / 2);
        }
    }

    private void UpdateVignette(float value)
    {
        vignette.intensity.value = Mathf.Lerp(
            MIN_VIGNETTE_INTENSITY,
            MAX_VIGNETTE_INTENSITY,
            value * zoneContainerOpacity
        );
        lastVignetteOpacity = vignette.intensity.value;
    }

    private void UpdateZoneFeedbackTexture(float value)
    {
        var tempColor = zoneDamageFeedbackTextue.color;
        tempColor.a = Mathf.Lerp(MIN_ZONE_TEXTURE_INTENSITY, MAX_ZONE_TEXTURE_INTENSITY, value);
        zoneDamageFeedbackTextue.color = tempColor;
    }

    public void DisplayZoneDamageFeedback(bool value)
    {
        if (playerInZone != value)
        {
            ToggleZoneFeedback();
            playerInZone = value;
        }
    }

    public void ToggleZoneFeedback()
    {
        var canvGroup = zoneDamageFeedbackContainer.gameObject.GetComponent<CanvasGroup>();
        StartCoroutine(DoFade(canvGroup, canvGroup.alpha, zoneDamageFeedbackisFaded ? 1 : 0));
        zoneDamageFeedbackisFaded = !zoneDamageFeedbackisFaded;
    }

    public IEnumerator DoFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;
        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            zoneContainerOpacity = Mathf.Lerp(start, end, counter / fadeDuration);
            canvGroup.alpha = zoneContainerOpacity;

            yield return null;
        }
    }
}
