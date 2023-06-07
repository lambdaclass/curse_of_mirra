using System.Collections.Generic;
using UnityEngine;

public class EntityUpdates {
    public struct PlayerState
    {
        public enum PlayerAction
        {
            Nothing = 0,
            Attacking = 1,
            AttackingAOE = 2,
        }

        public Vector3 playerPosition;
        public int playerId;
        public long health;
        public PlayerAction action;
        public Vector3 aoeCenterPosition;
        public long timestamp;
    }

    public struct PlayerInput
    {
        public long grid_delta_x;
        public long grid_delta_y;
        public long timestamp;
    }

    private List<PlayerInput> pendingPlayerInputs = new List<PlayerInput>();

    private PlayerState lastServerUpdate;

    public void putPlayerInput(PlayerInput PlayerInput)
    {
        pendingPlayerInputs.Add(PlayerInput);
    }

    public void putServerUpdate(PlayerState serverPlayerUpdate)
    {
        pendingPlayerInputs.RemoveAll((input) => input.timestamp < serverPlayerUpdate.timestamp);
        lastServerUpdate = serverPlayerUpdate;
    }

    public PlayerState simulatePlayerState() {
        pendingPlayerInputs.ForEach(input => {
            lastServerUpdate.playerPosition.x += input.grid_delta_x;
            lastServerUpdate.playerPosition.z += input.grid_delta_y;
        });

        return lastServerUpdate;
    }
}
