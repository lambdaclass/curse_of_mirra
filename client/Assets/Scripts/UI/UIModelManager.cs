using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    Animator modelAnimator;

    public void SetModel(string characterName)
    {
        GameObject playerModel = CharactersManager
            .Instance
            .AvailableCharacters
            .Single(character => character.name == characterName)
            .UIModel;
        GameObject modelClone = Instantiate(playerModel, playerModelContainer.transform);
        animate = true;
        modelAnimator = modelClone.GetComponentInChildren<Animator>();
        if (SceneManager.GetActiveScene().name != "Battle")
        {
            animationClipDuration = AnimationClipTime(modelAnimator);
            characterAnimation = StartCoroutine(AnimateCharacter(modelClone));
        }
        if (characterShadow != null)
        {
            SetCharacterShadow(characterName);
        }
    }

    public void SetCharacterShadow(string characterName)
    {
        if (characterName == "H4ck")
        {
            characterShadow.localScale = new Vector3(.9f, 1, 1);
        }
        else if (characterName == "Muflus")
        {
            characterShadow.localScale = new Vector3(1.5f, 1.1f, 1);
        }
        else if (characterName == "Uma")
        {
            characterShadow.localScale = new Vector3(1, 1.1f, 1);
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
        string animationName = modelClone.name.Contains("Uma") ? "Radiance" : "Victory";
        while (animate)
        {
            yield return new WaitForSeconds(1f);
            modelClone.GetComponentInChildren<Animator>().SetBool(animationName, true);
            yield return new WaitForSeconds(animationClipDuration);
            modelClone.GetComponentInChildren<Animator>().SetBool(animationName, false);
            yield return new WaitForSeconds(ANIMATION_INTERVAL);
        }
    }

    public void ShowCharacterAnimation()
    {
        if (Utils.GetCharacter(GameServerConnectionManager.Instance.playerId))
        {
            bool isWinner = GameServerConnectionManager
                .Instance
                .PlayerIsWinner(GameServerConnectionManager.Instance.playerId);
            string animationName = isWinner ? "Victory" : "Defeat";
            if (modelAnimator.parameterCount > 0)
            {
                bool hasAnimationParameter = AnimationHasParameter(animationName);
                HandleAnimation(animationName, hasAnimationParameter);
            }
        }
    }

    private bool AnimationHasParameter(string parameterName)
    {
        AnimatorControllerParameter param = modelAnimator
            .parameters
            .ToList()
            .Find(p => p.name == parameterName);

        return param != null;
    }

    public void HandleAnimation(string animationName, bool hasAnimationParameter)
    {
        if (hasAnimationParameter)
        {
            modelAnimator.SetBool(animationName, true);
        }
        else
        {
            modelAnimator.Play(animationName);
        }
    }
}
