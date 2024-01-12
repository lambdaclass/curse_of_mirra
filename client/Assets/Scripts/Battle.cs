using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public HashSet<SkillInfo> skillInfoSet;

    [SerializeField]
    LeftMMTouchJoystick joystickL;

    [SerializeField]
    CustomInputManager InputManager;

    [SerializeField]
    CustomGUIManager CustomGUIManager;

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
    private bool playerMaterialColorChanged;
    private bool sendMovementStarted = false;

    [SerializeField]
    private CustomLevelManager levelManager;

//     // We do this to only have the state effects in the enum instead of all the effects
//     private enum StateEffects
//     {
//         Slowed = PlayerEffect.Slowed,
//         Paralyzed = PlayerEffect.Paralyzed,
//         Poisoned = PlayerEffect.Poisoned,
//         OutOfArea = PlayerEffect.OutOfArea
//     }

//     void Start()
//     {
//         InitBlockingStates();
//         SetupInitialState();
//         StartCoroutine(InitializeProjectiles());
//         loot = GetComponent<Loot>();
//         playerMaterialColorChanged = false;
//     }

//     private void InitBlockingStates()
//     {
//         BlockingMovementStates = new CharacterStates.MovementStates[1];
//         BlockingMovementStates[0] = CharacterStates.MovementStates.Attacking;
//     }

//     private void SetupInitialState()
//     {
//         useClientPrediction = true;
//         useInterpolation = true;
//         accumulatedTime = 0;
//         showClientPredictionGhost = false;
//         showInterpolationGhosts = false;
//     }

//     private IEnumerator InitializeProjectiles()
//     {
//         yield return new WaitUntil(() => GameServerConnectionManager.Instance.players.Count > 0);
//         CreateProjectilesPoolers();
//     }

//     void CreateProjectilesPoolers()
//     {
//         skillInfoSet = new HashSet<SkillInfo>();
//         foreach (GameObject player in GameServerConnectionManager.Instance.players)
//         {
//             skillInfoSet.UnionWith(
//                 player
//                     .GetComponents<Skill>()
//                     .Select(skill => skill.GetSkillInfo())
//                     .Where(skill => skill.projectilePrefab != null)
//             );
//         }
//         GetComponent<ProjectileHandler>().CreateProjectilesPoolers(skillInfoSet);
//     }

    void Update()
    {
        if (!sendMovementStarted)
        {
            sendMovementStarted = true;
            InvokeRepeating("SendPlayerMovement", 0, 0.1f);
        }

        MoveEntities();
//         if (
//             GameServerConnectionManager.Instance.gamePlayers != null
//             && GameServerConnectionManager.Instance.players.Count > 0
//             && GameServerConnectionManager.Instance.gamePlayers.Count > 0
//         )
//         {
//             SetAccumulatedTime();
//             UpdateBattleState();
//         }

//         if (ServerConnection.Instance.gameStarted && !sendMovementStarted)
//         {
//             sendMovementStarted = true;
//             float clientActionRate = GameServerConnectionManager.Instance.serverTickRate_ms / 1000f;
//             InvokeRepeating("SendPlayerMovement", 0, clientActionRate);
//         }
    }

    private void MoveEntities()
    {
        // TODO: For now we hardcode to only move entity 1 which is the single spawned player entity
        var entity = levelManager.PlayerPrefabs[0];
        entity.transform.position = new Vector3(5f, 0f, 5f);
    }

//     void UpdateBattleState()
//     {
//         UpdatePlayerActions();
//         UpdateProjectileActions();
//         loot.UpdateLoots();
//     }

//     private void SetAccumulatedTime()
//     {
//         if (firstTimestamp == 0)
//         {
//             firstTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
//         }
//         var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
//         accumulatedTime = (currentTimestamp - firstTimestamp);
//     }

//     public bool PlayerMovementAuthorized(Character character)
//     {
//         if ((BlockingMovementStates != null) && (BlockingMovementStates.Length > 0))
//         {
//             for (int i = 0; i < BlockingMovementStates.Length; i++)
//             {
//                 if (BlockingMovementStates[i] == (character.MovementState.CurrentState))
//                 {
//                     return false;
//                 }
//             }
//         }

