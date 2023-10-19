using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    public GameObject PlayerName;

    [SerializeField]
    public GameObject Hitbox;

    [SerializeField]
    public GameObject FeedbackContainer;

    [SerializeField]
    public GameObject AimDirection;

    [SerializeField]
    public GameObject SkillRange;

    [SerializeField]
    public GameObject spawnFeedback;

    public IEnumerator activateSpawnFeedback()
    {
        print("arranco");
        float lifeTime = spawnFeedback.GetComponent<VisualEffect>().GetFloat("LifeTime");
        spawnFeedback.SetActive(true);
        yield return new WaitForSeconds(lifeTime);
        spawnFeedback.SetActive(false);
    }

    void Awake()
    {
        // StartCoroutine(activateSpawnFeedback());
    }
}
