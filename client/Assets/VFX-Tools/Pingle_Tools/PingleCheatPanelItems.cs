using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PingleCheatPanelItems : MonoBehaviour
{
    public GameObject golden_clock_vfx = null;
    public GameObject mirras_blessing_vfx = null;
    public GameObject magic_boots_vfx = null;
    public Transform compiler_root = null;

    public Shader new_character_shader = null;

    private List<Character> character_instances = null;
    private List<GameObject> pool = new List<GameObject>();
    private const float ARENA_SIZE = 40.0f;
    private HashSet<Material> mats = new HashSet<Material>();
    private Material[] mats_copy = null;

    public void init()
    {
        character_instances = FindObjectsOfType<Character>().ToList();
        clearPool();

        foreach( Renderer render in FindObjectsOfType<Renderer>() )
        {
          if ( render.sharedMaterial == null )
            continue;

          if ( render.sharedMaterial.shader == null )
            continue;

          if ( render.sharedMaterial.shader != new_character_shader )
            continue;

          mats.Add( render.sharedMaterial );
        }
        mats_copy = mats.ToArray();

        resetAnims();
    }

    private void resetAnims()
    {
        foreach(Material mat in mats )
            mat.SetFloat("_FresnelEffectAmount", 0);

        foreach(Character character in character_instances)
        {
            character.CharacterAnimator.ResetTrigger("Skill1");
            character.CharacterAnimator.ResetTrigger("Skill2");
            character.CharacterAnimator.ResetTrigger("Skill3");
            character.CharacterAnimator.SetTrigger("Walking");
            character.CharacterAnimator.ResetTrigger("Walking");
        }
    }

    private void clearPool()
    {
        foreach(GameObject go in pool)
            Destroy(go);

        pool.Clear();
    }

    private void OnGUI()
    {
        if ( GUI.Button(new Rect( 100, 100, 80, 80 ), "init") )
            init();

        if ( GUI.Button(new Rect( 200, 100, 80, 80 ), "golden_clock") )
        {
            activateItemVFX(golden_clock_vfx);
        }

        if ( GUI.Button(new Rect( 300, 100, 80, 80 ), "mirras_blessing") )
        {
            activateItemVFX(mirras_blessing_vfx);
        }

        if ( GUI.Button(new Rect( 400, 100, 80, 80 ), "magic_boots") )
        {
            activateItemVFX(magic_boots_vfx);
        }
    }

    private void activateItemVFX(GameObject vfx)
    {
        init();

        if ( vfx == null )
          return;

        GameObject cached_vfx = null;
        foreach(Character character in character_instances)
        {
            cached_vfx = Instantiate(vfx, character.transform);
            pool.Add( cached_vfx );
        }
    }

}