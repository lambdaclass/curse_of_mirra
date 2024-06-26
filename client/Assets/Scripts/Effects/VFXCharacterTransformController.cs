using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXCharacterTransformController : MonoBehaviour
{
    [SerializeField] private AnimationCurve scale_curve = null;
    [SerializeField] private float scaling_duration = 1.0f;

    private IEnumerator transform_coroutine = null;
    private float cached_duration = 0;
    private Vector3 cached_vector = Vector3.zero;
    private const float DEFAULT_SCALE_DURATION = 5.0f;//Test case! Remove before merge

    void Start()
    {
        scaleCharacter();//Test case! Remove before merge
    }

    public void scaleCharacter(float duration = DEFAULT_SCALE_DURATION)
    {
        scaleCoroutine(duration);
    }

    public void resetScale()
    {
        if(transform_coroutine != null)
            StopCoroutine(transform_coroutine);
    }

    private void scaleCoroutine(float duration)
    {
        resetScale();

        transform_coroutine = impl();
        StartCoroutine(transform_coroutine);

        IEnumerator impl()
        {
            yield return scaleUp();
            //scaling up and down time is included to effect time
            yield return new WaitForSeconds(duration - scaling_duration - scaling_duration);
            yield return scaleDown();
        }
    }

    private IEnumerator scaleUp()
    {
        while(cached_duration < scaling_duration)
        {
          cached_vector.x = scale_curve.Evaluate(cached_duration / scaling_duration);
          cached_vector.y = cached_vector.x;
          cached_vector.z = cached_vector.x;

          transform.parent.transform.localScale = cached_vector;

          yield return null;
          cached_duration += Time.deltaTime;
        }
    }

    private IEnumerator scaleDown()
    {
        cached_duration = scaling_duration;
        while(cached_duration > 0)
        {
          cached_vector.x = scale_curve.Evaluate(cached_duration / scaling_duration);
          cached_vector.y = cached_vector.x;
          cached_vector.z = cached_vector.x;

          transform.parent.transform.localScale = cached_vector;

          yield return null;
          cached_duration -= Time.deltaTime;
        }
    }
}
