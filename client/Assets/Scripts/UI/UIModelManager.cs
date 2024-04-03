using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIModelManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerModelContainer;

    [SerializeField]
    Transform characterShadow;
    bool animate = false;
    const float ANIMATION_INTERVAL = 20f;
    float animationClipDuration;
    Coroutine characterAnimation;

    public void SetModel(string characterName)
    {
        GameObject playerModel = CharactersManager
            .Instance
            .AvailableCharacters
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
        characterAnimation = StartCoroutine(AnimateCharacter(modelClone));
        if (characterShadow != null)
        {
            SetCharacterShadow(modelClone);
        }
    }

    public void SetCharacterShadow(GameObject playerModel)
    {
        if (playerModel.name.Contains("H4ck"))
        {
            characterShadow.localScale = new Vector3(.75f, .9f, 1);
        }
        else if (playerModel.name.Contains("Muflus"))
        {
            characterShadow.localScale = new Vector3(1.5f, 1.1f, 1);
        }
        else if (playerModel.name.Contains("Uma"))
        {
            characterShadow.localScale = new Vector3(.75f, .9f, 1);
        }
        else
        {
            characterShadow.localScale = new Vector3(1, 1, 1);
        }
    }

    public void RemoveCurrentModel()
    {
        if (playerModelContainer.transform.childCount > 0)
        {
            StopCoroutine(characterAnimation);
            Destroy(playerModelContainer.transform.GetChild(0).gameObject);
        }
    }

    float AnimationClipTime(Animator modelClone)
    {
        List<AnimationClip> clips = modelClone.runtimeAnimatorController.animationClips.ToList();
        return modelClone.name.Contains("Uma")
            ? clips.Single(clip => clip.name == "GR_VeilOfRadiance").length / 2
            : clips.Single(clip => clip.name == "Victory").length;
    }

    IEnumerator AnimateCharacter(GameObject modelClone)
    {
        // Fix this: With the characterSelection PR we can add a string in the ComCharacter to
        // select which animations should be played
        string animationName = "Victory";
        while (animate)
        {
            yield return new WaitForSeconds(1f);
            modelClone.GetComponentInChildren<Animator>().SetBool(animationName, true);
            yield return new WaitForSeconds(animationClipDuration);
            modelClone.GetComponentInChildren<Animator>().SetBool(animationName, false);
            yield return new WaitForSeconds(ANIMATION_INTERVAL);
        }
    }
}
