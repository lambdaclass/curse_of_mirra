using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIModelManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerModelContainer;

    [Tooltip("All UI character models")]
    [SerializeField]
    List<GameObject> playerModels;

    [Tooltip("Enable characters name to be used")]
    private List<string> enabledCharacters = new List<string>();
    bool animate = false;
    const float ANIMATION_INTERVAL = 20f;
    float animationClipDuration;

    public void SetModel(CoMCharacter character = null)
    {
        List<string> availableModels = new List<string>();
        int index = 0;
        string name = "";
        if (character == null)
        {
            //We intersect all the characters model and the enable ones.
            availableModels = playerModels
                .Select(el => el.name)
                .Intersect(enabledCharacters)
                .ToList();
            index = Random.Range(0, availableModels.Count);
            name = availableModels[index];
        }
        GameObject playerModel =
            character != null
                ? character.UIModel
                : playerModels.Single(playerModel => playerModel.name == name);
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

    public void SetupList(List<string> characters)
    {
        enabledCharacters = characters;
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
