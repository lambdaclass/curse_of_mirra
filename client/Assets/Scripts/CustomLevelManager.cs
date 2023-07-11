using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomLevelManager : LevelManager
{
    bool paused = false;
    private GameObject mapPrefab;
    public GameObject quickMapPrefab;
    public GameObject quickGamePrefab;

    [SerializeField]
    GameObject roundSplash;

    [SerializeField]
    Text roundText;

    [SerializeField]
    GameObject backToLobbyButton;
    private List<Player> gamePlayers;

    [SerializeField]
    MMSoundManager soundManager;

    [SerializeField]
    private MMF_Player backgroundMusic;
    private bool isMuted;
    private ulong totalPlayers;
    private ulong playerId;
    private GameObject prefab;
    public Camera UiCamera;
    public CinemachineCameraController camera;

    public List<CoMCharacter> charactersInfo = new List<CoMCharacter>();
    public List<GameObject> mapList = new List<GameObject>();

    int winnersCount = 0;

    protected override void Awake()
    {
        base.Awake();
        this.totalPlayers = (ulong)LobbyConnection.Instance.playerCount;
        InitializeMap();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InitializeLevel());
    }

    private void InitializeMap()
    {
        if (LobbyManager.LevelSelected == null)
        {
            InitializeMapPrefab(quickMapPrefab);
        }
        else
        {
            mapPrefab = mapList.Find(map => map.name == LobbyManager.LevelSelected);
            InitializeMapPrefab(mapPrefab);
        }
    }

    private void InitializeMapPrefab(GameObject mapPrefab)
    {
        GameObject map = Instantiate(mapPrefab);
        //Add gameobject to the scene root
        map.transform.SetParent(
            SceneManager.GetActiveScene().GetRootGameObjects()[0].transform.parent
        );
    }

    private IEnumerator InitializeLevel()
    {
        yield return new WaitUntil(() => SocketConnectionManager.Instance.gamePlayers != null);
        this.gamePlayers = SocketConnectionManager.Instance.gamePlayers;
        playerId = LobbyConnection.Instance.playerId;
        GeneratePlayers();
        SetPlayersSkills(playerId);
        setCameraToPlayer(playerId);
        InitializeAudio();
    }

    void Update()
    {
        if (
            (
                SocketConnectionManager.Instance.winners.Count >= 1
                && winnersCount != SocketConnectionManager.Instance.winners.Count
            )
            || SocketConnectionManager.Instance.winnerPlayer != null
        )
        {
            ShowRoundTransition(SocketConnectionManager.Instance.winners.Count);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GUIManager.Instance.SetPauseScreen(paused == false ? true : false);
            paused = !paused;
        }
    }

    private GameObject GetCharacterPrefab(ulong playerId)
    {
        GameObject prefab = null;
        foreach (
            KeyValuePair<ulong, string> entry in SocketConnectionManager.Instance.selectedCharacters
        )
        {
            if (entry.Key == (ulong)playerId)
            {
                prefab = charactersInfo.Find(el => el.name == entry.Value).prefab;
            }
        }
        return prefab;
    }

    private void GeneratePlayers()
    {
        // prefab = prefab == null ? quickGamePrefab : prefab;
        for (ulong i = 0; i < totalPlayers; i++)
        {
            prefab = GetCharacterPrefab(i + 1);
            if (LobbyConnection.Instance.playerId == i + 1)
            {
                // Player1 is the ID to match with the client InputManager
                prefab.GetComponent<Character>().PlayerID = "Player1";
            }
            else
            {
                prefab.GetComponent<Character>().PlayerID = "";
            }
            Character newPlayer = Instantiate(
                prefab.GetComponent<Character>(),
                Utils.transformBackendPositionToFrontendPosition(gamePlayers[(int)i].Position),
                Quaternion.identity
            );
            newPlayer.name = "Player" + " " + (i + 1);
            newPlayer.PlayerID = (i + 1).ToString();

            SocketConnectionManager.Instance.players.Add(newPlayer.gameObject);
            this.Players.Add(newPlayer);
        }
        this.PlayerPrefabs = (this.Players).ToArray();
    }

    private void setCameraToPlayer(ulong playerID)
    {
        foreach (Character player in this.PlayerPrefabs)
        {
            if (UInt64.Parse(player.PlayerID) == playerID)
            {
                this.camera.SetTarget(player);
                this.camera.StartFollowing();
            }
        }
    }

    private void SetPlayersSkills(ulong clientPlayerId)
    {
        CustomInputManager inputManager = UiCamera.GetComponent<CustomInputManager>();

        foreach (Character player in this.PlayerPrefabs)
        {
            SkillBasic skillBasic = player.gameObject.AddComponent<SkillBasic>();
            Skill1 skill1 = player.gameObject.AddComponent<Skill1>();
            Skill2 skill2 = player.gameObject.AddComponent<Skill2>();
            Skill3 skill3 = player.gameObject.AddComponent<Skill3>();
            Skill4 skill4 = player.gameObject.AddComponent<Skill4>();

            string selectedCharacter = SocketConnectionManager.Instance.selectedCharacters[
                UInt64.Parse(player.PlayerID)
            ];
            CoMCharacter characterInfo = charactersInfo.Find(el => el.name == selectedCharacter);
            SkillAnimationEvents skillsAnimationEvent =
                player.CharacterModel.GetComponent<SkillAnimationEvents>();

            skillBasic.SetSkill(
                Action.BasicAttack,
                characterInfo.skillBasicInfo,
                skillsAnimationEvent
            );
            skill1.SetSkill(Action.Skill1, characterInfo.skill1Info, skillsAnimationEvent);
            skill2.SetSkill(Action.Skill2, characterInfo.skill2Info, skillsAnimationEvent);
            skill3.SetSkill(Action.Skill3, characterInfo.skill3Info, skillsAnimationEvent);
            skill4.SetSkill(Action.Skill4, characterInfo.skill4Info, skillsAnimationEvent);

            if (UInt64.Parse(player.PlayerID) == clientPlayerId)
            {
                inputManager.InitializeInputSprite(characterInfo);
                inputManager.AssignSkillToInput(
                    UIControls.SkillBasic,
                    characterInfo.skillBasicInfo.inputType,
                    skillBasic
                );
                inputManager.AssignSkillToInput(
                    UIControls.Skill1,
                    characterInfo.skill1Info.inputType,
                    skill1
                );
                inputManager.AssignSkillToInput(
                    UIControls.Skill2,
                    characterInfo.skill2Info.inputType,
                    skill2
                );
                inputManager.AssignSkillToInput(
                    UIControls.Skill3,
                    characterInfo.skill3Info.inputType,
                    skill3
                );
                inputManager.AssignSkillToInput(
                    UIControls.Skill4,
                    characterInfo.skill4Info.inputType,
                    skill4
                );
            }
        }
    }

    private void ShowRoundTransition(int roundNumber)
    {
        bool animate = true;
        if (SocketConnectionManager.Instance.winners.Count == 2)
        {
            roundText.text = "Last Round!";
        }
        if (SocketConnectionManager.Instance.winnerPlayer != null)
        {
            roundText.text =
                "Player " + SocketConnectionManager.Instance.winnerPlayer.Id + " Wins!";
            backToLobbyButton.SetActive(true);
            animate = false;
        }

        roundSplash.SetActive(true);
        roundSplash.GetComponent<Animator>().SetBool("NewRound", animate);
        winnersCount = roundNumber;
    }

    private void InitializeAudio()
    {
        backgroundMusic.PlayFeedbacks();
        soundManager.PauseTrack(MMSoundManager.MMSoundManagerTracks.Music);
        soundManager.MuteMaster();
    }
}
