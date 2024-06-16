using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PoolSkill : MonoBehaviour
{
    private SkillInfo skillInfo;

    [SerializeField]
    VisualEffect vfx;
    List<string> currentEffects = new List<string>();
    Coroutine effectCoroutine;

    private readonly Color ORIGINAL_COLOR_A = new Color(.3372549f, .172549f, .3803922f, 0f);
    private readonly Color ORIGINAL_COLOR_B = new Color(.2704863f, .8907394f, .8487674f, 0f);

    private readonly Color BUFFED_COLOR_A = new Color(1f, 0f, .68f, 0f);
    private readonly Color BUFFED_COLOR_B = new Color(.21f, .20f, .67f, 0f);

    private const float ORIGINAL_DURATION = 5f;
    private float originalEffectDiameter;

    public void Initialize(SkillInfo skillInfo, Vector3 initialPosition, float radius)
    {
        this.skillInfo = skillInfo;

        originalEffectDiameter = radius * 2;
        if(vfx.HasFloat("EffectDiameter"))
        {
            vfx.SetFloat("EffectDiameter", originalEffectDiameter);
        }
        else if(vfx.HasFloat("EffectRadius"))
        {
            vfx.SetFloat("EffectRadius", radius);
        }
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        if(skillInfo.name == "SINGULARITY")
        {
            RestartParameterValues(vfx);
        }
        currentEffects.Clear();
        gameObject.SetActive(false);
    }

    public void HandlePoolEffects(RepeatedField<Effect> effects)
    {
        Dictionary<string, int> newEffectsInPool = new Dictionary<string, int>();
            
        // First iteration, hardcoded to work only with Valtimer's ultimate
        foreach(string effectName in effects
                                        .Where(effect => effect.Name == "buff_singularity")
                                        .Select(effect => effect.Name)
                                    )
        {
            if (newEffectsInPool.ContainsKey(effectName))
            {
                newEffectsInPool[effectName]++;
            }
            else
            {
                newEffectsInPool[effectName] = 1;
            }
        }

        foreach(string existingEffect in currentEffects)
        {
            if(newEffectsInPool[existingEffect] == 1)
            {
                newEffectsInPool.Remove(existingEffect);
            }
            else
            {
                newEffectsInPool[existingEffect]--;
            }
        }

        foreach(string newEffect in newEffectsInPool.Keys)
        {
            currentEffects.Add(newEffect);

            if(effectCoroutine != null)
            {
                StopCoroutine(effectCoroutine);
            }
            
            effectCoroutine = StartCoroutine(BuffSingularity(vfx, 1f));
        }
    }

    private IEnumerator BuffSingularity(VisualEffect poolVFX, float durationAddition)
    {
        Color colorABeforeBuff = poolVFX.GetVector4("Color A");
        Color colorBBeforeBuff = poolVFX.GetVector4("Color B");
        float diameterBeforeBuff = poolVFX.GetFloat("EffectDiameter");
        float originalDuration = poolVFX.GetFloat("Duration");

        Color buffedColorA = BUFFED_COLOR_A;
        Color buffedColorB = BUFFED_COLOR_B;
        float buffedEffectDiameter = originalEffectDiameter + 1f;
        float newDuration = originalDuration + durationAddition;

        float transitionTime = durationAddition / 2;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionTime;

            poolVFX.SetVector4("Color A", Color.Lerp(colorABeforeBuff, buffedColorA, t));
            poolVFX.SetVector4("Color B", Color.Lerp(colorBBeforeBuff, buffedColorB, t));
            poolVFX.SetFloat("EffectDiameter", Mathf.Lerp(diameterBeforeBuff, buffedEffectDiameter, t));
            poolVFX.SetFloat("Duration", Mathf.Lerp(originalDuration, newDuration, t));

            yield return null;
        }

        poolVFX.SetVector4("Color A", buffedColorA);
        poolVFX.SetVector4("Color B", buffedColorB);
        poolVFX.SetFloat("EffectDiameter", buffedEffectDiameter);
        poolVFX.SetFloat("Duration", newDuration);

        elapsedTime = 0f;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionTime;

            poolVFX.SetVector4("Color A", Color.Lerp(buffedColorA, ORIGINAL_COLOR_A, t));
            poolVFX.SetVector4("Color B", Color.Lerp(buffedColorB, ORIGINAL_COLOR_B, t));
            poolVFX.SetFloat("EffectDiameter", Mathf.Lerp(buffedEffectDiameter, originalEffectDiameter, t));

            yield return null;
        }
    }

    // This only works for Valtimer's ultimate since this properties are specific to that VFX. That's why it's only called when the skill's name is "SINGULARITY"
    private void RestartParameterValues(VisualEffect poolVFX)
    {
        poolVFX.SetVector4("Color A", ORIGINAL_COLOR_A);
        poolVFX.SetVector4("Color B", ORIGINAL_COLOR_B);
        poolVFX.SetFloat("EffectDiameter", originalEffectDiameter);
        poolVFX.SetFloat("Duration", ORIGINAL_DURATION);
    }
}
