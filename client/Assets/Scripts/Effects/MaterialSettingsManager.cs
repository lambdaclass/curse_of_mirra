using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSettingsManager : MonoBehaviour
{
    [SerializeField] private MaterialSettingsHolder holder = null;

     public MaterialSettingsBlock getBlockByKey( MaterialSettingsKey key )
     {
        return holder.getBlockByKey( key );
     }
}
