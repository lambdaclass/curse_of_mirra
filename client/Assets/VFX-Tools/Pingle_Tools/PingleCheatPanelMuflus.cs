using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PingleCheatPanelMuflus : MonoBehaviour
{
    public Transform spawn_point = null;
    public GameObject skill1_vfx = null;

    public GameObject skill2_vfx_init = null;
    public GameObject skill2_vfx_trail = null;
    public Transform skill2_vfx_trail_root = null;
    public GameObject skill2_vfx_final = null;
    public int fly_duration = 120;
    public float fly_distance = 10.0f;
    public AnimationCurve fly_x_curve = null;

    public GameObject dash_vfx = null;
    public Transform dash_vfx_root = null;
    public int skill3_duration = 80;
    public float skill3_speed = 7.0f;
    public float skill3_start_delay = 0.2f;

    private Character character_instance = null;
    private List<GameObject> pool = new List<GameObject>();
    private const float ARENA_SIZE = 40.0f;

    public void init()
    {
        Application.targetFrameRate = 60;
        character_instance = FindObjectOfType<Character>();
        resetAnims();
        character_instance.transform.position = spawn_point == null ? Vector3.zero : spawn_point.position;
    }

    private void resetAnims()
    {
        character_instance.CharacterAnimator.ResetTrigger("Skill1");
        character_instance.CharacterAnimator.ResetTrigger("Skill2");
        character_instance.CharacterAnimator.ResetTrigger("Skill3");
        character_instance.CharacterAnimator.SetTrigger("Walking");
        character_instance.CharacterAnimator.ResetTrigger("Walking");
    }

    private void OnGUI()
    {
        if ( GUI.Button(new Rect( 100, 100, 80, 80 ), "init") )
            init();

        if ( GUI.Button(new Rect( 200, 100, 80, 80 ), "Skill_1") )
            return;

        if ( GUI.Button(new Rect( 300, 100, 80, 80 ), "Skill_2") )
            activateSkill2();

        if ( GUI.Button(new Rect( 400, 100, 80, 80 ), "Skill_3") )
            activateSkill3();
    }

    private void activateSkill1()
    {
        resetAnims();
        clearPool();
        character_instance.CharacterAnimator.SetTrigger("Skill1");

        GameObject cached_vfx = null;
        cached_vfx = Instantiate( skill1_vfx );
        pool.Add( cached_vfx );
    }

    private void activateSkill2()
    {
        resetAnims();
        clearPool();
        StartCoroutine(skill2());
    }


    private IEnumerator skill2()
    {
        GameObject cached_vfx = null;
        character_instance.CharacterAnimator.SetTrigger("Skill2");

        if (skill2_vfx_init != null)
        {
            cached_vfx = Instantiate(skill2_vfx_init, character_instance.transform.position, character_instance.transform.rotation);
            pool.Add(cached_vfx);
        }

        if (skill2_vfx_trail != null)
        {
            cached_vfx = Instantiate(skill2_vfx_trail, skill2_vfx_trail_root);
            pool.Add( cached_vfx );
        }

        Vector3 old_pos = character_instance.transform.position;
        yield return new WaitForSeconds(0.3f);

        for(int i = 0; i < fly_duration; i++)
        {
            Vector3 new_pos = character_instance.transform.position;
            new_pos.x = Mathf.Lerp(old_pos.x, old_pos.x + fly_distance, fly_x_curve.Evaluate((float)i / (float)fly_duration));

            character_instance.transform.position = new_pos;
            yield return null;
        }

        character_instance.CharacterAnimator.ResetTrigger("Skill2");

        yield return new WaitForSeconds(0.3f);

        if (skill2_vfx_final != null)
        {
            cached_vfx = Instantiate(skill2_vfx_final, dash_vfx_root.transform.position, dash_vfx_root.transform.rotation);
            pool.Add(cached_vfx);
        }

      yield return new WaitForSeconds(3.2f);
      clearPool();
    }

    private void clearPool()
    {
        foreach(GameObject go in pool)
            Destroy(go);

        pool.Clear();
    }

    private void activateSkill3()
    {
        resetAnims();
        StartCoroutine(skill3());
    }

    private IEnumerator skill3()
    {
        GameObject spawned_dash_vfx = null;

        if(dash_vfx != null)
        {
            spawned_dash_vfx = Instantiate(dash_vfx, dash_vfx_root.transform);
            spawned_dash_vfx.transform.rotation = character_instance.transform.rotation;

        }

        character_instance.CharacterAnimator.SetTrigger("Skill3");

        yield return new WaitForSeconds(skill3_start_delay);
        Vector3 new_pos = character_instance.transform.position;

        for( int i = 0; i < skill3_duration; i++ )
        {
            new_pos = character_instance.transform.position;
            new_pos.x += skill3_speed * Time.deltaTime;

            if (new_pos.x > ARENA_SIZE || new_pos.x < -ARENA_SIZE)
                continue;

            character_instance.transform.position = new_pos;
            yield return null;
        }

        resetAnims();
        character_instance.CharacterAnimator.SetTrigger("Walking");
        yield return new WaitForSeconds(1.0f);

        character_instance.CharacterAnimator.ResetTrigger("Walking");
        Destroy(spawned_dash_vfx);
    }
}