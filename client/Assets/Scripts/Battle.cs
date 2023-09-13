using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public HashSet<SkillInfo> skillInfoSet;

    [SerializeField]
    MMTouchJoystick joystickL;

    [SerializeField]
    CustomInputManager InputManager;

    public bool showClientPredictionGhost;
    public bool showInterpolationGhosts;
    public List<GameObject> InterpolationGhosts = new List<GameObject>();
    public GameObject clientPredictionGhost;
    public bool useClientPrediction;
    public bool useInterpolation;
    public CharacterStates.MovementStates[] BlockingMovementStates;
    public CharacterStates.CharacterConditions[] BlockingConditionStates;
    public long accumulatedTime;
    public long firstTimestamp;

    private Loot loot;

    // We do this to only have the state effects in the enum instead of all the effects
    private enum StateEffects
    {
        Slowed = PlayerEffect.Slowed,
    }

    void Start()
    {
        InitBlockingStates();
        float clientActionRate = SocketConnectionManager.Instance.serverTickRate_ms / 1000f;
        InvokeRepeating("SendPlayerMovement", clientActionRate, clientActionRate);
        SetupInitialState();
        StartCoroutine(InitializeProjectiles());
        loot = GetComponent<Loot>();
    }

    private void InitBlockingStates()
    {
        BlockingMovementStates = new CharacterStates.MovementStates[1];
        BlockingMovementStates[0] = CharacterStates.MovementStates.Attacking;
    }

    private void SetupInitialState()
    {
        useClientPrediction = true;
        useInterpolation = true;
        accumulatedTime = 0;
        showClientPredictionGhost = false;
        showInterpolationGhosts = false;
    }

    private IEnumerator InitializeProjectiles()
    {
        yield return new WaitUntil(() => SocketConnectionManager.Instance.players.Count > 0);
        CreateProjectilesPoolers();
    }

    void CreateProjectilesPoolers()
    {
        skillInfoSet = new HashSet<SkillInfo>();
        foreach (GameObject player in SocketConnectionManager.Instance.players)
        {
            skillInfoSet.UnionWith(
                player
                    .GetComponents<Skill>()
                    .Select(skill => skill.GetSkillInfo())
                    .Where(skill => skill.projectilePrefab != null)
            );
        }
        GetComponent<ProjectileHandler>().CreateProjectilesPoolers(skillInfoSet);
    }

    void Update()
    {
        if (
            SocketConnectionManager.Instance.gamePlayers != null
            && SocketConnectionManager.Instance.players.Count > 0
            && SocketConnectionManager.Instance.gamePlayers.Count > 0
        )
        {
            SetAccumulatedTime();
            UpdatePlayerActions();
            UpdateProjectileActions();
            loot.UpdateLoots();
        }
    }

    private void SetAccumulatedTime()
    {
        if (firstTimestamp == 0)
        {
            firstTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
        var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        accumulatedTime = (currentTimestamp - firstTimestamp);
    }

    public bool PlayerMovementAuthorized(Character character)
    {
        if ((BlockingMovementStates != null) && (BlockingMovementStates.Length > 0))
        {
            for (int i = 0; i < BlockingMovementStates.Length; i++)
            {
                if (BlockingMovementStates[i] == (character.MovementState.CurrentState))
                {
                    return false;
                }
            }
        }

        if ((BlockingConditionStates != null) && (BlockingConditionStates.Length > 0))
        {
            for (int i = 0; i < BlockingConditionStates.Length; i++)
            {
                if (BlockingConditionStates[i] == (character.ConditionState.CurrentState))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void SendPlayerMovement()
    {
        GameObject player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);
        GameEvent lastEvent = SocketConnectionManager.Instance.eventsBuffer.lastEvent();
        Player playerUpdate = lastEvent.Players
            .ToList()
            .Find(p => p.Id == SocketConnectionManager.Instance.playerId);

        if (player)
        {
            CustomCharacter character = player.GetComponent<CustomCharacter>();
            if (PlayerMovementAuthorized(character))
            {
                var inputFromVirtualJoystick = joystickL is not null;
                if (
                    inputFromVirtualJoystick
                    && (joystickL.RawValue.x != 0 || joystickL.RawValue.y != 0)
                )
                {
                    GetComponent<PlayerControls>()
                        .SendJoystickValues(joystickL.RawValue.x, joystickL.RawValue.y);
                }
                else
                {
                    GetComponent<PlayerControls>().SendAction();
                }
            }
        }
    }

    void UpdatePlayerActions()
    {
        long currentTime;
        long pastTime;
        GameObject interpolationGhost = null;
        EventsBuffer buffer = SocketConnectionManager.Instance.eventsBuffer;
        GameEvent gameEvent;

        currentTime = buffer.firstTimestamp + accumulatedTime;
        pastTime = currentTime - buffer.deltaInterpolationTime;

        if (buffer.firstTimestamp == 0)
        {
            buffer.firstTimestamp = buffer.lastEvent().ServerTimestamp;
        }

        for (int i = 0; i < SocketConnectionManager.Instance.gamePlayers.Count; i++)
        {
            if (showInterpolationGhosts)
            {
                interpolationGhost = FindGhostPlayer(
                    SocketConnectionManager.Instance.gamePlayers[i].Id.ToString()
                );
            }

            if (
                useInterpolation
                && (
                    SocketConnectionManager.Instance.playerId
                        != SocketConnectionManager.Instance.gamePlayers[i].Id
                    || !useClientPrediction
                )
            )
            {
                gameEvent = buffer.getNextEventToRender(pastTime);
            }
            else
            {
                gameEvent = buffer.lastEvent();
            }

            // There are a few frames during which this is outdated and produces an error
            if (SocketConnectionManager.Instance.gamePlayers.Count == gameEvent.Players.Count)
            {
                // This call to `new` here is extremely important for client prediction. If we don't make a copy,
                // prediction will modify the player in place, which is not what we want.
                Player serverPlayerUpdate = new Player(gameEvent.Players[i]);

                if (
                    serverPlayerUpdate.Id == (ulong)SocketConnectionManager.Instance.playerId
                    && useClientPrediction
                )
                {
                    // Move the ghost BEFORE client prediction kicks in, so it only moves up until
                    // the last server update.
                    if (clientPredictionGhost != null)
                    {
                        UpdatePlayer(clientPredictionGhost, serverPlayerUpdate, pastTime);
                    }
                    SocketConnectionManager.Instance.clientPrediction.simulatePlayerState(
                        serverPlayerUpdate,
                        gameEvent.PlayerTimestamp
                    );
                }

                if (interpolationGhost != null)
                {
                    UpdatePlayer(interpolationGhost, buffer.lastEvent().Players[i], pastTime);
                }

                GameObject actualPlayer = Utils.GetPlayer(serverPlayerUpdate.Id);

                if (actualPlayer.activeSelf)
                {
                    if (serverPlayerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Paralyzed))
                    {
                        UpdatePlayer(actualPlayer, buffer.lastEvent().Players[i], pastTime);
                    }
                    else
                    {
                        UpdatePlayer(actualPlayer, serverPlayerUpdate, pastTime);
                    }

                    if (
                        !buffer.timestampAlreadySeen(
                            SocketConnectionManager.Instance.gamePlayers[i].Id,
                            gameEvent.ServerTimestamp
                        )
                    )
                    {
                        executeSkillFeedback(
                            actualPlayer,
                            serverPlayerUpdate.Action,
                            serverPlayerUpdate.Direction
                        );
                        buffer.setLastTimestampSeen(
                            SocketConnectionManager.Instance.gamePlayers[i].Id,
                            gameEvent.ServerTimestamp
                        );
                    }
                }

                // TODO: try to optimize GetComponent calls
                CustomCharacter playerCharacter = actualPlayer.GetComponent<CustomCharacter>();

                if (serverPlayerUpdate.Health <= 0)
                {
                    SetPlayerDead(playerCharacter);
                }

                if (serverPlayerUpdate.Id != SocketConnectionManager.Instance.playerId)
                {
                    // TODO: Refactor: create a script/reference.
                    actualPlayer
                        .GetComponent<CustomCharacter>()
                        .characterBase.Position.GetComponent<Renderer>()
                        .material.color = new Color(1, 0, 0, .5f);
                }

                Transform hitbox = actualPlayer
                    .GetComponent<CustomCharacter>()
                    .characterBase.Hitbox.transform;

                float hitboxSize = serverPlayerUpdate.BodySize / 50f;
                hitbox.localScale = new Vector3(hitboxSize, hitbox.localScale.y, hitboxSize);
            }
        }
    }

    private void executeSkillFeedback(
        GameObject actualPlayer,
        PlayerAction playerAction,
        RelativePosition direction
    )
    {
        if (actualPlayer.name.Contains("BOT"))
        {
            return;
        }
        // TODO: Refactor
        switch (playerAction)
        {
            case PlayerAction.Attacking:
                actualPlayer.GetComponent<SkillBasic>().ExecuteFeedback();
                rotatePlayer(actualPlayer, direction);
                break;
            case PlayerAction.StartingSkill1:
                actualPlayer.GetComponent<Skill1>().StartFeedback();
                rotatePlayer(actualPlayer, direction);
                break;
            case PlayerAction.ExecutingSkill1:
                actualPlayer.GetComponent<Skill1>().ExecuteFeedback();
                rotatePlayer(actualPlayer, direction);
                break;
            case PlayerAction.StartingSkill2:
                actualPlayer.GetComponent<Skill2>().StartFeedback();
                rotatePlayer(actualPlayer, direction);
                break;
            case PlayerAction.ExecutingSkill2:
                actualPlayer.GetComponent<Skill2>().ExecuteFeedback();
                rotatePlayer(actualPlayer, direction);
                break;
            case PlayerAction.StartingSkill3:
                actualPlayer.GetComponent<Skill3>().StartFeedback();
                rotatePlayer(actualPlayer, direction);
                break;
            case PlayerAction.ExecutingSkill3:
                actualPlayer.GetComponent<Skill3>().ExecuteFeedback();
                rotatePlayer(actualPlayer, direction);
                break;
            case PlayerAction.StartingSkill4:
                actualPlayer.GetComponent<Skill4>().StartFeedback();
                rotatePlayer(actualPlayer, direction);
                break;
            case PlayerAction.ExecutingSkill4:
                actualPlayer.GetComponent<Skill4>().ExecuteFeedback();
                rotatePlayer(actualPlayer, direction);
                break;
        }
    }

    void UpdateProjectileActions()
    {
        Dictionary<int, GameObject> projectiles = SocketConnectionManager.Instance.projectiles;
        List<Projectile> gameProjectiles = SocketConnectionManager.Instance.gameProjectiles;
        ClearProjectiles(projectiles, gameProjectiles);
        ProcessProjectilesCollision(projectiles, gameProjectiles);
        UpdateProjectiles(projectiles, gameProjectiles);
    }

    void UpdateProjectiles(
        Dictionary<int, GameObject> projectiles,
        List<Projectile> gameProjectiles
    )
    {
        GameObject projectile;
        for (int i = 0; i < gameProjectiles.Count; i++)
        {
            if (projectiles.TryGetValue((int)gameProjectiles[i].Id, out projectile))
            {
                Vector3 backToFrontPosition = Utils.transformBackendPositionToFrontendPosition(
                    gameProjectiles[i].Position
                );

                projectile
                    .GetComponent<SkillProjectile>()
                    .UpdatePosition(
                        new Vector3(backToFrontPosition[0], 3f, backToFrontPosition[2])
                    );
            }
            else if (gameProjectiles[i].Status == ProjectileStatus.Active)
            {
                float angle = Vector3.SignedAngle(
                    new Vector3(1f, 0, 0),
                    new Vector3(
                        (long)(gameProjectiles[i].Direction.Y * 100),
                        0f,
                        -(long)(gameProjectiles[i].Direction.X * 100)
                    ),
                    Vector3.up
                );
                GameObject projectileFromSkill = skillInfoSet
                    .Single(skill => skill.name == gameProjectiles[i].SkillName)
                    .projectilePrefab;
                GameObject skillProjectile = GetComponent<ProjectileHandler>()
                    .InstanceProjectile(projectileFromSkill, angle);

                projectiles.Add((int)gameProjectiles[i].Id, skillProjectile);
            }
        }
    }

    void ClearProjectiles(Dictionary<int, GameObject> projectiles, List<Projectile> gameProjectiles)
    {
        foreach (int projectileId in projectiles.Keys.ToList())
        {
            if (!gameProjectiles.Exists(x => (int)x.Id == projectileId))
            {
                projectiles[projectileId].GetComponent<SkillProjectile>().Remove();
                projectiles.Remove(projectileId);
            }
        }
    }

    void ProcessProjectilesCollision(
        Dictionary<int, GameObject> projectiles,
        List<Projectile> gameProjectiles
    )
    {
        foreach (var pr in projectiles.ToList())
        {
            Projectile gameProjectile = gameProjectiles.Find(x => (int)x.Id == pr.Key);
            if (gameProjectile.Status == ProjectileStatus.Exploded)
            {
                pr.Value.GetComponent<SkillProjectile>().ProcessCollision();
                projectiles.Remove(pr.Key);
            }
        }
    }

    private void rotatePlayer(GameObject player, RelativePosition direction)
    {
        CharacterOrientation3D characterOrientation = player.GetComponent<CharacterOrientation3D>();
        characterOrientation.ForcedRotation = true;
        Vector3 movementDirection = new Vector3(direction.X, 0f, direction.Y);
        movementDirection.Normalize();
        characterOrientation.ForcedRotationDirection = movementDirection;
    }

    private void UpdatePlayer(GameObject player, Player playerUpdate, long pastTime)
    {
        /*
        Player has a speed of 3 tiles per tick. A tile in unity is 0.3f a distance of 0.3f.
        There are 50 ticks per second. A player's velocity is 50 * 0.3f

        In general, if a player's velocity is n tiles per tick, their unity velocity
        is 50 * (n / 10f)

        The above is the player's velocity's magnitude. Their velocity's direction
        is the direction of deltaX, which we can calculate (assumming we haven't lost socket
        frames, but that's fine).
        */
        CustomCharacter character = player.GetComponent<CustomCharacter>();
        var characterSpeed = PlayerControls.getBackendCharacterSpeed(playerUpdate.Id) / 100f;
        Animator modelAnimator = player
            .GetComponent<CustomCharacter>()
            .CharacterModel.GetComponent<Animator>();

        characterSpeed = ManageStateFeedbacks(player, playerUpdate, character, characterSpeed);

        HandleMovement(player, playerUpdate, pastTime, characterSpeed);

        HandlePlayerHealth(player, playerUpdate);

        if (playerUpdate.Id == SocketConnectionManager.Instance.playerId)
        {
            /*
                - We divided the milliseconds time in two parts because
                - rustler can't handle u128, so instead of developing those functions
                - we decided to use 2 u64 fields to represent the time in milliseconds

                - If you need to use complete time in milliseconds, you should use both
                - If you need to use remaining time in milliseconds, you can use only low field
                - because high field will be 0
            */
            InputManager.CheckSkillCooldown(
                UIControls.SkillBasic,
                (float)playerUpdate.BasicSkillCooldownLeft.Low / 1000f,
                player.GetComponent<SkillBasic>().GetSkillInfo().showCooldown
            );
            InputManager.CheckSkillCooldown(
                UIControls.Skill1,
                (float)playerUpdate.Skill1CooldownLeft.Low / 1000f,
                player.GetComponent<Skill1>().GetSkillInfo().showCooldown
            );
            InputManager.CheckSkillCooldown(
                UIControls.Skill2,
                (float)playerUpdate.Skill2CooldownLeft.Low / 1000f,
                player.GetComponent<Skill2>().GetSkillInfo().showCooldown
            );
            InputManager.CheckSkillCooldown(
                UIControls.Skill3,
                (float)playerUpdate.Skill3CooldownLeft.Low / 1000f,
                player.GetComponent<Skill3>().GetSkillInfo().showCooldown
            );
            InputManager.CheckSkillCooldown(
                UIControls.Skill4,
                (float)playerUpdate.Skill4CooldownLeft.Low / 1000f,
                player.GetComponent<Skill4>().GetSkillInfo().showCooldown
            );
        }
    }

    private void HandlePlayerHealth(GameObject player, Player playerUpdate)
    {
        Health healthComponent = player.GetComponent<Health>();

        // Display damage done on you on your client
        GetComponent<PlayerFeedbacks>()
            .DisplayDamageRecieved(player, healthComponent, playerUpdate.Health, playerUpdate.Id);

        // Display damage done on others players (not you)
        GetComponent<PlayerFeedbacks>()
            .ChangePlayerTextureOnDamage(
                player,
                healthComponent.CurrentHealth,
                playerUpdate.Health
            );

        if (playerUpdate.Health != healthComponent.CurrentHealth)
        {
            healthComponent.SetHealth(playerUpdate.Health);
        }
    }

    private void HandleMovement(
        GameObject player,
        Player playerUpdate,
        long pastTime,
        float characterSpeed
    )
    {
        // This is tickRate * characterSpeed. Once we decouple tickRate from speed on the backend
        // it'll be changed.
        float tickRate = 1000f / SocketConnectionManager.Instance.serverTickRate_ms;
        float velocity = tickRate * characterSpeed;

        var frontendPosition = Utils.transformBackendPositionToFrontendPosition(
            playerUpdate.Position
        );

        float xChange = frontendPosition.x - player.transform.position.x;
        float yChange = frontendPosition.z - player.transform.position.z;

        Animator modelAnimator = player
            .GetComponent<CustomCharacter>()
            .CharacterModel.GetComponent<Animator>();

        bool walking = false;

        if (playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Paralyzed))
        {
            if (player.transform.position != frontendPosition)
            {
                player.transform.position = new Vector3(
                    frontendPosition.x,
                    player.transform.position.y,
                    frontendPosition.z
                );
            }
            modelAnimator.SetBool("Walking", walking);
            return;
        }

        if (useClientPrediction)
        {
            walking =
                playerUpdate.Id == SocketConnectionManager.Instance.playerId
                    ? InputsAreBeingUsed()
                    : SocketConnectionManager.Instance.eventsBuffer.playerIsMoving(
                        playerUpdate.Id,
                        (long)pastTime
                    );
        }
        else
        {
            if (playerUpdate.Id == SocketConnectionManager.Instance.playerId)
            {
                walking = SocketConnectionManager.Instance.eventsBuffer.playerIsMoving(
                    playerUpdate.Id,
                    (long)pastTime
                );
            }
        }

        Vector2 movementChange = new Vector2(xChange, yChange);

        if (movementChange.magnitude > 0f)
        {
            Vector3 movementDirection = new Vector3(xChange, 0f, yChange);
            movementDirection.Normalize();

            // FIXME: Remove harcoded validation once is fixed on the backend.
            if (
                playerUpdate.CharacterName == "Muflus"
                && playerUpdate.Action == PlayerAction.ExecutingSkill3
            )
            {
                player.transform.position = frontendPosition;
            }
            else
            {
                // The idea here is, when moving, we never want to go past the position the backend is telling us we are in.
                // Let's say the movementChange vector is (1, 0), i.e., we are moving horizontally to the right.
                // Let's also say frontendPosition is (2, y, 1)
                // If newPosition is (2.1, y, 1), we want it to just be (2, y, 1).
                // In this case, all we are doing is saying that the `x` coordinate should be min(2, newPosition.x)
                // If the movement were left, we would take max(2, newPosition.x)
                // Let's now say that the movement is in the (1, 1) normalized direction, so diagonally up and right.
                // If frontendPosition is (2, y, 1), I can't go past it in the (1, 1) direction. What we need to do here is
                // simply take the `x` coordinate to be min(2, newPosition.x) and the `z` coordinate to be min(1, newPosition.z)

                // In general, if the movementDirection vector is (x, y, z) normalized, then if its `x` coordinate is positive, we should
                // take newPosition.x = min(frontendPosition.x, newPosition.x)
                // If, on the other hand, its `x` coordinate is negative, we take newPosition.x = max(frontendPosition.x, newPosition.x)
                // The exact same thing applies to `z`
                Vector3 newPosition =
                    player.transform.position + movementDirection * velocity * Time.deltaTime;

                if (movementDirection.x > 0)
                {
                    newPosition.x = Math.Min(frontendPosition.x, newPosition.x);
                }
                else
                {
                    newPosition.x = Math.Max(frontendPosition.x, newPosition.x);
                }

                if (movementDirection.z > 0)
                {
                    newPosition.z = Math.Min(frontendPosition.z, newPosition.z);
                }
                else
                {
                    newPosition.z = Math.Max(frontendPosition.z, newPosition.z);
                }
                player.transform.position = newPosition;

                // FIXME: This is a temporary solution to solve unwanted player rotation until we handle movement blocking on backend
                // if the player is in attacking state, movement rotation from movement should be ignored
                RelativePosition direction = GetPlayerDirection(playerUpdate);

                if (PlayerMovementAuthorized(player.GetComponent<CustomCharacter>()))
                {
                    rotatePlayer(player, direction);
                }
            }
            walking = true;
        }

        modelAnimator.SetBool("Walking", walking);
    }

    public void SetPlayerDead(CustomCharacter playerCharacter)
    {
        GetComponent<PlayerFeedbacks>().PlayDeathFeedback(playerCharacter);
        playerCharacter.CharacterModel.SetActive(false);
        playerCharacter.ConditionState.ChangeState(CharacterStates.CharacterConditions.Dead);
        playerCharacter.characterBase.Hitbox.SetActive(false);
        playerCharacter.characterBase.Position.SetActive(false);
    }

    // CLIENT PREDICTION UTILITY FUNCTIONS , WE USE THEM IN THE MMTOUCHBUTTONS OF THE PAUSE SPLASH
    public void ToggleClientPrediction()
    {
        useClientPrediction = !useClientPrediction;
        if (!useClientPrediction)
        {
            TurnOffClientPredictionGhost();
        }
    }

    public void ToggleClientPredictionGhost()
    {
        showClientPredictionGhost = !showClientPredictionGhost;
        if (showClientPredictionGhost && clientPredictionGhost == null)
        {
            SpawnClientPredictionGhost();
        }
        else
        {
            TurnOffClientPredictionGhost();
        }
    }

    private void SpawnClientPredictionGhost()
    {
        GameObject player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);
        clientPredictionGhost = Instantiate(player, player.transform.position, Quaternion.identity);
        clientPredictionGhost.GetComponent<CustomCharacter>().PlayerID =
            SocketConnectionManager.Instance.playerId.ToString();
        clientPredictionGhost.GetComponent<CustomCharacter>().name =
            $"Client Prediction Ghost {SocketConnectionManager.Instance.playerId}";
        showClientPredictionGhost = true;
    }

    private void TurnOffClientPredictionGhost()
    {
        if (!showClientPredictionGhost && clientPredictionGhost != null)
        {
            clientPredictionGhost
                .GetComponent<CustomCharacter>()
                .GetComponent<Health>()
                .SetHealth(0);
            clientPredictionGhost.SetActive(false);
            Destroy(clientPredictionGhost);
            clientPredictionGhost = null;
        }
    }

    // ENTITY INTERPOLATION UTILITY FUNCTIONS, WE USE THEM IN THE MMTOUCHBUTTONS OF THE PAUSE SPLASH
    public void ToggleInterpolationGhosts()
    {
        showInterpolationGhosts = !showInterpolationGhosts;
        if (showInterpolationGhosts)
        {
            SpawnInterpolationGhosts();
        }
        else
        {
            TurnOffInterpolationGhosts();
        }
    }

    private void SpawnInterpolationGhosts()
    {
        for (int i = 0; i < SocketConnectionManager.Instance.gamePlayers.Count; i++)
        {
            GameObject player = Utils.GetPlayer(SocketConnectionManager.Instance.gamePlayers[i].Id);
            GameObject interpolationGhost;
            interpolationGhost = Instantiate(
                player,
                player.transform.position,
                Quaternion.identity
            );
            interpolationGhost.GetComponent<CustomCharacter>().PlayerID = SocketConnectionManager
                .Instance
                .gamePlayers[i].Id.ToString();
            interpolationGhost.GetComponent<CustomCharacter>().name =
                $"Interpolation Ghost #{SocketConnectionManager.Instance.gamePlayers[i].Id}";

            InterpolationGhosts.Add(interpolationGhost);
        }
    }

    private void TurnOffInterpolationGhosts()
    {
        foreach (GameObject interpolationGhost in InterpolationGhosts)
        {
            interpolationGhost.GetComponent<CustomCharacter>().GetComponent<Health>().SetHealth(0);
            interpolationGhost.SetActive(false);
            Destroy(interpolationGhost);
        }
        InterpolationGhosts = new List<GameObject>();
    }

    public bool InputsAreBeingUsed()
    {
        var inputFromVirtualJoystick = joystickL is not null;

        return (
                inputFromVirtualJoystick && (joystickL.RawValue.x != 0 || joystickL.RawValue.y != 0)
            )
            || (
                (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                || (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
                || (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                || (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            );
    }

    public RelativePosition GetPlayerDirection(Player playerUpdate)
    {
        if (SocketConnectionManager.Instance.playerId != playerUpdate.Id || !useClientPrediction)
        {
            return playerUpdate.Direction;
        }

        var inputFromVirtualJoystick = joystickL is not null;

        var direction = playerUpdate.Direction;
        if (joystickL.RawValue.x != 0 || joystickL.RawValue.y != 0)
        {
            direction = new RelativePosition { X = joystickL.RawValue.x, Y = joystickL.RawValue.y };
        }
        else if (
            Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.S)
        )
        {
            direction = new RelativePosition { X = 0, Y = 0 };
            if (Input.GetKey(KeyCode.W))
                direction.Y = 1;
            if (Input.GetKey(KeyCode.A))
                direction.X = -1;
            if (Input.GetKey(KeyCode.D))
                direction.X = 1;
            if (Input.GetKey(KeyCode.S))
                direction.Y = -1;
        }

        return direction;
    }

    private GameObject FindGhostPlayer(string playerId)
    {
        return InterpolationGhosts.Find(
            g => g.GetComponent<CustomCharacter>().PlayerID == playerId
        );
    }

    private float ManageStateFeedbacks(
        GameObject player,
        Player playerUpdate,
        CustomCharacter character,
        float characterSpeed
    )
    {
        ManageFeedbacks(player, playerUpdate);

        if (playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Scherzo))
        {
            characterSpeed *= 0.5f;
        }

        if (playerUpdate.CharacterName == "Muflus")
        {
            if (playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Raged))
            {
                characterSpeed *= playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Leaping)
                    ? 4f
                    : 1.5f;
            }
            else
            {
                if (playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Leaping))
                {
                    characterSpeed *= 4f;
                }
            }
        }

        // TODO: Temporary out of area feedback. Refactor!
        if (playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.OutOfArea))
        {
            for (int i = 0; i < character.CharacterModel.transform.childCount; i++)
            {
                Renderer renderer = character.CharacterModel.transform
                    .GetChild(i)
                    .GetComponent<Renderer>();
                if (renderer)
                {
                    renderer.material.color = Color.magenta;
                }
            }
        }
        else
        {
            for (int i = 0; i < character.CharacterModel.transform.childCount; i++)
            {
                Renderer renderer = character.CharacterModel.transform
                    .GetChild(i)
                    .GetComponent<Renderer>();
                if (renderer)
                {
                    renderer.material.color = Color.white;
                }
            }
        }

        if (playerUpdate.Id == SocketConnectionManager.Instance.playerId)
        {
            GetComponent<PlayerFeedbacks>()
                .ExecuteH4ckDisarmFeedback(
                    playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Disarmed)
                );
        }

        if (playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Slowed))
        {
            characterSpeed *= 0.5f;
        }
        else if (
            playerUpdate.CharacterName == "H4ck"
            && playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.NeonCrashing)
        )
        {
            characterSpeed *= 4f;
        }

        if (playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Paralyzed))
        {
            characterSpeed = 0f;
        }

        MMHealthBar healthBar = player.GetComponent<MMHealthBar>();

        healthBar.ForegroundColor = playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Poisoned)
            ? Utils.GetHealthBarGradient(MMColors.Green)
            : Utils.GetHealthBarGradient(MMColors.BestRed);

        return characterSpeed;
    }

    private void ManageFeedbacks(GameObject player, Player playerUpdate)
    {
        if (playerUpdate.Effects.Keys.Count == 0 || !PlayerIsAlive(playerUpdate))
        {
            GetComponent<PlayerFeedbacks>().ClearAllFeedbacks(player);
        }

        foreach (ulong key in playerUpdate.Effects.Keys)
        {
            foreach (int effect in Enum.GetValues(typeof(StateEffects)))
            {
                if (playerUpdate.Effects.ContainsKey((ulong)effect))
                {
                    string name = Enum.GetName(typeof(StateEffects), effect);
                    bool isActive = key == (ulong)effect && PlayerIsAlive(playerUpdate);
                    print(name + " " + isActive);
                    GetComponent<PlayerFeedbacks>().SetActiveFeedback(player, name, isActive);
                }
            }
        }
    }

    private bool PlayerIsAlive(Player playerUpdate)
    {
        return playerUpdate.Status == Status.Alive;
    }
}
