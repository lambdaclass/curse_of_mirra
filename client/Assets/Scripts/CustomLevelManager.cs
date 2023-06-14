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
        foreach (Character player in this.PlayerPrefabs)
        {
            if (Int32.Parse(player.PlayerID) == playerID)
            {
                UnityEvent aoeEvent = new UnityEvent();
                aoeEvent.AddListener(player.GetComponent<GenericAoeAttack>().ShowAimAoeAttack);
                UiCamera
                    .GetComponent<CustomInputManager>()
                    .AssignInputToAbilityPosition("y", "joystick", aoeEvent);

                UnityEvent<Vector2> aimEvent = new UnityEvent<Vector2>();
                aimEvent.AddListener(player.GetComponent<GenericAoeAttack>().AimAoeAttack);
                UiCamera
                    .GetComponent<CustomInputManager>()
                    .AssignInputToAimPosition("y", "joystick", aimEvent);

                UnityEvent<Vector2> attackEvent = new UnityEvent<Vector2>();
                attackEvent.AddListener(player.GetComponent<GenericAoeAttack>().ExecuteAoeAttack);
                UiCamera
                    .GetComponent<CustomInputManager>()
                    .AssignInputToAbilityExecution("y", "joystick", attackEvent);

                UnityEvent mainAttackEvent = new UnityEvent();
                mainAttackEvent.AddListener(player.GetComponent<DetectNearPlayer>().GetPlayerFaceDirection);
                UiCamera.GetComponent<CustomInputManager>().AssingMainAttack("joystick", mainAttackEvent);
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
