using System;
using System.Collections.Generic;
using UnityEngine;

public class PinnedEffectsController : MonoBehaviour
{
  [SerializeField] private List<PlacementHolderEffectPair> placeholder_effect_pairs = null;

  private List<GameObject> spawned_objects = new List<GameObject>();

  private void Start()
  {
      play(FindObjectOfType<PinnedEffectsManager>()); // needs better workaround, TODO: pass PinnedEffectsManager from CustomCharacter or so
  }

  private void OnDestroy()
  {
      clearEffects();
  }

  public void play(PinnedEffectsManager manager)
  {
      clearEffects();

      if (manager == null)
          return;

      GameObject spawned_object = null;

      foreach(PlacementHolderEffectPair pair in placeholder_effect_pairs)
      {
          Transform spawn_transform = manager.getTransformByPlaceholder(pair.placement_holder);
          if(spawn_transform == null)
              continue;

          spawned_object = Instantiate(pair.effect, spawn_transform);

          spawned_objects.Add(spawned_object);
      }
  }

  public void clearEffects()
  {
      foreach(GameObject effect in spawned_objects)
          Destroy(effect);

      spawned_objects.Clear();
  }
}

[Serializable]
public class PlacementHolderEffectPair
{
    ///Used to spawn GameObject effect into transform geted by PinnedEffectsManager.getTransformByPlaceholder(placement_holder)
    [SerializeField] public PlacementHolder placement_holder = null;
    [SerializeField] public GameObject effect = null;
}