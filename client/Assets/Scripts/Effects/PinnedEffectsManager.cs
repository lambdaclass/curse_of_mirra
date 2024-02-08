using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PinnedEffectsManager : MonoBehaviour
{
    [SerializeField] private List<PlacementHolderTransformPair> placeholder_transform_pairs = null;

    public Transform getTransformByPlaceholder(PlacementHolder look_up_placementholder)
    {
        Transform look_up_transform = null;

        if (look_up_placementholder == null)
            return look_up_transform;

        look_up_transform = placeholder_transform_pairs.FirstOrDefault(x => x.placement_holder == look_up_placementholder)?.look_up_transform;
        return look_up_transform;
    }
}

[Serializable]
public class PlacementHolderTransformPair
{
  /// Used to get look_up_transform by placement_holder
  [SerializeField] public PlacementHolder placement_holder = null;
  [SerializeField] public Transform look_up_transform = null;
}