using System;
using System.Collections.Generic;
using UnityEngine;

public class PinnedEffectsController : MonoBehaviour
{
    [SerializeField]
    private List<PlacementHolderEffectPair> placeholder_effect_pairs = null;

    private List<GameObject> spawned_objects = new List<GameObject>();

    private void Start()
    {
        Play(FindObjectOfType<PinnedEffectsManager>()); // needs better workaround, TODO: pass PinnedEffectsManager from CustomCharacter or so
    }

    private void OnDestroy()
    {
        ClearEffects();
    }

    public void Play(PinnedEffectsManager manager)
    {
        ClearEffects();

        if (manager == null)
            return;

        GameObject spawned_object = null;

        foreach (PlacementHolderEffectPair pair in placeholder_effect_pairs)
        {
            Transform spawn_transform = manager.GetTransformByPlaceholder(pair.placement_holder);
            if (spawn_transform == null)
                continue;

          if (pair.spawn_as_child)
              spawned_object = Instantiate(pair.effect, spawn_transform);
          else
              spawned_object = Instantiate(pair.effect, spawn_transform.position, spawn_transform.rotation);

          spawned_object.transform.localScale = spawn_transform.localScale;
          spawned_objects.Add(spawned_object);
      }
  }

    public void ClearEffects()
    {
        foreach (GameObject effect in spawned_objects)
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
    [SerializeField] public bool spawn_as_child = true;
}
