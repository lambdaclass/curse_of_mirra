using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

[System.Serializable]
public class AnimationStep
{
    [SerializeField]
    public float durationPercent; 

    [SerializeField]
    public AnimationClip animation;

    public bool triggersVfx;

    [MMCondition("triggersVfx", true)]
    public VfxStep vfxStep;
}