//         if ((BlockingConditionStates != null) && (BlockingConditionStates.Length > 0))
//         {
//             for (int i = 0; i < BlockingConditionStates.Length; i++)
//             {
//                 if (BlockingConditionStates[i] == (character.ConditionState.CurrentState))
//                 {
//                     return false;
//                 }
//             }
//         }

//         return true;
//     }

    public void SendPlayerMovement()
    {
        GetComponent<PlayerControls>().SendAction();
//         GameObject player = Utils.GetPlayer(GameServerConnectionManager.Instance.playerId);
//         OldGameEvent lastEvent = GameServerConnectionManager.Instance.eventsBuffer.lastEvent();
//         OldPlayer playerUpdate = lastEvent
//             .Players
//             .ToList()
//             .Find(p => p.Id == GameServerConnectionManager.Instance.playerId);

//         if (player)
//         {
//             CustomCharacter character = player.GetComponent<CustomCharacter>();
//             if (PlayerMovementAuthorized(character))
//             {
//                 var inputFromVirtualJoystick = joystickL is not null;
//                 if (
//                     inputFromVirtualJoystick
//                     && (joystickL.RawValue.x != 0 || joystickL.RawValue.y != 0)
//                 )
//                 {
//                     GetComponent<PlayerControls>()
//                         .SendJoystickValues(joystickL.RawValue.x, joystickL.RawValue.y);
//                 }
//                 else
//                 {
//                     GetComponent<PlayerControls>().SendAction();
//                 }
//             }
//         }
    }

//     void UpdatePlayerActions()
//     {
//         long currentTime;
//         long pastTime;
//         GameObject interpolationGhost = null;
//         EventsBuffer buffer = GameServerConnectionManager.Instance.eventsBuffer;
//         OldGameEvent gameEvent;

//         currentTime = buffer.firstTimestamp + accumulatedTime;
//         pastTime = currentTime - buffer.deltaInterpolationTime;

//         if (buffer.firstTimestamp == 0)
//         {
//             buffer.firstTimestamp = buffer.lastEvent().ServerTimestamp;
//         }

//         for (int i = 0; i < GameServerConnectionManager.Instance.gamePlayers.Count; i++)
//         {
//             if (showInterpolationGhosts)
//             {
//                 interpolationGhost = FindGhostPlayer(
//                     GameServerConnectionManager.Instance.gamePlayers[i].Id.ToString()
//                 );
//             }

//             if (
//                 useInterpolation
//                 && (
//                     GameServerConnectionManager.Instance.playerId
//                         != GameServerConnectionManager.Instance.gamePlayers[i].Id
//                     || !useClientPrediction
//                 )
//             )
//             {
//                 gameEvent = buffer.getNextEventToRender(pastTime);
//             }
//             else
//             {
//                 gameEvent = buffer.lastEvent();
//             }

//             // There are a few frames during which this is outdated and produces an error
//             if (GameServerConnectionManager.Instance.gamePlayers.Count == gameEvent.Players.Count)
//             {
//                 // This call to `new` here is extremely important for client prediction. If we don't make a copy,
//                 // prediction will modify the player in place, which is not what we want.
//                 OldPlayer serverPlayerUpdate = new OldPlayer(gameEvent.Players[i]);
//                 if (
//                     serverPlayerUpdate.Id == (ulong)GameServerConnectionManager.Instance.playerId
//                     && useClientPrediction
//                 )
//                 {
//                     // Move the ghost BEFORE client prediction kicks in, so it only moves up until
//                     // the last server update.
//                     if (clientPredictionGhost != null)
//                     {
//                         UpdatePlayer(clientPredictionGhost, serverPlayerUpdate, pastTime);
//                     }
//                     GameServerConnectionManager
//                         .Instance
//                         .clientPrediction
//                         .simulatePlayerState(serverPlayerUpdate, gameEvent.PlayerTimestamp);
//                 }

