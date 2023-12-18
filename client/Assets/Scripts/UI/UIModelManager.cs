using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIModelManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerModelContainer;
    bool animate = false;
    const float ANIMATION_INTERVAL = 20f;
    float animationClipDuration;

    public void SetModel(string characterName)
    {
        GameObject playerModel = CharactersManager.Instance.characterSriptableObjects
            .Single(character => character.name == characterName)
            .UIModel;
        GameObject modelClone = Instantiate(
            playerModel,
            playerModelContainer.transform.position,
            playerModel.transform.rotation,
            playerModelContainer.transform
        );
        animate = true;
        animationClipDuration = AnimationClipTime(modelClone.GetComponentInChildren<Animator>());
        StartCoroutine(AnimateCharacter(modelClone));
    }

    public void RemoveCurrentModel()
    {
        if (playerModelContainer.transform.childCount > 0)
        {
            Destroy(playerModelContainer.transform.GetChild(0).gameObject);
        }
    }

    float AnimationClipTime(Animator modelClone)
    {
        List<AnimationClip> clips = modelClone.runtimeAnimatorController.animationClips.ToList();
        return clips.Single(clip => clip.name == "Victory").length;
    }

    IEnumerator AnimateCharacter(GameObject modelClone)
    {
        while (animate)
        {
            yield return new WaitForSeconds(1f);
            modelClone.GetComponentInChildren<Animator>().SetBool("Victory", true);
            yield return new WaitForSeconds(animationClipDuration);
            modelClone.GetComponentInChildren<Animator>().SetBool("Victory", false);
            yield return new WaitForSeconds(ANIMATION_INTERVAL);
        }
    }
}
