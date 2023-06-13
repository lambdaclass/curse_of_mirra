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
        public float grid_delta_x;
        public float grid_delta_y;
        public long timestamp;
    }

    public List<PlayerInput> pendingPlayerInputs = new List<PlayerInput>();

    public PlayerState lastServerUpdate = new PlayerState();

    public void putPlayerInput(PlayerInput PlayerInput)
    {
        pendingPlayerInputs.Add(PlayerInput);
    }

    public void putServerUpdate(PlayerState serverPlayerUpdate)
    {
        var acknowledgedInputs = pendingPlayerInputs.FindAll((input) => input.timestamp + 260 <= serverPlayerUpdate.timestamp);
        acknowledgedInputs.ForEach(input => {
            lastServerUpdate.playerPosition.x += input.grid_delta_x;
            lastServerUpdate.playerPosition.z += input.grid_delta_y;
        });
        pendingPlayerInputs.RemoveAll((input) => input.timestamp + 260 <= serverPlayerUpdate.timestamp);
    }

    public PlayerState simulatePlayerState() {
        var ret = new PlayerState();

        ret.playerPosition = lastServerUpdate.playerPosition;
        // ret.playerId = lastServerUpdate.playerId;
        ret.playerId = 1;
        // ret.health = lastServerUpdate.health;
        ret.health = 100;
        ret.action = lastServerUpdate.action;
        ret.aoeCenterPosition = lastServerUpdate.aoeCenterPosition;
        ret.timestamp = lastServerUpdate.timestamp;

        pendingPlayerInputs.ForEach(input => {
            ret.playerPosition.x += input.grid_delta_x;
            ret.playerPosition.z += input.grid_delta_y;
        });

        return ret;
    }

    public bool inputsIsEmpty() {
        return pendingPlayerInputs.Count == 0;
    }
}
