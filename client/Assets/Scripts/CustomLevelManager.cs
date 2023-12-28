using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Communication.Protobuf;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;

public class CustomLevelManager : LevelManager
{
    private const float DEATH_FEEDBACK_DURATION = 1.5f;
    bool paused = false;
    private GameObject mapPrefab;
    public GameObject quickMapPrefab;

    [SerializeField]
    GameObject deathSplash;

    [SerializeField]
    GameObject cardList;

    [SerializeField]
    List<GameObject> cards;

    private List<OldPlayer> gamePlayers;
    private ulong totalPlayers;
    private ulong playerId;
    private GameObject prefab;
    public Camera UiCamera;
    public OldPlayer playerToFollow;

    [SerializeField]
    public GameObject UiControls;
    public Camera camera;
    private ulong playerToFollowId;
    public List<CoMCharacter> charactersInfo = new List<CoMCharacter>();
    public List<GameObject> mapList = new List<GameObject>();

    //Camera cinematic variables
    [SerializeField]
    GameObject loadingScreen;

    Int32 CAMERA_OFFSET = 30;
    Int32 CAMERA_Y_OFFSET = 6;
    double xDigit = 0;
    double zDigit = 0;
    CinemachineFramingTransposer cameraFramingTransposer = null;
    private bool deathSplashIsShown = false;
    EndGameManager endGameManager;

    protected override void Awake()
    {
        base.Awake();
        // this.totalPlayers = (ulong)LobbyConnection.Instance.playerCount;
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
        yield return new WaitUntil(checkPlayerHasJoined);
        this.gamePlayers = SocketConnectionManager.Instance.gamePlayers;
        this.totalPlayers = (ulong)this.gamePlayers.Count();
        playerId = SocketConnectionManager.Instance.playerId;
        playerToFollowId = playerId;
        GeneratePlayers();
        SetPlayersSkills(playerId);
        setCameraToPlayer(playerId);

        SetPlayerHealthBar(playerId);
        SetOrientationArrow(playerId);
        StartCoroutine(CameraCinematic());

        endGameManager = deathSplash.GetComponentInChildren<EndGameManager>();
    }

    void Update()
    {
        OldPlayer gamePlayer = Utils.GetGamePlayer(playerId);
        GameObject player = Utils.GetPlayer(playerId);
        foreach (CardUpdate card in cardList.GetComponentsInChildren<CardUpdate>())
        {
            card.GetComponentInChildren<TextMeshProUGUI>().text = Utils
                .GetGamePlayer(card.playerId)
                .Health.ToString();
        }

        if (GameHasEndedOrPlayerHasDied(gamePlayer) && !deathSplashIsShown)
        {
            StartCoroutine(ShowDeathSplash(player));
            deathSplashIsShown = true;
        }
        if (GameHasEnded())
        {
            endGameManager.finalSplash.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GUIManager.Instance.SetPauseScreen(paused == false ? true : false);
            paused = !paused;
        }

        if (gamePlayer != null && gamePlayer.Health <= 0)
        {
            SetCameraToAlivePlayer();
        }
    }

    private GameObject GetCharacterPrefab(ulong playerId)
    {
        GameObject prefab = null;

        OldPlayer player = Utils.GetGamePlayer(playerId);
        prefab = charactersInfo.Find(el => el.name == player.CharacterName).prefab;
        return prefab;
    }