//                 if (interpolationGhost != null)
//                 {
//                     UpdatePlayer(interpolationGhost, buffer.lastEvent().Players[i], pastTime);
//                 }

//                 GameObject currentPlayer = Utils.GetPlayer(serverPlayerUpdate.Id);

//                 // TODO: try to optimize GetComponent calls
//                 CustomCharacter playerCharacter = currentPlayer.GetComponent<CustomCharacter>();

//                 if (currentPlayer.activeSelf)
//                 {
//                     if (serverPlayerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Paralyzed))
//                     {
//                         UpdatePlayer(currentPlayer, buffer.lastEvent().Players[i], pastTime);
//                     }
//                     else
//                     {
//                         UpdatePlayer(currentPlayer, serverPlayerUpdate, pastTime);
//                     }

//                     if (
//                         !buffer.timestampAlreadySeen(
//                             GameServerConnectionManager.Instance.gamePlayers[i].Id,
//                             gameEvent.ServerTimestamp
//                         )
//                     )
//                     {
//                         foreach (OldActionTracker actionTracker in serverPlayerUpdate.Action)
//                         {
//                             if (
//                                 PlayerMovementAuthorized(playerCharacter)
//                                 && !playerCharacter.currentActions.Contains(actionTracker)
//                             )
//                             {
//                                 playerCharacter.currentActions.Add(actionTracker);
//                                 ExecuteSkillFeedback(
//                                     currentPlayer,
//                                     actionTracker.PlayerAction,
//                                     serverPlayerUpdate.Direction,
//                                     actionTracker.Duration
//                                 );
//                             }
//                         }

//                         List<OldActionTracker> actionsToDelete = playerCharacter
//                             .currentActions
//                             .Except(serverPlayerUpdate.Action)
//                             .ToList();

//                         foreach (OldActionTracker action in actionsToDelete)
//                         {
//                             playerCharacter.currentActions.Remove(action);
//                         }

//                         buffer.setLastTimestampSeen(
//                             GameServerConnectionManager.Instance.gamePlayers[i].Id,
//                             gameEvent.ServerTimestamp
//                         );
//                     }
//                 }

//                 if (serverPlayerUpdate.Health <= 0)
//                 {
//                     SetPlayerDead(playerCharacter);
//                 }

//                 Transform hitbox = playerCharacter.characterBase.Hitbox.transform;

//                 float hitboxSize = serverPlayerUpdate.BodySize / 50f;
//                 hitbox.localScale = new Vector3(hitboxSize, hitbox.localScale.y, hitboxSize);
//             }
//         }
//     }

//     private void ExecuteSkillFeedback(
//         GameObject currentPlayer,
//         OldPlayerAction playerAction,
//         RelativePosition direction,
//         ulong skillDuration
//     )
//     {
//         // TODO: Refactor
//         switch (playerAction)
//         {
//             case OldPlayerAction.Attacking:
//                 currentPlayer.GetComponent<SkillBasic>().ExecuteFeedbacks(skillDuration, false);
//                 rotatePlayer(currentPlayer, direction);
//                 break;
//             case OldPlayerAction.StartingSkill1:
//                 currentPlayer.GetComponent<Skill1>().ExecuteFeedbacks(skillDuration, true);
//                 rotatePlayer(currentPlayer, direction);
//                 break;
//             case OldPlayerAction.ExecutingSkill1:
//                 currentPlayer.GetComponent<Skill1>().ExecuteFeedbacks(skillDuration, false);
//                 rotatePlayer(currentPlayer, direction);
//                 break;
//             case OldPlayerAction.StartingSkill2:
//                 currentPlayer.GetComponent<Skill2>().ExecuteFeedbacks(skillDuration, true);
//                 rotatePlayer(currentPlayer, direction);
//                 break;
//             case OldPlayerAction.ExecutingSkill2:
//                 currentPlayer.GetComponent<Skill2>().ExecuteFeedbacks(skillDuration, false);
//                 rotatePlayer(currentPlayer, direction);
//                 break;
//             case OldPlayerAction.StartingSkill3:
//                 currentPlayer.GetComponent<Skill3>().ExecuteFeedbacks(skillDuration, true);
//                 rotatePlayer(currentPlayer, direction);
//                 break;
//             case OldPlayerAction.ExecutingSkill3:
//                 currentPlayer.GetComponent<Skill3>().ExecuteFeedbacks(skillDuration, false);
//                 rotatePlayer(currentPlayer, direction);
//                 break;
//         }
//     }

