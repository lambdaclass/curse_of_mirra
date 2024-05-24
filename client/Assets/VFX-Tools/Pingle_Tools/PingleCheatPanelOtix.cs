using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PingleCheatPanelOtix : MonoBehaviour
{
    public Transform spawn_point = null;
    public GameObject skill1_projectile_vfx = null;
    public float skill1_projectile_lifetime = 2.0f;
    public AnimationCurve skill1_projectile_y = null;
    public GameObject skill1_explosion_vfx = null;
    public Transform skil1_target_point = null;
    public float skill1_start_delay = 0.2f;
    public float skill1_duration = 1.2f;
    public Renderer[] char_renderers = null;
    public int lava_duration = 40;

    public GameObject skill2_vfx = null;
    public float skill2_start_delay = 0.2f;
    public float skill2_duration = 1.8f;

    public GameObject skill3_explosion_vfx = null;
    public GameObject skill3_trail_vfx = null;
    public GameObject skill3_decal_vfx = null;
    public int skill3_duration = 80;
    public float skill3_speed = 7.0f;
    public float skill3_start_delay = 0.2f;

    public Renderer[] characters_renderers = null;
    public float lava_amount = 0.0f;


    public Character character_instance = null;
    private List<GameObject> pool = new List<GameObject>();
    private const float ARENA_SIZE = 40.0f;
    private IEnumerator skill_cor = null;

    public void init()
    {
        Application.targetFrameRate = 60;
        //character_instance = FindObjectOfType<Character>();
        resetAnims();
        clearPool();

        character_instance.transform.position = spawn_point.position;
    }

    private void Update()
    {
      return;
      foreach(Renderer renderer in characters_renderers)
      {
        renderer.material.SetFloat( "_LavaAmount", lava_amount );
      }
    }

    private void resetAnims()
    {
        character_instance.CharacterAnimator.ResetTrigger("Skill1");
        character_instance.CharacterAnimator.ResetTrigger("Skill2");
        character_instance.CharacterAnimator.ResetTrigger("Skill3");
        character_instance.CharacterAnimator.SetTrigger("Walking");
        character_instance.CharacterAnimator.ResetTrigger("Walking");

        //
        foreach( Renderer renderer in char_renderers)
        {
          renderer.material.SetFloat( "_LavaAmount", 0.0f );
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

        if (skill_cor != null)
            StopCoroutine(skill_cor);

        skill_cor = skill1();
        StartCoroutine(skill_cor);
    }

    private IEnumerator skill1()
    {
        foreach( Renderer renderer in char_renderers)
        {
            renderer.gameObject.SetActive(true);
        }

        character_instance.CharacterAnimator.SetTrigger("Skill1");

        yield return new WaitForSeconds(skill1_start_delay);


        GameObject cached_projectile = null;
        if( skill1_projectile_vfx != null )
        {
          cached_projectile = Instantiate(skill1_projectile_vfx, character_instance.transform);
          pool.Add( cached_projectile );
        }
        yield return projectileCor(cached_projectile?.transform);

        Destroy( cached_projectile );

        GameObject cached_vfx = null;
        cached_vfx = Instantiate(skill1_explosion_vfx, skil1_target_point);
        pool.Add( cached_vfx );

        for(int i = 0; i< lava_duration; i++)
        {
          yield return null;
          foreach( Renderer renderer in char_renderers)
          {
            renderer.material.SetFloat( "_LavaAmount", i / (float)lava_duration * 0.5f );
          }
        }
        character_instance.CharacterAnimator.ResetTrigger("Skill1");
    }

    private IEnumerator projectileCor( Transform projectile )
    {
      Vector3 new_pos = Vector3.zero;
      float cached_time = skill1_projectile_lifetime;
      while(cached_time > 0.0f)
      {
        cached_time -= Time.deltaTime;
        new_pos = Vector3.Lerp( character_instance.transform.position, skil1_target_point.position, 1 - (cached_time / skill1_projectile_lifetime) );
        new_pos.y = skill1_projectile_y.Evaluate(1 - (cached_time / skill1_projectile_lifetime));
        projectile.position = new_pos;
        yield return null;
      }
      yield return null;
    }

    private void activateSkill2()
    {
        resetAnims();
        clearPool();

        if (skill_cor != null)
            StopCoroutine(skill_cor);

        skill_cor = skill2();
        StartCoroutine(skill_cor);
    }


    private IEnumerator skill2()
    {
        //character_instance.CharacterAnimator.SetTrigger("Skill2");

        yield return new WaitForSeconds(skill2_start_delay);

        GameObject cached_vfx = null;

        if( skill2_vfx != null )
        {
            cached_vfx = Instantiate(skill2_vfx, character_instance.transform.position, character_instance.transform.rotation);
            cached_vfx.transform.position = new Vector3(cached_vfx.transform.position.x, 0.0f, cached_vfx.transform.position.z);
        }

        pool.Add( cached_vfx );
        yield return new WaitForSeconds(skill2_duration - skill2_start_delay);
        //character_instance.CharacterAnimator.ResetTrigger("Skill2");
    }

    private void activateSkill3()
    {
        resetAnims();
        clearPool();

        if (skill_cor != null)
            StopCoroutine(skill_cor);

        skill_cor = skill3();
        StartCoroutine(skill_cor);
    }

    private IEnumerator skill3()
    {
        foreach( Renderer renderer in char_renderers)
        {
            renderer.gameObject.SetActive(false);
        }

        GameObject spawned_dash_vfx = null;

        character_instance.CharacterAnimator.SetTrigger("Skill3");

        yield return new WaitForSeconds(skill3_start_delay);

        GameObject cached_vfx = null;

        if( skill3_explosion_vfx != null )
            cached_vfx = Instantiate(skill3_explosion_vfx, character_instance.transform.position, character_instance.transform.rotation);

        pool.Add( cached_vfx );

        if( skill3_trail_vfx != null )
            cached_vfx = Instantiate(skill3_trail_vfx, character_instance.transform);

        pool.Add( cached_vfx );

        if( skill3_decal_vfx != null )
            cached_vfx = Instantiate(skill3_decal_vfx, character_instance.transform.position, character_instance.transform.rotation);

        cached_vfx.transform.position = new Vector3(cached_vfx.transform.position.x, 0.3f, cached_vfx.transform.position.z);

        pool.Add( cached_vfx );

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