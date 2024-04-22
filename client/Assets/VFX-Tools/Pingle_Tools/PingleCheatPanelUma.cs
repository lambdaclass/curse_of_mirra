using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PingleCheatPanelUma : MonoBehaviour
{
    public GameObject skill1_vfx = null;
    public Transform skill1_vfx_root = null;

    public GameObject skill2_vfx = null;
    public Transform skill2_vfx_root = null;
    public GameObject skill2_vfx_floor = null;
    public Renderer character_renderer = null;
    public Transform spawn_point = null;

    public GameObject dash_vfx = null;
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

        character_renderer.enabled = true;
        character_instance.transform.position = spawn_point.position;
    }

    private void resetAnims()
    {
        character_instance.CharacterAnimator.ResetTrigger("Skill1");
        character_instance.CharacterAnimator.ResetTrigger("Skill2");
        character_instance.CharacterAnimator.ResetTrigger("Skill3");
        character_instance.CharacterAnimator.SetTrigger("Walking");
        character_instance.CharacterAnimator.ResetTrigger("Walking");
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

        if ( GUI.Button(new Rect( 200, 100, 80, 80 ), "Skill_1") )
            activateSkill1();

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
        cached_vfx = Instantiate(skill1_vfx, skill1_vfx_root);
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
        character_instance.CharacterAnimator.SetTrigger("Skill2");

        yield return new WaitForSeconds(0.2f);

        GameObject cached_vfx = null;

        if( skill2_vfx != null )
            cached_vfx = Instantiate(skill2_vfx, skill2_vfx_root.position, skill2_vfx_root.rotation);

        pool.Add( cached_vfx );

        if( skill2_vfx_floor != null )
            cached_vfx = Instantiate(skill2_vfx_floor, character_instance.transform);

        pool.Add( cached_vfx );

        yield return new WaitForSeconds(0.1f);
        //character_renderer.enabled = false;

        yield return new WaitForSeconds(1.2f);
        character_instance.CharacterAnimator.ResetTrigger("Skill2");
        yield return new WaitForSeconds(1.2f);

        //character_renderer.enabled = true;
        clearPool();
    }

    private void activateSkill3()
    {
        resetAnims();
        StartCoroutine(skill3());
    }

    private IEnumerator skill3()
    {
        GameObject spawned_dash_vfx = null;

        character_instance.CharacterAnimator.SetTrigger("Skill3");

        yield return new WaitForSeconds(skill3_start_delay);

        if(dash_vfx != null)
            spawned_dash_vfx = Instantiate(dash_vfx, character_instance.transform);

        Vector3 new_pos = character_instance.transform.position;

        for(int i = 0; i < skill3_duration; i++)
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

        Destroy(spawned_dash_vfx);
        character_instance.CharacterAnimator.ResetTrigger("Walking");
    }
}