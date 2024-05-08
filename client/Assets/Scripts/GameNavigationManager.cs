using System.Collections;
using System.Linq;
using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameNavigationManager : LevelSelector
{
    private const string BATTLE_SCENE_NAME = "Battle";
    private const string LOBBY_SCENE_NAME = "Lobby";
    private const string MAIN_SCENE_NAME = "MainScreen";

    [SerializeField] TextMeshProUGUI tipText;
    [SerializeField] Image characterImage;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == LOBBY_SCENE_NAME){
            StartCoroutine(Utils.WaitForBattleCreation(LOBBY_SCENE_NAME, BATTLE_SCENE_NAME, "join"));
            ShowCharacterTips();
        }
    }
    public void ExitGame(string goToScene){
        Utils.BackToLobbyFromGame(goToScene);
    }

    public void JoinLobby()
    {
        SceneManager.LoadScene(LOBBY_SCENE_NAME);
    }

    public void QuickGame()
    {
        StartCoroutine(Utils.WaitForBattleCreation(MAIN_SCENE_NAME, BATTLE_SCENE_NAME, "quick_game"));
    }

    private void ShowCharacterTips(){
        var characterSelected = ServerConnection.Instance.selectedCharacterName;
        TextAsset jsonFile = Resources.Load<TextAsset>("tips");
        CharactersData charactersData = JsonUtility.FromJson<CharactersData>(jsonFile.text);

        foreach (CharacterTips character in charactersData.characters)
        {
            if(character.name.ToUpper() == characterSelected.ToUpper()){
                int random = Random.Range(0,character.tips.Count());
                string abilityTip = character.tips[random];

                int threshold = Mathf.FloorToInt(character.tips.Count() * 0.3f);

                if(random <= threshold){
                    var genetalTip = charactersData.characters.Last();
                    var randomTip = Random.Range(0, genetalTip.tips.Count());
                    tipText.text = genetalTip.tips[randomTip];
                }else{
                    tipText.text = abilityTip;
                }

                for(int i = 0; i < CharactersManager.Instance.AllCharacters.Count();i++){
                    if(character.name.ToUpper() == CharactersManager.Instance.AllCharacters[i].name.ToUpper()){
                        characterImage.sprite = CharactersManager.Instance.AllCharacters[i].UIIcon;
                        return;
                    }
                }
                return;
            }
        }
    }

    [System.Serializable]
    public class CharactersData
    {
        public CharacterTips[] characters;
    }

    [System.Serializable]
    public class CharacterTips
    {
        public string name;
        public string[] tips;
    }

     public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
}
