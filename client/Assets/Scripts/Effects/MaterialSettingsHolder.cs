using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialSettingsHolder", menuName = "ScriptableObject/MaterialSettingsHolder")]
public class MaterialSettingsHolder : ScriptableObject
{
    [SerializeField] public MaterialSettingsBlock[] blocks = null;

    public MaterialSettingsBlock getBlockByKey( MaterialSettingsKey key )
    {
      foreach( MaterialSettingsBlock block in blocks )
      {
          if( block.key != key )
              continue;

          return block;
      }

      return null;
    }
}
