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

    IEnumerator activateSpawnFeedback()
    {
        spawnFeedback.SetActive(true);
        yield return new WaitForSeconds(2f);
        spawnFeedback.SetActive(false);
    }

    void Awake()
    {
        StartCoroutine(activateSpawnFeedback());
    }
}