//     void UpdateProjectileActions()
//     {
//         Dictionary<int, GameObject> projectiles = GameServerConnectionManager.Instance.projectiles;
//         List<Communication.Protobuf.OldProjectile> gameProjectiles = GameServerConnectionManager
//             .Instance
//             .gameProjectiles;
//         ClearProjectiles(projectiles, gameProjectiles);
//         ProcessProjectilesCollision(projectiles, gameProjectiles);
//         UpdateProjectiles(projectiles, gameProjectiles);
//     }

//     void UpdateProjectiles(
//         Dictionary<int, GameObject> projectiles,
//         List<Communication.Protobuf.OldProjectile> gameProjectiles
//     )
//     {
//         GameObject projectile;
//         for (int i = 0; i < gameProjectiles.Count; i++)
//         {
//             Vector3 backToFrontPosition = Utils.transformBackendOldPositionToFrontendPosition(
//                 gameProjectiles[i].Position
//             );
//             if (projectiles.TryGetValue((int)gameProjectiles[i].Id, out projectile))
//             {
//                 projectile
//                     .GetComponent<SkillProjectile>()
//                     .UpdatePosition(
//                         new Vector3(backToFrontPosition[0], 3f, backToFrontPosition[2])
//                     );
//             }
//             else if (gameProjectiles[i].Status == ProjectileStatus.Active)
//             {
//                 float angle = Vector3.SignedAngle(
//                     new Vector3(1f, 0, 0),
//                     new Vector3(
//                         (long)(gameProjectiles[i].Direction.Y * 100),
//                         0f,
//                         -(long)(gameProjectiles[i].Direction.X * 100)
//                     ),
//                     Vector3.up
//                 );
//                 GameObject projectileFromSkill = skillInfoSet
//                     .Where(el => el.name == gameProjectiles[i].SkillName)
//                     .First()
//                     .projectilePrefab;
//                 GameObject skillProjectile = GetComponent<ProjectileHandler>()
//                     .InstanceProjectile(
//                         projectileFromSkill,
//                         angle,
//                         new Vector3(backToFrontPosition[0], 3f, backToFrontPosition[2])
//                     );

//                 projectiles.Add((int)gameProjectiles[i].Id, skillProjectile);
//             }
//         }
//     }

//     void ClearProjectiles(
//         Dictionary<int, GameObject> projectiles,
//         List<Communication.Protobuf.OldProjectile> gameProjectiles
//     )
//     {
//         foreach (int projectileId in projectiles.Keys.ToList())
//         {
//             if (!gameProjectiles.Exists(x => (int)x.Id == projectileId))
//             {
//                 projectiles[projectileId].GetComponent<SkillProjectile>().Remove();
//                 projectiles.Remove(projectileId);
//             }
//         }
//     }

//     void ProcessProjectilesCollision(
//         Dictionary<int, GameObject> projectiles,
//         List<Communication.Protobuf.OldProjectile> gameProjectiles
//     )
//     {
//         foreach (var pr in projectiles.ToList())
//         {
//             Communication.Protobuf.OldProjectile gameProjectile = gameProjectiles.Find(
//                 x => (int)x.Id == pr.Key
//             );
//             if (gameProjectile.Status == ProjectileStatus.Exploded)
//             {
//                 pr.Value.GetComponent<SkillProjectile>().ProcessCollision();
//                 projectiles.Remove(pr.Key);
//             }
//         }
//     }

