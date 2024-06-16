using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PoolSkill : MonoBehaviour
{
    [SerializeField]
    VisualEffect vfx;
    List<string> currentEffects = new List<string>();
    Coroutine effectCoroutine;

    private readonly Color ORIGINAL_COLOR_A = new Color(.3372549f, .172549f, .3803922f, 0f);
    private readonly Color ORIGINAL_COLOR_B = new Color(.2704863f, .8907394f, .8487674f, 0f);

    private readonly Color BUFFED_COLOR_A = new Color(1f, 0f, .68f, 0f);
    private readonly Color BUFFED_COLOR_B = new Color(.21f, .20f, .67f, 0f);

    public void TurnOff()
    {
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
        poolVFX.SetVector4("Color A", BUFFED_COLOR_A);
        poolVFX.SetVector4("Color B", BUFFED_COLOR_B);
        poolVFX.SetFloat("EffectDiameter", 11f);
        
        float oldDuration = poolVFX.GetFloat("Duration");
        poolVFX.SetFloat("Duration", oldDuration + durationAddition);

        yield return new WaitForSeconds(durationAddition);

        poolVFX.SetVector4("Color A", ORIGINAL_COLOR_A);
        poolVFX.SetVector4("Color B", ORIGINAL_COLOR_B);
        poolVFX.SetFloat("EffectDiameter", 10f);
    }
}
