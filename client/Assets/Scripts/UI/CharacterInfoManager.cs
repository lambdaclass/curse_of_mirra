using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoManager : MonoBehaviour
{
    [SerializeField]
    private UIModelManager modelManager;

    [Header("Character info")]
    [SerializeField]
    TextMeshProUGUI nameText;

    [SerializeField]
    Image classImage;

    [SerializeField]
    TextMeshProUGUI subTitle;

    [Header("Character skills")]
    [SerializeField]
    List<SkillDescription> skillDescriptions;

    private int characterIndex;

    List<CoMCharacter> availableCharacters;

    List<string> animationsList = new List<string>();
    int maxSize = 3;

    string lastSkill;

    void Start()
    {
        availableCharacters = CharactersManager.Instance.AvailableCharacters;
        string goToCharacter = CharactersManager.Instance.GetGoToCharacter();

        // Get index from selected character to show previous and next character
        characterIndex = availableCharacters.FindIndex(
            availableCharacter => availableCharacter.name == goToCharacter
        );

        CoMCharacter characterToShow = availableCharacters.Find(
            character => character.name == goToCharacter
        );
        SetCharacterInfo(characterToShow);
    }

    public void SetCharacterInfo(CoMCharacter comCharacter)
    {
        modelManager.RemoveCurrentModel();
        modelManager.SetModel(comCharacter.name);
        nameText.text = comCharacter.name;
        subTitle.text = comCharacter.description;
        classImage.sprite = comCharacter.classImage;
        skillDescriptions[0].SetSkillDescription(comCharacter.skillsInfo[0]);
        skillDescriptions[1].SetSkillDescription(comCharacter.skillsInfo[1]);
        skillDescriptions[2].SetSkillDescription(comCharacter.skillsInfo[2]);
        CharactersManager.Instance.SetGoToCharacter(comCharacter.name);
    }

    public void SelectButton()
    {
        ServerConnection.Instance.selectedCharacterName = CharactersManager
            .Instance
            .GetGoToCharacter();

        PlayerPrefs.SetString("selectedCharacterName",  ServerConnection.Instance.selectedCharacterName);
        this.GetComponent<MMLoadScene>().LoadScene();
        // CharactersManager.Instance.SetGoToCharacter()
        // StartCoroutine(SetCharacter());
    }

    private IEnumerator SetCharacter()
    {
        yield return StartCoroutine(
            ServerUtils.SetSelectedCharacter(
                CharactersManager.Instance.GetGoToCharacter(),
                response =>
                {
                    ServerConnection.Instance.selectedCharacterName = response.selected_character;
                },
                error =>
                {
                    Errors.Instance.HandleNetworkError("Error", error);
                }
            )
        );
        this.GetComponent<MMLoadScene>().LoadScene();
    }

    public void PlaySkillAnimation(string parameterName){
        if(animationsList.Count < maxSize){

            if(lastSkill != parameterName){
                animationsList.Add(parameterName);
            }

            foreach(AnimatorControllerParameter param in modelManager.modelAnimator.parameters){
                if(param.name.ToUpper() == parameterName.ToUpper() + "_END" 
                    || param.name.ToUpper() == parameterName.ToUpper() + "_MIDDLE"){
                    animationsList.Add(param.name);
                }
            }
        }
        if(animationsList.Count > 0 && modelManager.modelAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == true){
            lastSkill = animationsList[0];
            StartCoroutine(modelManager.AnimateChainedCharacterSkill(animationsList,animationsList[0].ToUpper()));
        }
    }
}