//     private void rotatePlayer(GameObject player, RelativePosition direction)
//     {
//         CharacterOrientation3D characterOrientation = player.GetComponent<CharacterOrientation3D>();
//         characterOrientation.ForcedRotation = true;
//         Vector3 movementDirection = new Vector3(direction.X, 0f, direction.Y);
//         movementDirection.Normalize();
//         characterOrientation.ForcedRotationDirection = movementDirection;
//     }

//     private void UpdatePlayer(GameObject player, OldPlayer playerUpdate, long pastTime)
//     {
//         /*
//         Player has a speed of 3 tiles per tick. A tile in unity is 0.3f a distance of 0.3f.
//         There are 50 ticks per second. A player's velocity is 50 * 0.3f

//         In general, if a player's velocity is n tiles per tick, their unity velocity
//         is 50 * (n / 10f)

//         The above is the player's velocity's magnitude. Their velocity's direction
//         is the direction of deltaX, which we can calculate (assumming we haven't lost socket
//         frames, but that's fine).
//         */
//         CustomCharacter character = player.GetComponent<CustomCharacter>();
//         var characterSpeed = playerUpdate.Speed / 100f;

//         Animator modelAnimator = player
//             .GetComponent<CustomCharacter>()
//             .CharacterModel
//             .GetComponent<Animator>();

//         ManageStateFeedbacks(player, playerUpdate, character);

//         if (!GameServerConnectionManager.Instance.GameHasEnded())
//         {
//             HandleMovement(player, playerUpdate, pastTime, characterSpeed);
//         }
//         else
//         {
//             modelAnimator.SetBool("Walking", false);
//         }

//         HandlePlayerHealth(player, playerUpdate);

//         if (playerUpdate.Id == GameServerConnectionManager.Instance.playerId)
//         {
//             /*
//                 - We divided the milliseconds time in two parts because
//                 - rustler can't handle u128, so instead of developing those functions
//                 - we decided to use 2 u64 fields to represent the time in milliseconds

//                 - If you need to use complete time in milliseconds, you should use both
//                 - If you need to use remaining time in milliseconds, you can use only low field
//                 - because high field will be 0
//             */
//             InputManager.CheckSkillCooldown(
//                 UIControls.SkillBasic,
//                 (float)playerUpdate.BasicSkillCooldownLeft.Low / 1000f,
//                 player.GetComponent<SkillBasic>().GetSkillInfo().showCooldown
//             );
//             InputManager.CheckSkillCooldown(
//                 UIControls.Skill1,
//                 (float)playerUpdate.Skill1CooldownLeft.Low / 1000f,
//                 player.GetComponent<Skill1>().GetSkillInfo().showCooldown
//             );
//         }
//     }

//     private void HandlePlayerHealth(GameObject player, OldPlayer playerUpdate)
//     {
//         Health healthComponent = player.GetComponent<Health>();
//         CharacterFeedbacks characterFeedbacks = player.GetComponent<CharacterFeedbacks>();

//         characterFeedbacks.DamageFeedback(
//             healthComponent.CurrentHealth,
//             playerUpdate.Health,
//             playerUpdate.Id
//         );

//         if (playerUpdate.Health != healthComponent.CurrentHealth)
//         {
//             healthComponent.SetHealth(playerUpdate.Health);
//         }
//     }

//     private void HandleMovement(
//         GameObject player,
//         OldPlayer playerUpdate,
//         long pastTime,
//         float characterSpeed
//     )
//     {
//         // This is tickRate * characterSpeed. Once we decouple tickRate from speed on the backend
//         // it'll be changed.
//         float tickRate = 1000f / GameServerConnectionManager.Instance.serverTickRate_ms;
//         float velocity = tickRate * characterSpeed;

//         var frontendPosition = Utils.transformBackendOldPositionToFrontendPosition(
//             playerUpdate.Position
//         );

//         float xChange = frontendPosition.x - player.transform.position.x;
//         float yChange = frontendPosition.z - player.transform.position.z;

