// using System.Collections;
using System.Collections;
using System.IO;
using System.Linq;
using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : LevelSelector
{
    private const string BATTLE_SCENE_NAME = "Battle";
    private const string LOBBY_SCENE_NAME = "Lobby";
    private const string MAIN_SCENE_NAME = "MainScreen";
    private const string LOBBIES_BACKGROUND_MUSIC = "LobbiesBackgroundMusic";

    [SerializeField] TextMeshProUGUI tipText;
    [SerializeField] Image characterImage;

    public static string LevelSelected;

    void Start()
    {
        StartCoroutine(WaitForLobbyJoin());
        if(SceneManager.GetActiveScene().ToString() != BATTLE_SCENE_NAME){
            ShowCharacterTips();
        }
    }


    private void ShowCharacterTips(){
        var characterSelected = ServerConnection.Instance.selectedCharacterName;
        print(characterSelected);
        string filePath = Path.Combine(Application.dataPath, "Resources/tips.json");
        string jsonData = File.ReadAllText(filePath);
        CharactersData charactersData = JsonUtility.FromJson<CharactersData>(jsonData);
 
        foreach (CharacterTips character in charactersData.characters)
        {
            if(character.name.ToUpper() == characterSelected.ToUpper()){
                int random = Random.Range(0,character.tips.Count());
                string abilityTip = character.tips[random];

                tipText.text = abilityTip;
                characterImage.sprite = CharactersManager.Instance.AllCharacters.Find(character => character.name.ToUpper() == characterSelected.ToUpper()).UIIcon;
            }
        }
    }

    public void BackToLobbyFromGame()
    {
        Destroy(GameObject.Find(LOBBIES_BACKGROUND_MUSIC));
        BackToLobbyAndCloseConnection();
    }

    public void BackToLobbyAndCloseConnection()
    {
        // Websocket connection is closed as part of Init() destroy;
        GameServerConnectionManager.Instance.Init();
        DestroySingletonInstances();
        Back();
    }

    private void DestroySingletonInstances()
    {
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
    }

    public void Back()
    {
        // ServerConnection.Instance.Init();
        this.LevelName = MAIN_SCENE_NAME;
        SceneManager.LoadScene(this.LevelName);
    }

    public IEnumerator WaitForLobbyJoin()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == LOBBY_SCENE_NAME);
        ServerConnection.Instance.JoinLobby();
        yield return new WaitUntil(
            () =>
                !string.IsNullOrEmpty(ServerConnection.Instance.LobbySession)
                && !string.IsNullOrEmpty(SessionParameters.GameId)
        );
        SceneManager.LoadScene(BATTLE_SCENE_NAME);
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
