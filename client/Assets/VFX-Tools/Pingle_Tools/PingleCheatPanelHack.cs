using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PingleCheatPanelHack : MonoBehaviour
{
  public GameObject skill2_vfx = null;
  public float projectile_speed = 4.0f;
  public float spawn_pivot_height = 1.2f;
  public int skill3_duration = 80;
  public float skill3_speed = 7.0f;
  public Transform spawn_point = null;
  public GameObject dash_vfx = null;

  private Character hack_instance = null;
  private List<GameObject> pool = new List<GameObject>();
  private WaitForSeconds waiter = null;
  private const float ARENA_SIZE = 40.0f;

  private void Start()
  {
      StartCoroutine( moveProjectiles() );
  }
  public void init()
  {
      Application.targetFrameRate = 60;
      waiter = new WaitForSeconds( 0.1f );
      hack_instance = FindObjectOfType<Character>();
      resetAnims();
      hack_instance.transform.position = spawn_point.position;
  }

  private void resetAnims()
  {
      hack_instance.CharacterAnimator.ResetTrigger("Skill1");
      hack_instance.CharacterAnimator.ResetTrigger("Skill2");
      hack_instance.CharacterAnimator.ResetTrigger("Skill3");
      hack_instance.CharacterAnimator.SetTrigger("Walking");
      hack_instance.CharacterAnimator.ResetTrigger("Walking");
  }

  private void OnGUI()
  {
      if (GUI.Button(new Rect( 100, 100, 80, 80 ), "init"))
          init();

      if (GUI.Button(new Rect( 200, 100, 80, 80 ), "Skill_1"))
          return;

      if (GUI.Button(new Rect( 300, 100, 80, 80 ), "Skill_2"))
          activateSkill2();

      if (GUI.Button(new Rect( 400, 100, 80, 80 ), "Skill_3"))
          activateSkill3();
  }

  private void activateSkill2()
  {
      resetAnims();
      clearPool();
      StartCoroutine(skill2());
  }


  private IEnumerator skill2()
  {
      hack_instance.CharacterAnimator.SetTrigger("Skill2");

      yield return new WaitForSeconds( 0.2f );

      GameObject cached_vfx = null;
      Vector3 new_pos = Vector3.zero;
      Quaternion look_dir = Quaternion.Euler(0.0f, 90.0f,0.0f);

      for ( int i = 0; i < 15; i++ )
      {
          new_pos = hack_instance.transform.position;
          new_pos.y += spawn_pivot_height;
          new_pos.x += 2.0f;
          cached_vfx = Instantiate( skill2_vfx, new_pos, look_dir );

          pool.Add( cached_vfx );
          yield return waiter;
      }

      hack_instance.CharacterAnimator.ResetTrigger("Skill2");
      yield return new WaitForSeconds(3.2f);
      clearPool();
  }

  private IEnumerator moveProjectiles()
  {
      Vector3 cached_vector = Vector3.zero;

      while(true)
      {
        foreach(GameObject go in pool)
        {
          cached_vector = go.transform.position;
          cached_vector.x += projectile_speed * Time.deltaTime;
          go.transform.position = cached_vector;
        }

        yield return null;
      }
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
      GameObject spawned_dash_vfx = Instantiate(dash_vfx, hack_instance.transform.position, Quaternion.Euler(0.0f, 90.0f, 0.0f), hack_instance.transform);

      hack_instance.CharacterAnimator.SetTrigger("Skill3");

      yield return new WaitForSeconds(1.0f);
      Vector3 new_pos = hack_instance.transform.position;

      for( int i = 0; i < skill3_duration; i++ )
      {
          new_pos = hack_instance.transform.position;
          new_pos.x += skill3_speed * Time.deltaTime;

          if ( new_pos.x > ARENA_SIZE || new_pos.x < -ARENA_SIZE )
              continue;

          hack_instance.transform.position = new_pos;
          yield return null;
      }

      resetAnims();
      hack_instance.CharacterAnimator.SetTrigger("Walking");
      yield return new WaitForSeconds(1.0f);

      hack_instance.CharacterAnimator.ResetTrigger("Walking");
      Destroy(spawned_dash_vfx);
  }
}