//         Animator modelAnimator = player
//             .GetComponent<CustomCharacter>()
//             .CharacterModel
//             .GetComponent<Animator>();

//         bool walking = false;

//         if (playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Paralyzed))
//         {
//             if (player.transform.position != frontendPosition)
//             {
//                 player.transform.position = new Vector3(
//                     frontendPosition.x,
//                     player.transform.position.y,
//                     frontendPosition.z
//                 );
//             }
//             modelAnimator.SetBool("Walking", walking);
//             return;
//         }

//         if (useClientPrediction)
//         {
//             walking =
//                 playerUpdate.Id == GameServerConnectionManager.Instance.playerId
//                     ? InputsAreBeingUsed()
//                     : GameServerConnectionManager
//                         .Instance
//                         .eventsBuffer
//                         .playerIsMoving(playerUpdate.Id, (long)pastTime);
//         }
//         else
//         {
//             if (playerUpdate.Id == GameServerConnectionManager.Instance.playerId)
//             {
//                 walking = GameServerConnectionManager
//                     .Instance
//                     .eventsBuffer
//                     .playerIsMoving(playerUpdate.Id, (long)pastTime);
//             }
//         }

//         Vector2 movementChange = new Vector2(xChange, yChange);

//         if (movementChange.magnitude > 0f)
//         {
//             Vector3 movementDirection = new Vector3(xChange, 0f, yChange);
//             movementDirection.Normalize();

//             // FIXME: Remove harcoded validation once is fixed on the backend.
//             if (
//                 playerUpdate.CharacterName == "Muflus"
//                 && playerUpdate
//                     .Action
//                     .Any(
//                         actionTracker =>
//                             actionTracker.PlayerAction == OldPlayerAction.ExecutingSkill3
//                     )
//             )
//             {
//                 player.transform.position = frontendPosition;
//             }
//             else
//             {
//                 // The idea here is, when moving, we never want to go past the position the backend is telling us we are in.
//                 // Let's say the movementChange vector is (1, 0), i.e., we are moving horizontally to the right.
//                 // Let's also say frontendPosition is (2, y, 1)
//                 // If newPosition is (2.1, y, 1), we want it to just be (2, y, 1).
//                 // In this case, all we are doing is saying that the `x` coordinate should be min(2, newPosition.x)
//                 // If the movement were left, we would take max(2, newPosition.x)
//                 // Let's now say that the movement is in the (1, 1) normalized direction, so diagonally up and right.
//                 // If frontendPosition is (2, y, 1), I can't go past it in the (1, 1) direction. What we need to do here is
//                 // simply take the `x` coordinate to be min(2, newPosition.x) and the `z` coordinate to be min(1, newPosition.z)

//                 // In general, if the movementDirection vector is (x, y, z) normalized, then if its `x` coordinate is positive, we should
//                 // take newPosition.x = min(frontendPosition.x, newPosition.x)
//                 // If, on the other hand, its `x` coordinate is negative, we take newPosition.x = max(frontendPosition.x, newPosition.x)
//                 // The exact same thing applies to `z`
//                 Vector3 newPosition =
//                     player.transform.position + movementDirection * velocity * Time.deltaTime;

//                 if (movementDirection.x > 0)
//                 {
//                     newPosition.x = Math.Min(frontendPosition.x, newPosition.x);
//                 }
//                 else
//                 {
//                     newPosition.x = Math.Max(frontendPosition.x, newPosition.x);
//                 }

//                 if (movementDirection.z > 0)
//                 {
//                     newPosition.z = Math.Min(frontendPosition.z, newPosition.z);
//                 }
//                 else
//                 {
//                     newPosition.z = Math.Max(frontendPosition.z, newPosition.z);
//                 }
//                 player.transform.position = newPosition;

//                 // FIXME: This is a temporary solution to solve unwanted player rotation until we handle movement blocking on backend
//                 // if the player is in attacking state, movement rotation from movement should be ignored
//                 RelativePosition direction = GetPlayerDirection(playerUpdate);

