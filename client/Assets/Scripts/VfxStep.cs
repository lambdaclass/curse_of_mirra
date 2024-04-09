using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VfxStep
{
    [SerializeField]
    public GameObject vfx;

    [SerializeField]
    public float duration;

    [SerializeField]
    public float delay;

    [SerializeField]
    public bool instantiateVfxOnModel;

    [SerializeField]
    public bool hasDestination;
}
