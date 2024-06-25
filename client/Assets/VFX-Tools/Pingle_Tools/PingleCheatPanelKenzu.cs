using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PingleCheatPanelKenzu : MonoBehaviour
{
    [Range(0.1f, 2f)]public float time_scale = 1.0f;
    public Transform spawn_point = null;

    public GameObject skill1_vfx_1 = null;
    public GameObject skill1_vfx_2 = null;
    public GameObject skill1_vfx_3 = null;
    public Transform  skill1_vfx_root = null;
    public float skill1_duration_1 = 1.0f;
    public float skill1_duration_2 = 1.0f;
    public float skill1_duration_3 = 1.0f;

    public GameObject skill2_vfx = null;
    public Transform  skill2_vfx_root = null;
    public float skill2_duration = 2.0f;

    public GameObject skill3_vfx_start = null;
    public GameObject skill3_vfx_block = null;
    public GameObject skill3_vfx_attack = null;
    public Transform  skill3_vfx_root = null;
    public GameObject skill3_vfx_sword = null;
    public Transform  skill3_vfx_sword_root = null;
    public float skill3_duration = 5.0f;
    public float skill3_attack_duration = 1.0f;

    public Character character_instance = null;
    private List<GameObject> pool = new List<GameObject>();
    private const float ARENA_SIZE = 40.0f;
    private IEnumerator running_cor = null;

    public void init()
    {
        Application.targetFrameRate = 60;
        resetAnims();
        clearPool();

        character_instance.transform.position = spawn_point.position;
    }

    private void resetAnims()
    {
        character_instance.CharacterAnimator.ResetTrigger("Basic_1");
        character_instance.CharacterAnimator.ResetTrigger("Basic_2"); 
        character_instance.CharacterAnimator.ResetTrigger("Basic_3");
        character_instance.CharacterAnimator.ResetTrigger("Skill_2");
        character_instance.CharacterAnimator.ResetTrigger("Skill_3");
        character_instance.CharacterAnimator.ResetTrigger("Skill_3_attack");
        character_instance.CharacterAnimator.SetTrigger("Walking");
        character_instance.CharacterAnimator.ResetTrigger("Walking");
    }

    private void clearPool()
    {
        foreach(GameObject go in pool)
            Destroy(go);

        pool.Clear();
    }

    private void resetCoroutine( IEnumerator cor )
    {
      if(running_cor != null)
            StopCoroutine(running_cor);

      running_cor = cor;
      StartCoroutine(running_cor);
    }

    private void OnGUI()
    {
        Time.timeScale = time_scale;

        if ( GUI.Button(new Rect( 100, 100, 80, 80 ), "init") )
            init();

        if ( GUI.Button(new Rect( 200, 100, 80, 80 ), "Skill1Full") )
            activateSkill1All();

        if ( GUI.Button(new Rect( 200, 200, 80, 80 ), "Skill1_1") )
            activateSkill1(1, skill1_vfx_1);

        if ( GUI.Button(new Rect( 200, 300, 80, 80 ), "Skill1_2") )
            activateSkill1(2, skill1_vfx_2);

        if ( GUI.Button(new Rect( 200, 400, 80, 80 ), "Skill1_3") )
            activateSkill1(3, skill1_vfx_3);

        if ( GUI.Button(new Rect( 300, 100, 80, 80 ), "Skill2") )
            activateSkill2();

        if ( GUI.Button(new Rect( 400, 100, 80, 80 ), "Skill3") )
            activateSkill3();

        if ( GUI.Button(new Rect( 400, 200, 80, 80 ), "Skill3Block") )
            activateSkill3Block();

        if ( GUI.Button(new Rect( 400, 300, 80, 80 ), "Skill3Attack") )
            activateSkill3Attack();

        if ( GUI.Button(new Rect( 500, 100, 80, 80 ), "Walk") )
            character_instance.CharacterAnimator.SetTrigger("Walking");

        if ( GUI.Button(new Rect( 500, 200, 80, 80 ), "Idle") )
            character_instance.CharacterAnimator.SetTrigger("Reset");
    }

    private void activateSkill1( int skill_number, GameObject vfx )
    {
        resetAnims();
        clearPool();
        character_instance.CharacterAnimator.SetTrigger("Basic_" + skill_number);

        GameObject cached_vfx = null;

        if(vfx != null)
          cached_vfx = Instantiate(vfx, skill1_vfx_root);

        pool.Add( cached_vfx );
    }

    private void activateSkill1All()
    {
        resetAnims();
        clearPool();

        resetCoroutine(impl());

        IEnumerator impl()
        {
          activateSkill1(1, skill1_vfx_1);
          yield return new WaitForSeconds(skill1_duration_1);
          activateSkill1(2, skill1_vfx_2);
          yield return new WaitForSeconds(skill1_duration_2);
          activateSkill1(3, skill1_vfx_3);
          yield return new WaitForSeconds(skill1_duration_3);
        }
    }

    private void activateSkill2()
    {
        resetAnims();
        clearPool();
        resetCoroutine(skill2());
    }


    private IEnumerator skill2()
    {
        character_instance.CharacterAnimator.SetTrigger("Skill_2");

        GameObject cached_vfx = null;

        if( skill2_vfx != null )
            cached_vfx = Instantiate(skill2_vfx, skill2_vfx_root);

        pool.Add( cached_vfx );

        yield return new WaitForSeconds(skill2_duration);
  
        character_instance.CharacterAnimator.ResetTrigger("Skill_2");
    }

    private void activateSkill3()
    {
        resetAnims();
        clearPool();
        resetCoroutine(skill3());
    }

    private IEnumerator skill3()
    {
        character_instance.CharacterAnimator.SetTrigger("Skill_3");

        GameObject cached_vfx = null;

        if( skill3_vfx_start != null )
            cached_vfx = Instantiate(skill3_vfx_start, skill3_vfx_root);

        pool.Add( cached_vfx );

        if( skill3_vfx_sword != null )
            cached_vfx = Instantiate(skill3_vfx_sword, skill3_vfx_sword_root);
        pool.Add( cached_vfx );

        yield return new WaitForSeconds(skill3_duration);
        
        character_instance.CharacterAnimator.SetTrigger("Skill_3_attack");

          if( skill3_vfx_attack != null )
                cached_vfx = Instantiate(skill3_vfx_attack, skill3_vfx_root);

          pool.Add( cached_vfx );
    }

    private void activateSkill3Block()
    {
      GameObject cached_vfx = null;

      if( skill3_vfx_block != null )
          cached_vfx = Instantiate(skill3_vfx_block, skill3_vfx_root);

      pool.Add( cached_vfx );
    }

    private void activateSkill3Attack()
    {
        resetAnims();
        clearPool();
        resetCoroutine(skill3Attack());
    }

    private IEnumerator skill3Attack()
    {
        character_instance.CharacterAnimator.SetTrigger("Skill_3");

        yield return new WaitForSeconds(skill3_attack_duration);

        GameObject cached_vfx = null;
        
        character_instance.CharacterAnimator.SetTrigger("Skill_3_attack");

        if( skill3_vfx_attack != null )
            cached_vfx = Instantiate(skill3_vfx_attack, skill3_vfx_root);
    
        pool.Add( cached_vfx );
    }
}