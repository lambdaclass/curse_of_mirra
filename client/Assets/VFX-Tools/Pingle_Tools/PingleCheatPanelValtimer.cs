using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PingleCheatPanelValtimer : MonoBehaviour
{
  public Transform spawn_point = null;

  public GameObject skill1_vfx = null;
  public float skill1_init_delay = 0.3f;
  public float projectile_speed = 4.0f;
  public float spawn_pivot_height = 1.2f;

  public GameObject skill2_vfx_self = null;
  public GameObject skill2_vfx_void = null;
  public float skill2_init_delay = 0.3f;
  public float skill2_void_delay = 0.3f;
  public Transform void_root = null;

  public GameObject skill3_vfx_self = null;
  public GameObject skill3_vfx_portal = null;
  public Transform portal_root = null;
  public float skill3_init_delay = 0.3f;
  public float skill3_teleport_delay = 0.3f;

  private Character character_instance = null;
  private List<GameObject> pool = new List<GameObject>();
  private List<GameObject> projectiles_pool = new List<GameObject>();
  private WaitForSeconds waiter = null;
  private const float ARENA_SIZE = 40.0f;
  private IEnumerator skill1_cor = null;
  private IEnumerator skill2_cor = null;
  private IEnumerator skill3_cor = null;

  private void Start()
  {
      StartCoroutine( moveProjectiles() );
  }
  public void init()
  {
      Application.targetFrameRate = 60;
      waiter = new WaitForSeconds( 0.1f );
      character_instance = FindObjectOfType<Character>();
      resetAnims();
      stopAllCoroutines();
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

  private void stopAllCoroutines()
  {
      if (skill1_cor != null)
          StopCoroutine(skill1_cor);

      if (skill2_cor != null)
          StopCoroutine(skill2_cor);

      if (skill3_cor != null)
          StopCoroutine(skill3_cor);
  }

  private IEnumerator moveProjectiles()
  {
      Vector3 cached_vector = Vector3.zero;

      while(true)
      {
        foreach(GameObject go in projectiles_pool)
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

      foreach(GameObject go in projectiles_pool)
          Destroy(go);

      projectiles_pool.Clear();
  }

  private void OnGUI()
  {
      if (GUI.Button(new Rect( 100, 100, 80, 80 ), "init"))
          init();

      if (GUI.Button(new Rect( 200, 100, 80, 80 ), "Skill_1"))
          activateSkill1();

      if (GUI.Button(new Rect( 300, 100, 80, 80 ), "Skill_2"))
          activateSkill2();

      if (GUI.Button(new Rect( 400, 100, 80, 80 ), "Skill_3"))
          activateSkill3();
  }

  private void activateSkill1()
  {
      resetAnims();
      clearPool();
      stopAllCoroutines();

      skill1_cor = skill1();
      StartCoroutine(skill1_cor);
  }

  private IEnumerator skill1()
  {
      character_instance.CharacterAnimator.SetTrigger("Skill1");

      yield return new WaitForSeconds(skill1_init_delay);

      GameObject cached_vfx = null;
      Vector3 new_pos = Vector3.zero;

      new_pos = character_instance.transform.position;
      new_pos.y += spawn_pivot_height;
      new_pos.x += 0.5f;
      cached_vfx = Instantiate( skill1_vfx, new_pos, character_instance.transform.rotation );

      projectiles_pool.Add(cached_vfx);

      yield return new WaitForSeconds(1.0f);
      resetAnims();
  }

  private void activateSkill2()
  {
      resetAnims();
      clearPool();
      stopAllCoroutines();

      skill2_cor = skill2();
      StartCoroutine(skill2_cor);
  }


  private IEnumerator skill2()
  {
      character_instance.CharacterAnimator.SetTrigger("Skill2");

      yield return new WaitForSeconds(skill2_init_delay);

      GameObject cached_vfx = null;
      Vector3 new_pos = Vector3.zero;

      if(skill2_vfx_self != null)
      {
          cached_vfx = Instantiate(skill2_vfx_self, character_instance.transform.position, character_instance.transform.rotation);
          pool.Add( cached_vfx );
      }

      yield return new WaitForSeconds(skill2_void_delay);

      if(skill2_vfx_void != null)
      {
          cached_vfx = Instantiate(skill2_vfx_void, void_root.transform.position, void_root.transform.rotation);
          pool.Add( cached_vfx );
      }

      yield return new WaitForSeconds(1.0f);
      resetAnims();
  }

  private void activateSkill3()
  {
      resetAnims();
      clearPool();
      stopAllCoroutines();

      skill3_cor = skill3();
      StartCoroutine(skill3_cor);
  }

  private IEnumerator skill3()
  {

      character_instance.CharacterAnimator.SetTrigger("Skill3");
      yield return new WaitForSeconds(skill3_init_delay);

      GameObject spawned_vfx = null;
      if (skill3_vfx_self != null)
      {
          spawned_vfx = Instantiate(skill3_vfx_self, character_instance.transform.position, character_instance.transform.rotation);
          pool.Add( spawned_vfx );
      }

      if (skill3_vfx_self != null)
      {
          spawned_vfx = Instantiate(skill3_vfx_portal, portal_root.position, portal_root.rotation);
          pool.Add( spawned_vfx );
      }

      yield return new WaitForSeconds(skill3_teleport_delay);
      character_instance.transform.position = portal_root.position;

      yield return new WaitForSeconds(0.8f);
      resetAnims();
  }
}