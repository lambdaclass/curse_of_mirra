using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIModelManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerModelContainer;
    public bool responsiveRawImage;

    [MMCondition("responsiveRawImage", true)]
    [SerializeField]
    GameObject rawImage;

    public bool hasCharacterShadow;

    [MMCondition("hasCharacterShadow", true)]
    [SerializeField]
    Transform characterShadow;
    bool animate = false;
    const float ANIMATION_INTERVAL = 20f;
    float animationClipDuration;
    Coroutine characterAnimation;
    public Animator modelAnimator;

    void Start()
    {
        if (responsiveRawImage)
        {
            rawImage.transform.localScale = ResponsiveModel();
        }
    }

    public void SetModel(string characterName)
    {
        CoMCharacter character = CharactersManager
            .Instance
            .AvailableCharacters
            .Single(character => character.name == characterName);

        GameObject playerModel = character.UIModel;
        if (hasCharacterShadow)
            characterShadow.localScale = character.shadowScaleValues;
        GameObject modelClone = Instantiate(playerModel, playerModelContainer.transform);
        animate = true;
        modelAnimator = modelClone.GetComponentInChildren<Animator>();
        if (SceneManager.GetActiveScene().name == "MainScreen")
        {
            animationClipDuration = AnimationClipTime(modelAnimator, "Victory");
            characterAnimation = StartCoroutine(AnimateCharacter("Victory"));
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

    float AnimationClipTime(Animator modelClone, string parameterName)
    {
        List<AnimationClip> clips = modelClone.runtimeAnimatorController.animationClips.ToList();
        float animationClipTime = 0;     
    
        if(modelClone.name.Contains("Uma") && parameterName == "Victory"){
            animationClipTime =  clips.Single(clip => clip.name == "Victory_aux").length / 2;
        } else {
            animationClipTime =  clips.Single(clip => clip.name.ToLower() == parameterName.ToLower()).length;
        }
       
        return animationClipTime;
    }

    public IEnumerator AnimateCharacter(string parameterName)
    {
        // Fix this: With the characterSelection PR we can add a string in the ComCharacter to
        // select which animations should be played
        // string animationName = modelClone.name.Contains("Uma") ? "Radiance" : "Victory";
        GameObject modelClone = modelAnimator.transform.parent.gameObject;


        if(modelClone.name.Contains("Uma") && parameterName == "Victory"){
            parameterName = "Victory_aux";
        }

        while (animate)
        {
            yield return new WaitForSeconds(1f);
            modelClone.GetComponentInChildren<Animator>().SetBool(parameterName, true);
            yield return new WaitForSeconds(animationClipDuration);
            modelClone.GetComponentInChildren<Animator>().SetBool(parameterName, false);
            yield return new WaitForSeconds(ANIMATION_INTERVAL);
        }
    }

    public IEnumerator AnimateChainedCharacterSkill(List<string> animationsList, string currentParameterName)
    {
        animationsList.RemoveAt(0);
        
        modelAnimator.SetBool(currentParameterName, true);
        float animationDuration = AnimationClipTime(modelAnimator, currentParameterName);
        yield return new WaitForSeconds(animationDuration);
        modelAnimator.SetBool(currentParameterName, false);

        if(animationsList.Count > 0) {
            StartCoroutine(AnimateChainedCharacterSkill(animationsList, animationsList[0].ToUpper()));
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
        bool hasAnimationParameter = modelAnimator
            .parameters
            .ToList()
            .Any(anim => anim.name == animationName);
        if (hasAnimationParameter)
        {
            modelAnimator.SetBool(animationName, true);
        }
        else
        {
            modelAnimator.Play(animationName);
        }
    }

    Vector3 ResponsiveModel()
    {
        // What makes this responsive is taking into account the canvas scaling
        float scaleCanvas = GetComponentInParent<Canvas>().transform.localScale.x;
        Vector3 rawImageInitialScale = rawImage.transform.localScale;
        Vector3 responsiveScale = new Vector3(
            rawImageInitialScale.x - scaleCanvas,
            rawImageInitialScale.y - scaleCanvas,
            rawImageInitialScale.z - scaleCanvas
        );
        return responsiveScale;
    }
}
