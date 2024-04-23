using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class SafeZone : MonoBehaviour
{
    [SerializeField]
    Renderer dangerZone;

    void Start()
    {
        dangerZone.sharedMaterial.SetFloat("_AlphaMultiplier", 0f);
    }

    IEnumerator ShowDangerZone()
    {
        yield return new WaitForSeconds(1f);
        float currentValue = dangerZone.sharedMaterial.GetFloat("_AlphaMultiplier");
        if (currentValue == 0.70f)
            yield break;
        currentValue += 0.05f;
        dangerZone
            .sharedMaterial
            .SetFloat("_AlphaMultiplier", Mathf.Round(currentValue * 100) / 100f);
    }

    void Update()
    {
        // We have a difference of x100 between the backend and frontend values
        float radius = GameServerConnectionManager.Instance.playableRadius / 115;
        bool isZoneEnabled = GameServerConnectionManager.Instance.zoneEnabled;

        if (isZoneEnabled && dangerZone.sharedMaterial.GetFloat("_AlphaMultiplier") != 0.70f)
        {
            StartCoroutine(ShowDangerZone());
        }
        // The vfx goes to 0 to 1.
        // The raidus from the backen goes from 0.552 to 0

        double finalVfxValue = 1;
        double initialPlayableRadius = 0.552;
        float currentRadius = radius / 115;
        double value = finalVfxValue + (currentRadius) * (-finalVfxValue) / (initialPlayableRadius);

        dangerZone.sharedMaterial.SetFloat("_Progress", (float)value);
    }
}
