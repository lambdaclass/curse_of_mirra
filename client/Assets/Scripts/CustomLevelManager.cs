using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private GameObject mapInstance;
    public GameObject quickMapPrefab;

    [SerializeField]
    GameObject deathSplash;
    private ulong totalPlayers = 1;
    private ulong playerId;

    // private GameObject prefab;
    public Camera UiCamera;

    // public OldPlayer playerToFollow;

    [SerializeField]
    public GameObject UiControls;
    public CinemachineCameraController camera;

    private ulong playerToFollowId;
    public List<GameObject> mapList = new List<GameObject>();
    private bool deathSplashIsShown = false;
    EndGameManager endGameManager;

    protected override void Awake()
    {
        base.Awake();
        // this.totalPlayers = (ulong)ServerConnection.Instance.playerCount;
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
        mapInstance = Instantiate(mapPrefab);
        //Add gameobject to the scene root
        mapInstance
            .transform
            .SetParent(SceneManager.GetActiveScene().GetRootGameObjects()[0].transform.parent);
    }

    public GameObject GetMapInstance()
    {
        return mapInstance;
    }

    private IEnumerator InitializeLevel()
    {
        yield return new WaitUntil(checkPlayerHasJoined);
        this.totalPlayers = (ulong)GameServerConnectionManager.Instance.gamePlayers.Count();
        playerId = GameServerConnectionManager.Instance.playerId;
        playerToFollowId = playerId;
        GeneratePlayers();
        SetPlayersSkills(playerId);
        var player = Utils.GetPlayer(playerId);
        SetOrientationArrow(playerId);

        endGameManager = deathSplash.GetComponentInChildren<EndGameManager>();
        endGameManager.SetDeathSplashCharacter();
    }

    void Update()
    {
        Entity gamePlayer = Utils.GetGamePlayer(playerId);
        GameObject player = Utils.GetPlayer(playerId);
        if (GameHasEndedOrPlayerHasDied(gamePlayer) && !deathSplashIsShown)
        {
            StartCoroutine(ShowDeathSplash(player));
            deathSplashIsShown = true;
        }
        if (GameHasEnded())
        {
            // TODO: Redirect to EndGameScreen
            //SceneManager.LoadScene("EndGame");
            endGameManager.finalSplash.SetActive(true);
            endGameManager.ShowCharacterAnimation();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GUIManager.Instance.SetPauseScreen(paused == false ? true : false);
            paused = !paused;
        }

        // if (gamePlayer != null && gamePlayer.Player.Health <= 0)
        // {
        //     SetCameraToAlivePlayer();
        // }
    }

    // private GameObject GetCharacterPrefab(ulong playerId)
    // {
    //     GameObject prefab = null;

    //     OldPlayer player = Utils.GetGamePlayer(playerId);
    //     prefab = charactersInfo.Find(el => el.name == player.CharacterName).prefab;
    //     return prefab;
    // }

    private void GeneratePlayers()
    {
        // prefab = prefab == null ? quickGamePrefab : prefab;
        foreach (Entity player in GameServerConnectionManager.Instance.gamePlayers)
        {
            GameObject prefab = CharactersManager
                .Instance
                .AvailableCharacters
                .Find(el => el.name.ToLower() == player.Player.CharacterName)
                .prefab;

            if (GameServerConnectionManager.Instance.playerId == player.Id)
            {
                // Player1 is the ID to match with the client InputManager
                prefab.GetComponent<CustomCharacter>().PlayerID = "Player1";
            }
            else
            {
                prefab.GetComponent<CustomCharacter>().PlayerID = "";
            }
            Vector3 backToFrontPosition = Utils.transformBackendOldPositionToFrontendPosition(
                player.Position
            );
            CustomCharacter newPlayer = Instantiate(
                prefab.GetComponent<CustomCharacter>(),
                new Vector3(backToFrontPosition.x, 1.0f, backToFrontPosition.z),
                Quaternion.identity
            );

            if (GameServerConnectionManager.Instance.playerId == player.Id)
            {
                Instantiate(
                    newPlayer.characterBase.StaminaCharges,
                    newPlayer.characterBase.CanvasHolder.transform
                );
            }
            newPlayer.CharacterHealth.InitialHealth = player.Player.Health;
            newPlayer.CharacterHealth.MaximumHealth = player.Player.Health;
            newPlayer.name = "Player" + player.Id;
            newPlayer.PlayerID = player.Id.ToString();
            SetPlayerHealthBar(GameServerConnectionManager.Instance.playerId == player.Id, newPlayer);
            GameServerConnectionManager.Instance.players.Add(newPlayer.gameObject);
            this.Players.Add(newPlayer);
        }
        this.PlayerPrefabs = (this.Players).ToArray();
    }

    int RoundUpByTen(int i)
    {
        return (int)(Math.Ceiling(i / 10.0d) * 10);
    }

    private void SetOrientationArrow(ulong playerID)
    {
        foreach (CustomCharacter player in this.PlayerPrefabs)
        {
            player
                .GetComponentInChildren<CharacterBase>()
                .OrientationArrow
                .SetActive(UInt64.Parse(player.PlayerID) == playerID);
        }
    }

    private void SetCameraToPlayer(ulong playerID)
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

    // private void SetSkillAngles(List<SkillInfo> skillsClone)
    // {
    //     // var skills = ServerConnection.Instance.engineServerSettings.Skills;

    //     // List<SkillConfigItem> jsonSkills = Utils.ToList(skills);

    //     // float basicSkillInfoAngle = jsonSkills.Exists(skill => skillsClone[0].Equals(skill))
    //     //     ? float.Parse(jsonSkills.Find(skill => skillsClone[0].Equals(skill)).Mecha.Angle)
    //     //     : 0;
    //     skillsClone[0].angle = 45; // basicSkillInfoAngle;
    //     skillsClone[0].skillConeAngle = 45; // basicSkillInfoAngle;

    //     // float skill1InfoAngle = jsonSkills.Exists(skill => skillsClone[1].Equals(skill))
    //     //     ? float.Parse(jsonSkills.Find(skill => skillsClone[1].Equals(skill)).Angle)
    //     //     : 0;
    //     skillsClone[1].angle = 45; // skill1InfoAngle;
    //     skillsClone[1].skillConeAngle = 45; // skill1InfoAngle;
    // }

    private List<SkillInfo> InitSkills(CoMCharacter characterInfo, string id)
    {
        List<SkillInfo> skills = new List<SkillInfo>();
        characterInfo
            .skillsInfo
            .ForEach(skill =>
            {
                SkillInfo skillClone = Instantiate(skill);
                skillClone.InitWithBackend(id);
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
            Skill1 skill1 = player.gameObject.AddComponent<Skill1>();
            Skill2 skill2 = player.gameObject.AddComponent<Skill2>();
            Skill3 skill3 = player.gameObject.AddComponent<Skill3>();

            skillList.Add(skill1);
            skillList.Add(skill2);
            skillList.Add(skill3);

            CoMCharacter characterInfo = CharactersManager
                .Instance
                .AvailableCharacters
                .Find(el => el.name.ToLower() == player.CharacterModel.name.ToLower());

            List<SkillInfo> skillInfoClone = InitSkills(characterInfo, player.PlayerID);
            // SetSkillAngles(skillInfoClone);

            skill1.SetSkill("1", skillInfoClone[0]);
            skill2.SetSkill("2", skillInfoClone[1]);
            skill3.SetSkill("3", skillInfoClone[2]);

            // var skills = ServerConnection.Instance.engineServerSettings.Skills;

            if (UInt64.Parse(player.PlayerID) == clientPlayerId)
            {
                inputManager.InitializeInputSprite(characterInfo);
                inputManager.AssignSkillToInput(
                    UIControls.Skill1,
                    skillInfoClone[0].inputType,
                    skill1
                );
                inputManager.AssignSkillToInput(
                    UIControls.Skill2,
                    skillInfoClone[1].inputType,
                    skill2
                );
                inputManager.AssignSkillToInput(
                    UIControls.Skill3,
                    skillInfoClone[2].inputType,
                    skill3
                );
            }

            StartCoroutine(inputManager.ShowInputs());
        }
    }

    private void SetPlayerHealthBar(bool isClientId ,Character character)
    {
            Image healthBarFront = character
                .GetComponent<MMHealthBar>()
                .TargetProgressBar
                .ForegroundBar
                .GetComponent<Image>();
           
                healthBarFront.color = isClientId ? Utils.healthBarRed :  Utils.healthBarCyan;
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

    // private void SetCameraToAlivePlayer()
    // {
    //     playerToFollow = Utils.GetGamePlayer(KillFeedManager.instance.saveKillerId);
    //     if (KillFeedManager.instance.saveKillerId != 0)
    //     {
    //         StartCoroutine(WaitToChangeCamera(playerToFollow));
    //     }
    //     else
    //     {
    //         playerToFollow = Utils.GetAlivePlayers().ElementAt(0);
    //         setCameraToPlayer(playerToFollow.Id);
    //     }
    // }

    // private IEnumerator WaitToChangeCamera(OldPlayer player)
    // {
    //     yield return new WaitUntil(() => player != null);
    //     setCameraToPlayer(playerToFollow.Id);
    //     KillFeedManager.instance.saveKillerId = 0;
    // }

    private bool GameHasEndedOrPlayerHasDied(Entity gamePlayer)
    {
        return GameServerConnectionManager.Instance.GameHasEnded()
            || gamePlayer != null && (gamePlayer.Player.Health == 0);
    }

    private bool GameHasEnded()
    {
        return GameServerConnectionManager.Instance.GameHasEnded();
    }

    private bool checkPlayerHasJoined()
    {
        return GameServerConnectionManager.Instance.gamePlayers != null
            && GameServerConnectionManager
                .Instance
                .gamePlayers
                .Any((player) => player.Id == GameServerConnectionManager.Instance.playerId);
    }
}