//                 if (PlayerMovementAuthorized(player.GetComponent<CustomCharacter>()))
//                 {
//                     rotatePlayer(player, direction);
//                 }
//             }
//             walking = true;
//         }

//         RotateCharacterOrientation(player);

//         modelAnimator.SetBool("Walking", walking);
//     }

//     private void RotateCharacterOrientation(GameObject player)
//     {
//         player.GetComponentInChildren<CharacterBase>().OrientationIndicator.transform.rotation =
//             player.GetComponent<CustomCharacter>().CharacterModel.transform.rotation;
//     }

//     public void SetPlayerDead(CustomCharacter playerCharacter)
//     {
//         CharacterFeedbacks playerFeedback = playerCharacter.GetComponent<CharacterFeedbacks>();
//         playerFeedback.PlayDeathFeedback();
//         playerFeedback.ClearAllFeedbacks(playerCharacter.gameObject);
//         playerCharacter.CharacterModel.SetActive(false);
//         playerCharacter.ConditionState.ChangeState(CharacterStates.CharacterConditions.Dead);
//         playerCharacter.characterBase.Hitbox.SetActive(false);
//         levelManager.DestroySkillsClone(playerCharacter);
//         playerCharacter
//             .GetComponentInChildren<CharacterBase>()
//             .OrientationIndicator
//             .SetActive(false);
//         if (GameServerConnectionManager.Instance.playerId == ulong.Parse(playerCharacter.PlayerID))
//         {
//             CustomGUIManager.DisplayZoneDamageFeedback(false);
//         }
//     }

//     // CLIENT PREDICTION UTILITY FUNCTIONS , WE USE THEM IN THE MMTOUCHBUTTONS OF THE PAUSE SPLASH
//     public void ToggleClientPrediction()
//     {
//         useClientPrediction = !useClientPrediction;
//         if (!useClientPrediction)
//         {
//             TurnOffClientPredictionGhost();
//         }
//     }

//     public void ToggleClientPredictionGhost()
//     {
//         showClientPredictionGhost = !showClientPredictionGhost;
//         if (showClientPredictionGhost && clientPredictionGhost == null)
//         {
//             SpawnClientPredictionGhost();
//         }
//         else
//         {
//             TurnOffClientPredictionGhost();
//         }
//     }

//     private void SpawnClientPredictionGhost()
//     {
//         GameObject player = Utils.GetPlayer(GameServerConnectionManager.Instance.playerId);
//         clientPredictionGhost = Instantiate(player, player.transform.position, Quaternion.identity);
//         clientPredictionGhost.GetComponent<CustomCharacter>().PlayerID = GameServerConnectionManager
//             .Instance
//             .playerId
//             .ToString();
//         clientPredictionGhost.GetComponent<CustomCharacter>().name =
//             $"Client Prediction Ghost {GameServerConnectionManager.Instance.playerId}";
//         showClientPredictionGhost = true;
//     }

//     private void TurnOffClientPredictionGhost()
//     {
//         if (!showClientPredictionGhost && clientPredictionGhost != null)
//         {
//             clientPredictionGhost
//                 .GetComponent<CustomCharacter>()
//                 .GetComponent<Health>()
//                 .SetHealth(0);
//             clientPredictionGhost.SetActive(false);
//             Destroy(clientPredictionGhost);
//             clientPredictionGhost = null;
//         }
//     }

//     // ENTITY INTERPOLATION UTILITY FUNCTIONS, WE USE THEM IN THE MMTOUCHBUTTONS OF THE PAUSE SPLASH
//     public void ToggleInterpolationGhosts()
//     {
//         showInterpolationGhosts = !showInterpolationGhosts;
//         if (showInterpolationGhosts)
//         {
//             SpawnInterpolationGhosts();
//         }
//         else
//         {
//             TurnOffInterpolationGhosts();
//         }
//     }

