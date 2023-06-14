using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.UI;

public class CustomLevelManager : LevelManager
{
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

    bool paused = false;

    protected override void Awake()
    {
        base.Awake();
        this.totalPlayers = LobbyConnection.Instance.playerCount;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InitializeLevel());
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
        CustomInputManager _cim = UiCamera.GetComponent<CustomInputManager>();

        foreach (Character player in this.PlayerPrefabs)
        {
            if (Int32.Parse(player.PlayerID) == playerID)
            {
                AttackBasic attackBasic = player.gameObject.AddComponent<AttackBasic>();
                attackBasic.SetAbility(Action.AttackAoe);
                _cim.AssignAbilityToInput(UIControls.AttackBasic, UIType.Tap, attackBasic);

                Attack1 attack1 = player.gameObject.AddComponent<Attack1>();
                attack1.SetAbility(Action.AttackAoe);
                _cim.AssignAbilityToInput(UIControls.Attack1, UIType.Area, attack1);

                Attack2 attack2 = player.gameObject.AddComponent<Attack2>();
                attack2.SetAbility(Action.AttackAoe);
                _cim.AssignAbilityToInput(UIControls.Attack2, UIType.Tap, attack2);

                Attack3 attack3 = player.gameObject.AddComponent<Attack3>();
                attack3.SetAbility(Action.AttackAoe);
                _cim.AssignAbilityToInput(UIControls.Attack3, UIType.Direction, attack3);

                // Attack4 attack4 = player.gameObject.AddComponent<Attack4>();
                // attack4.SetAbility(Action.AttackAoe);
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
