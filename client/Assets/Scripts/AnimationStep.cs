using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

[System.Serializable]
public class AnimationStep
{
    [SerializeField]
    public float durationPercent;
    public bool triggersVfx;

    [MMCondition("triggersVfx", true)]
    public VfxStep vfxStep;
}
