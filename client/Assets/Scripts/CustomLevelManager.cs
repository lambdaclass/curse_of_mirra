using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    GameObject roundSplash;

    [SerializeField]
    Text roundText;

    [SerializeField]
    GameObject backToLobbyButton;
    private List<Player> gamePlayers;
    private int totalPlayers;
    private int playerId;
    public Character prefab;
    public Camera UiCamera;
    public CinemachineCameraController camera;

    int winnersCount = 0;

    protected override void Awake()
    {
        base.Awake();
        this.totalPlayers = LobbyConnection.Instance.playerCount;
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
            quickMapPrefab.SetActive(true);
        }
        else
        {
            mapPrefab = (GameObject)Resources.Load($"Maps/{LobbyManager.LevelSelected}", typeof(GameObject));
            GameObject map = Instantiate(mapPrefab);
            //Add gameobject to the scene root
            map.transform.SetParent(SceneManager.GetActiveScene().GetRootGameObjects()[0].transform.parent);
        }
    }

    private IEnumerator InitializeLevel()
    {
        yield return new WaitUntil(() => SocketConnectionManager.Instance.gamePlayers != null);
        this.gamePlayers = SocketConnectionManager.Instance.gamePlayers;
        GeneratePlayer();
        playerId = LobbyConnection.Instance.playerId;
        setCameraToPlayer(playerId);
        SetInputsAbilities(playerId);
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

    public void GeneratePlayer()
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            if (LobbyConnection.Instance.playerId == i + 1)
            {
                // Player1 is the ID to match with the client InputManager
                prefab.PlayerID = "Player1";
            }
            else
            {
                prefab.PlayerID = "";
            }
            Character newPlayer = Instantiate(
                prefab,
                Utils.transformBackendPositionToFrontendPosition(gamePlayers[i].Position),
                Quaternion.identity
            );
            if (SocketConnectionManager.Instance.playerId == i + 1) {
                SocketConnectionManager.Instance.entityUpdates.lastServerUpdate.playerPosition = Utils.transformBackendPositionToFrontendPosition(gamePlayers[i].Position);
                SocketConnectionManager.Instance.entityUpdates.lastServerUpdate.playerId = SocketConnectionManager.Instance.playerId;
                SocketConnectionManager.Instance.entityUpdates.lastServerUpdate.health = 100;
            }
            newPlayer.name = "Player" + " " + (i + 1);
            newPlayer.PlayerID = (i + 1).ToString();

            SocketConnectionManager.Instance.players.Add(newPlayer.gameObject);
            this.Players.Add(newPlayer);
        }
        this.PlayerPrefabs = (this.Players).ToArray();
    }

    private void setCameraToPlayer(int playerID)
    {
        foreach (Character player in this.PlayerPrefabs)
        {
            if (Int32.Parse(player.PlayerID) == playerID)
            {
                this.camera.SetTarget(player);
                this.camera.StartFollowing();
            }
        }
    }

    private void SetInputsAbilities(int playerID)
    {
        CustomInputManager _cim = UiCamera.GetComponent<CustomInputManager>();
        Player pl = SocketConnectionManager.GetPlayer(playerID, SocketConnectionManager.Instance.gamePlayers);

        foreach (Character player in this.PlayerPrefabs)
        {

            if (Int32.Parse(player.PlayerID) == playerID)
            {
                SkillBasic skillBasic = player.gameObject.AddComponent<SkillBasic>();
                skillBasic.SetSkill(Action.BasicAttack);
                _cim.AssignSkillToInput(UIControls.SkillBasic, UIType.Tap, skillBasic);

                Skill1 skill1 = player.gameObject.AddComponent<Skill1>();
                skill1.SetSkill(Action.Skill1);

                Skill2 skill2 = player.gameObject.AddComponent<Skill2>();
                skill2.SetSkill(Action.Skill2);

                if (pl.CharacterName == "Muflus"){
                    _cim.AssignSkillToInput(UIControls.Skill1, UIType.Tap, skill1);
                    _cim.AssignSkillToInput(UIControls.Skill2, UIType.Area, skill2);
                } else {
                    _cim.AssignSkillToInput(UIControls.Skill1, UIType.Direction, skill1);
                    _cim.AssignSkillToInput(UIControls.Skill2, UIType.Direction, skill2);
                }

                Skill3 skill3 = player.gameObject.AddComponent<Skill3>();
                skill3.SetSkill(Action.Skill4);
                _cim.AssignSkillToInput(UIControls.Skill3, UIType.Tap, skill3);

                // Skill4 skill4 = player.gameObject.AddComponent<Skill4>();
                // skill4.SetSkill(Action.AttackAoe);
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
}