    private void GeneratePlayers()
    {
        // prefab = prefab == null ? quickGamePrefab : prefab;
        for (ulong i = 0; i < totalPlayers; i++)
        {
            ulong playerID = gamePlayers[(int)i].Id;
            prefab = GetCharacterPrefab(playerID);
            if (SocketConnectionManager.Instance.playerId == playerID)
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
                Utils.transformBackendOldPositionToFrontendPosition(gamePlayers[(int)i].Position),
                Quaternion.identity
            );
            newPlayer.name = "Player" + " " + (i + 1);
            newPlayer.PlayerID = playerID.ToString();

            SocketConnectionManager.Instance.players.Add(newPlayer.gameObject);
            this.Players.Add(newPlayer);
            GameObject card = Instantiate(cards[(int)i], cardList.transform);
            card.GetComponent<CardUpdate>().playerId = gamePlayers[(int)i].Id;
            card.GetComponentInChildren<TextMeshProUGUI>().text = gamePlayers[
                (int)i
            ].Health.ToString();
        }
        this.PlayerPrefabs = (this.Players).ToArray();
    }

    IEnumerator CameraCinematic()
    {
        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(false);
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

    private void SetOrientationArrow(ulong playerID)
    {
        foreach (CustomCharacter player in this.PlayerPrefabs)
        {
            player
                .GetComponentInChildren<CharacterBase>()
                .OrientationArrow.SetActive(UInt64.Parse(player.PlayerID) == playerID);
        }
    }

    private void setCameraToPlayer(ulong playerID)
    {
        foreach (CustomCharacter player in this.PlayerPrefabs)
        {
            if (UInt64.Parse(player.PlayerID) == playerID)
            {
                GameObject gamePlayer = Utils.GetPlayer(playerId);
                camera.transform.rotation = Quaternion.Euler(
                    36.5f,
                    (int.Parse(player.PlayerID) * -90f) + 90f,
                    0
                );
                camera.transform.position = gamePlayer.transform.position;
                camera.transform.position -= camera.transform.forward * 28f;
                camera.transform.position += camera.transform.up * 6.5f;
            }
        }
    }

    private void SetSkillAngles(List<SkillInfo> skillsClone)
    {
        // var skills = LobbyConnection.Instance.engineServerSettings.Skills;

        // List<SkillConfigItem> jsonSkills = Utils.ToList(skills);

        // float basicSkillInfoAngle = jsonSkills.Exists(skill => skillsClone[0].Equals(skill))
        //     ? float.Parse(jsonSkills.Find(skill => skillsClone[0].Equals(skill)).Mecha.Angle)
        //     : 0;
        skillsClone[0].angle = 45; // basicSkillInfoAngle;
        skillsClone[0].skillConeAngle = 45; // basicSkillInfoAngle;
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

            skillList.Add(skillBasic);

            CoMCharacter characterInfo = charactersInfo.Find(
                el => el.name == Utils.GetGamePlayer(UInt64.Parse(player.PlayerID)).CharacterName
            );

            List<SkillInfo> skillInfoClone = InitSkills(characterInfo);
            SetSkillAngles(skillInfoClone);

            skillBasic.SetSkill(Communication.Protobuf.Action.BasicAttack, skillInfoClone[0]);

            var skills = LobbyConnection.Instance.engineServerSettings.Skills;

            // foreach (var skill in skills)
            // {
            //     for (int i = 0; i < skillList.Count; i++)
            //     {
            //         if (skill.Name.ToLower() == skillList[i].GetSkillName().ToLower())
            //         {
            //             // 350 in the back is equal to 12 in the front
            //             // So this is the calculation
            //             skillList[i].SetSkillAreaRadius(float.Parse(skill.SkillRange) / 100);
            //         }
            //     }
            // }

            if (UInt64.Parse(player.PlayerID) == clientPlayerId)
            {
                inputManager.InitializeInputSprite(characterInfo);
                inputManager.AssignSkillToInput(
                    UIControls.SkillBasic,
                    skillInfoClone[0].inputType,
                    skillBasic
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
        if (Utils.GetGamePlayer(KillFeedManager.instance.saveKillerId) != null)
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
    }

    private IEnumerator WaitToChangeCamera(OldPlayer player)
    {
        yield return new WaitUntil(() => player != null);
        setCameraToPlayer(playerToFollow.Id);
        KillFeedManager.instance.saveKillerId = 0;
    }

    private bool GameHasEndedOrPlayerHasDied(OldPlayer gamePlayer)
    {
        return SocketConnectionManager.Instance.GameHasEnded()
            || gamePlayer != null && (gamePlayer.Status == OldStatus.Dead);
    }

    private bool GameHasEnded()
    {
        return SocketConnectionManager.Instance.GameHasEnded();
    }

    private bool checkPlayerHasJoined()
    {
        return SocketConnectionManager.Instance.gamePlayers != null
            && SocketConnectionManager.Instance.playerId != null
            && SocketConnectionManager.Instance.gamePlayers.Any(
                (player) => player.Id == SocketConnectionManager.Instance.playerId
            );
    }
}