//     private void SpawnInterpolationGhosts()
//     {
//         for (int i = 0; i < GameServerConnectionManager.Instance.gamePlayers.Count; i++)
//         {
//             GameObject player = Utils.GetPlayer(
//                 GameServerConnectionManager.Instance.gamePlayers[i].Id
//             );
//             GameObject interpolationGhost;
//             interpolationGhost = Instantiate(
//                 player,
//                 player.transform.position,
//                 Quaternion.identity
//             );
//             interpolationGhost.GetComponent<CustomCharacter>().PlayerID =
//                 GameServerConnectionManager.Instance.gamePlayers[i].Id.ToString();
//             interpolationGhost.GetComponent<CustomCharacter>().name =
//                 $"Interpolation Ghost #{GameServerConnectionManager.Instance.gamePlayers[i].Id}";

//             InterpolationGhosts.Add(interpolationGhost);
//         }
//     }

//     private void TurnOffInterpolationGhosts()
//     {
//         foreach (GameObject interpolationGhost in InterpolationGhosts)
//         {
//             interpolationGhost.GetComponent<CustomCharacter>().GetComponent<Health>().SetHealth(0);
//             interpolationGhost.SetActive(false);
//             Destroy(interpolationGhost);
//         }
//         InterpolationGhosts = new List<GameObject>();
//     }

//     public bool InputsAreBeingUsed()
//     {
//         var inputFromVirtualJoystick = joystickL is not null;

//         return (
//                 inputFromVirtualJoystick && (joystickL.RawValue.x != 0 || joystickL.RawValue.y != 0)
//             )
//             || (
//                 (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
//                 || (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
//                 || (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
//                 || (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
//             );
//     }

//     public RelativePosition GetPlayerDirection(OldPlayer playerUpdate)
//     {
//         if (
//             GameServerConnectionManager.Instance.playerId != playerUpdate.Id
//             || !useClientPrediction
//         )
//         {
//             return playerUpdate.Direction;
//         }

//         var inputFromVirtualJoystick = joystickL is not null;

//         var direction = playerUpdate.Direction;
//         if (joystickL.RawValue.x != 0 || joystickL.RawValue.y != 0)
//         {
//             direction = new RelativePosition { X = joystickL.RawValue.x, Y = joystickL.RawValue.y };
//         }
//         else if (
//             Input.GetKey(KeyCode.W)
//             || Input.GetKey(KeyCode.A)
//             || Input.GetKey(KeyCode.D)
//             || Input.GetKey(KeyCode.S)
//         )
//         {
//             direction = new RelativePosition { X = 0, Y = 0 };
//             if (Input.GetKey(KeyCode.W))
//                 direction.Y = 1;
//             if (Input.GetKey(KeyCode.A))
//                 direction.X = -1;
//             if (Input.GetKey(KeyCode.D))
//                 direction.X = 1;
//             if (Input.GetKey(KeyCode.S))
//                 direction.Y = -1;
//         }

//         return direction;
//     }

//     private GameObject FindGhostPlayer(string playerId)
//     {
//         return InterpolationGhosts.Find(
//             g => g.GetComponent<CustomCharacter>().PlayerID == playerId
//         );
//     }

//     private void ManageStateFeedbacks(
//         GameObject player,
//         OldPlayer playerUpdate,
//         CustomCharacter character
//     )
//     {
//         CharacterFeedbackManager feedbackManager = character
//             .characterBase
//             .GetComponent<CharacterFeedbackManager>();

//         ManageFeedbacks(player, playerUpdate);
//         feedbackManager.ToggleHealthBar(player, playerUpdate);
//     }

//     private void ManageFeedbacks(GameObject player, OldPlayer playerUpdate)
//     {
//         foreach (int effect in Enum.GetValues(typeof(StateEffects)))
//         {
//             string name = Enum.GetName(typeof(StateEffects), effect);
//             bool hasEffect = playerUpdate.Effects.ContainsKey((ulong)effect);
//             CustomGUIManager.stateManagerUI.ToggleState(name, playerUpdate.Id, hasEffect);
//             player.GetComponent<CharacterFeedbacks>().SetActiveFeedback(player, name, hasEffect);
//         }
//     }
}
