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
        CoMCharacter character = CharactersManager
            .Instance
            .AvailableCharacters
            .Single(character => character.name == characterName);

        GameObject playerModel = character.UIModel;
        if(characterShadow != null) characterShadow.localScale = character.shadowScaleValues;
        GameObject modelClone = Instantiate(playerModel, playerModelContainer.transform);
        animate = true;
        modelAnimator = modelClone.GetComponentInChildren<Animator>();
        if (SceneManager.GetActiveScene().name != "Battle")
        {
            animationClipDuration = AnimationClipTime(modelAnimator);
            characterAnimation = StartCoroutine(AnimateCharacter(modelClone));
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

    public void ShowEndGameCharacterAnimation()
    {
        if (Utils.GetCharacter(GameServerConnectionManager.Instance.playerId))
        {
            bool isWinner = GameServerConnectionManager
                .Instance
                .PlayerIsWinner(GameServerConnectionManager.Instance.playerId);
            string animationName = isWinner ? "Victory" : "Defeat";
            if (modelAnimator.parameterCount > 0)
            {
                HandleAnimation(animationName);
            }
        }
    }

    public void HandleAnimation(string animationName)
    {
        bool hasAnimationParameter = modelAnimator.parameters.ToList().Find(anim => anim.name == animationName) != null;
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
