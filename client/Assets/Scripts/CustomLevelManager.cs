using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomLevelManager : LevelManager
{
    private const float DEATH_FEEDBACK_DURATION = 1.5f;
    bool paused = false;
    private GameObject mapPrefab;
    public GameObject quickMapPrefab;

    [SerializeField]
    GameObject roundSplash;

    [SerializeField]
    GameObject deathSplash;

    [SerializeField]
    Text roundText;

    [SerializeField]
    Text totalKillsText;

    [SerializeField]
    GameObject backToLobbyButton;
    private List<Player> gamePlayers;

    [SerializeField]
    private BackgroundMusic backgroundMusic;
    private ulong totalPlayers;
    private ulong playerId;
    private GameObject prefab;
    public Camera UiCamera;
    public Player playerToFollow;

    [SerializeField]
    public GameObject UiControls;
    public CinemachineCameraController camera;
    private ulong playerToFollowId;
    public List<CoMCharacter> charactersInfo = new List<CoMCharacter>();
    public List<GameObject> mapList = new List<GameObject>();

    //Camera cinematic variables
    [SerializeField]
    GameObject loadingScreen;

    [SerializeField]
    GameObject battleScreen;
    Int32 CAMERA_OFFSET = 30;
    Int32 CAMERA_Y_OFFSET = 6;
    double xDigit = 0;
    double zDigit = 0;
    CinemachineFramingTransposer cameraFramingTransposer = null;
    private bool deathSplashIsShown = false;

    protected override void Awake()
    {
        base.Awake();
        this.totalPlayers = (ulong)LobbyConnection.Instance.playerCount;
        SocketConnectionManager.Instance.BotSpawnRequested += GenerateBotPlayer;
        InitializeMap();
        cameraFramingTransposer = this.camera
            .GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineFramingTransposer>();
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
        playerToFollowId = playerId;
        GeneratePlayers();
        SetPlayersSkills(playerId);
        setCameraToPlayer(playerId);
        var player = Utils.GetPlayer(playerId);
        cameraFramingTransposer.m_TrackedObjectOffset = new Vector3(
            player.transform.position.x > 0 ? -CAMERA_OFFSET : CAMERA_OFFSET,
            CAMERA_Y_OFFSET,
            player.transform.position.z > 0 ? -CAMERA_OFFSET : CAMERA_OFFSET
        );

        SetPlayerHealthBar(playerId);
        deathSplash.GetComponent<DeathSplashManager>().SetDeathSplashPlayer();
        MMSoundManager.Instance.FreeAllSounds();
        MMSoundManagerSoundPlayEvent.Trigger(
            backgroundMusic.SoundClip,
            MMSoundManager.MMSoundManagerTracks.Music,
            this.transform.position,
            true
        );
        StartCoroutine(CameraCinematic());
    }

    void Update()
    {
        Player gamePlayer = Utils.GetGamePlayer(playerId);
        GameObject player = Utils.GetPlayer(playerId);
        if (GameHasEndedOrPlayerHasDied(gamePlayer) && !deathSplashIsShown)
        {
            StartCoroutine(ShowDeathSplash(player));
            deathSplashIsShown = true;
        }
        if (GameHasEnded())
        {
            deathSplash.GetComponent<DeathSplashManager>().ShowEndGameScreen();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GUIManager.Instance.SetPauseScreen(paused == false ? true : false);
            paused = !paused;
        }

        if (Utils.GetGamePlayer(SocketConnectionManager.Instance.playerId).Health <= 0)
        {
            SetCameraToAlivePlayer();
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
                prefab.GetComponent<CustomCharacter>().PlayerID = "Player1";
            }
            else
            {
                prefab.GetComponent<CustomCharacter>().PlayerID = "";
            }
            CustomCharacter newPlayer = Instantiate(
                prefab.GetComponent<CustomCharacter>(),
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

    IEnumerator CameraCinematic()
    {
        if (!SocketConnectionManager.Instance.cinematicDone)
        {
            //Start moving camera and remove loading sceen
            InvokeRepeating("Substract", 1f, 0.1f);
            yield return new WaitForSeconds(1.7f);
            loadingScreen.SetActive(false);
            battleScreen.SetActive(true);
            //Cancel camera movement and start zoom in
            StartCoroutine(
                Utils.GetCharacter(
                        playerId)
                    .characterBase.activateSpawnFeedback(true));
            Utils
                .GetAlivePlayers()
                .Where(player => player.Id != playerId)
                .ToList()
                .ForEach(el => StartCoroutine(
                    Utils.GetCharacter(
                        el.Id)
                    .characterBase.activateSpawnFeedback(false))
                );
            yield return new WaitForSeconds(2.1f);
            CancelInvoke("Substract");
            InvokeRepeating("MoveYCamera", 0.3f, 0.1f);
            //Cancel camera zoom
            yield return new WaitForSeconds(0.5f);
            CancelInvoke("MoveYCamera");
        }
        else
        {
            cameraFramingTransposer.m_TrackedObjectOffset = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.9f);
            loadingScreen.SetActive(false);
        }
    }

    int RoundUpByTen(int i)
    {
        return (int)(Math.Ceiling(i / 10.0d) * 10);
    }

    void MoveYCamera()
    {
        Vector3 cameraOffset = cameraFramingTransposer.m_TrackedObjectOffset;

        cameraFramingTransposer.m_TrackedObjectOffset = new Vector3(
            0,
            cameraOffset.y != 0 ? cameraOffset.y - 3 : 0,
            0
        );
    }

    void Substract()
    {
        Vector3 cameraOffset = cameraFramingTransposer.m_TrackedObjectOffset;

        var xIsPositive = Math.Round(cameraOffset.x) > 0;
        var zIsPositive = Math.Round(cameraOffset.z) > 0;
        var xValue = (xIsPositive ? -1 : 1);
        var zValue = (zIsPositive ? -1 : 1);

        cameraFramingTransposer.m_TrackedObjectOffset = new Vector3(
            cameraOffset.x + (float)(cameraOffset.x != 0 ? xValue : 0),
            cameraOffset.y,
            cameraOffset.z + (float)(cameraOffset.z != 0 ? zValue : 0)
        );
        ;
    }

    private void GenerateBotPlayer(SocketConnectionManager.BotSpawnEventData botSpawnEventData)
    {
        botSpawnEventData.gameEventPlayers
            .ToList()
            .FindAll((player) => !botSpawnEventData.gamePlayers.Any((p) => p.Id == player.Id))
            .ForEach(
                (player) =>
                {
                    var spawnPosition = Utils.transformBackendPositionToFrontendPosition(
                        player.Position
                    );
                    CustomCharacter botCharacter = SpawnBot.Instance.GetCharacterByName(
                        player.CharacterName
                    );
                    var botId = player.Id.ToString();
                    botCharacter.PlayerID = "";

                    CustomCharacter newPlayer = Instantiate(
                        botCharacter,
                        spawnPosition,
                        Quaternion.identity
                    );
                    newPlayer.PlayerID = botId.ToString();
                    newPlayer.name = "BOT" + botId;
                    Image healthBarFront = newPlayer
                        .GetComponent<MMHealthBar>()
                        .TargetProgressBar.ForegroundBar.GetComponent<Image>();

                    healthBarFront.color = Utils.healthBarRed;
                    SocketConnectionManager.Instance.players.Add(newPlayer.gameObject);

                    List<Skill> skillList = new List<Skill>();

                    SkillBasic skillBasic = newPlayer.gameObject.AddComponent<SkillBasic>();
                    Skill1 skill1 = newPlayer.gameObject.AddComponent<Skill1>();

                    skillList.Add(skillBasic);
                    skillList.Add(skill1);

                    CoMCharacter characterInfo = charactersInfo.Find(
                        el => el.name == player.CharacterName
                    );
                    SkillAnimationEvents skillsAnimationEvent =
                        newPlayer.CharacterModel.GetComponent<SkillAnimationEvents>();

                    List<SkillInfo> skillInfoClone = InitSkills(characterInfo);
                    SetSkillAngles(skillInfoClone);

                    skillBasic.SetSkill(
                        Action.BasicAttack,
                        skillInfoClone[0],
                        skillsAnimationEvent
                    );
                    skill1.SetSkill(Action.Skill1, skillInfoClone[1], skillsAnimationEvent);

                    var items = LobbyConnection.Instance.serverSettings.SkillsConfig.Items;

                    foreach (var skill in items)
                    {
                        for (int i = 0; i < skillList.Count; i++)
                        {
                            if (skill.Name.ToLower() == skillList[i].GetSkillName().ToLower())
                            {
                                // 350 in the back is equal to 12 in the front
                                // So this is the calculation
                                skillList[i].SetSkillAreaRadius(
                                    float.Parse(skill.SkillRange) / 100
                                );
                            }
                        }
                    }
                }
            );
    }

    private void setCameraToPlayer(ulong playerID)
    {
        foreach (CustomCharacter player in this.PlayerPrefabs)
        {
            if (UInt64.Parse(player.PlayerID) == playerID)
            {
                this.camera.SetTarget(player);
                this.camera.StartFollowing();
            }
        }
    }

    private void SetSkillAngles(List<SkillInfo> skillsClone)
    {
        var skills = LobbyConnection.Instance.serverSettings.SkillsConfig.Items;

        List<SkillConfigItem> jsonSkills = Utils.ToList(skills);

        float basicSkillInfoAngle = jsonSkills.Exists(skill => skillsClone[0].Equals(skill))
            ? float.Parse(jsonSkills.Find(skill => skillsClone[0].Equals(skill)).Angle)
            : 0;
        skillsClone[0].angle = basicSkillInfoAngle;
        skillsClone[0].skillConeAngle = basicSkillInfoAngle;

        float skill1InfoAngle = jsonSkills.Exists(skill => skillsClone[1].Equals(skill))
            ? float.Parse(jsonSkills.Find(skill => skillsClone[1].Equals(skill)).Angle)
            : 0;
        skillsClone[1].angle = skill1InfoAngle;
        skillsClone[1].skillConeAngle = skill1InfoAngle;
    }

    private List<SkillInfo> InitSkills(CoMCharacter characterInfo)
    {
        List<SkillInfo> skills = new List<SkillInfo>();
        characterInfo.skillsInfo.ForEach(skill =>
        {
            SkillInfo skillClone = Instantiate(skill);
            skillClone.InitWithBackend();
            skills.Add(skillClone);
        });

        return skills;
    }

    public void DestroySkillsClone(CustomCharacter player)
    {
        player
            .GetComponentsInChildren<Skill>()
            .ToList()
            .ForEach(skillInfo => Destroy(skillInfo.GetSkillInfo()));
    }

    private void SetPlayersSkills(ulong clientPlayerId)
    {
        CustomInputManager inputManager = UiCamera.GetComponent<CustomInputManager>();
        inputManager.Setup();

        List<Skill> skillList = new List<Skill>();
        foreach (CustomCharacter player in this.PlayerPrefabs)
        {
            SkillBasic skillBasic = player.gameObject.AddComponent<SkillBasic>();
            Skill1 skill1 = player.gameObject.AddComponent<Skill1>();

            skillList.Add(skillBasic);
            skillList.Add(skill1);

            string selectedCharacter = SocketConnectionManager.Instance.selectedCharacters[
                UInt64.Parse(player.PlayerID)
            ];
            CoMCharacter characterInfo = charactersInfo.Find(el => el.name == selectedCharacter);
            SkillAnimationEvents skillsAnimationEvent =
                player.CharacterModel.GetComponent<SkillAnimationEvents>();

            List<SkillInfo> skillInfoClone = InitSkills(characterInfo);
            SetSkillAngles(skillInfoClone);

            skillBasic.SetSkill(Action.BasicAttack, skillInfoClone[0], skillsAnimationEvent);
            skill1.SetSkill(Action.Skill1, skillInfoClone[1], skillsAnimationEvent);

            var items = LobbyConnection.Instance.serverSettings.SkillsConfig.Items;

            foreach (var skill in items)
            {
                for (int i = 0; i < skillList.Count; i++)
                {
                    if (skill.Name.ToLower() == skillList[i].GetSkillName().ToLower())
                    {
                        // 350 in the back is equal to 12 in the front
                        // So this is the calculation
                        skillList[i].SetSkillAreaRadius(float.Parse(skill.SkillRange) / 100);
                    }
                }
            }

            if (UInt64.Parse(player.PlayerID) == clientPlayerId)
            {
                inputManager.InitializeInputSprite(characterInfo);
                inputManager.AssignSkillToInput(
                    UIControls.SkillBasic,
                    skillInfoClone[0].inputType,
                    skillBasic
                );
                inputManager.AssignSkillToInput(
                    UIControls.Skill1,
                    skillInfoClone[1].inputType,
                    skill1
                );
            }

            StartCoroutine(inputManager.ShowInputs());
        }
    }

    private void SetPlayerHealthBar(ulong playerId)
    {
        foreach (CustomCharacter player in this.PlayerPrefabs)
        {
            Image healthBarFront = player
                .GetComponent<MMHealthBar>()
                .TargetProgressBar.ForegroundBar.GetComponent<Image>();
            if (UInt64.Parse(player.PlayerID) == playerId)
            {
                healthBarFront.color = Utils.healthBarCyan;
            }
            else
            {
                healthBarFront.color = Utils.healthBarRed;
            }
        }
    }

    private void ShowRoundTransition()
    {
        bool animate = true;

        roundText.text =
            "Player " + SocketConnectionManager.Instance.winnerPlayer.Item1.Id + " Wins!";
        totalKillsText.text = "Total Kills: " + SocketConnectionManager.Instance.winnerPlayer.Item2;
        backToLobbyButton.SetActive(true);
        animate = false;

        roundSplash.SetActive(true);
        roundSplash.GetComponent<Animator>().SetBool("NewRound", animate);
    }

    private IEnumerator ShowDeathSplash(GameObject player)
    {
        MMFeedbacks deathFeedback = player
            .GetComponent<CustomCharacter>()
            .GetComponent<Health>()
            .DeathMMFeedbacks;
        yield return new WaitForSeconds(DEATH_FEEDBACK_DURATION);
        deathSplash.SetActive(true);
        UiControls.SetActive(false);
    }

    private void SetCameraToAlivePlayer()
    {
        playerToFollow = Utils.GetGamePlayer(KillFeedManager.instance.saveKillerId);
        if (KillFeedManager.instance.saveKillerId != 0)
        {
            StartCoroutine(WaitToChangeCamera(playerToFollow));
        }
        else
        {
            playerToFollow = Utils.GetAlivePlayers().ElementAt(0);
            setCameraToPlayer(playerToFollow.Id);
        }
    }

    private IEnumerator WaitToChangeCamera(Player player)
    {
        yield return new WaitUntil(() => player != null);
        setCameraToPlayer(playerToFollow.Id);
        KillFeedManager.instance.saveKillerId = 0;
    }

    private bool GameHasEndedOrPlayerHasDied(Player gamePlayer)
    {
        return SocketConnectionManager.Instance.GameHasEnded()
            || gamePlayer != null && (gamePlayer.Status == Status.Dead);
    }

    private bool GameHasEnded()
    {
        return SocketConnectionManager.Instance.GameHasEnded();
    }
}
