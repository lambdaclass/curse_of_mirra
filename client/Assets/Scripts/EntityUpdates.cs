using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityUpdates {
    public struct PlayerState
    {
        public enum PlayerAction
        {
            Nothing = 0,
            Attacking = 1,
            AttackingAOE = 2,
            EXECUTING_SKILL_1 = 3,
            Teleporting = 4,
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

            lastServerUpdate.playerPosition.x = Math.Max(lastServerUpdate.playerPosition.x, -50f);
            lastServerUpdate.playerPosition.x = Math.Min(lastServerUpdate.playerPosition.x, 50f);

            lastServerUpdate.playerPosition.z = Math.Max(lastServerUpdate.playerPosition.z, -50f);
            lastServerUpdate.playerPosition.z = Math.Min(lastServerUpdate.playerPosition.z, 50f);
        });
        pendingPlayerInputs.RemoveAll((input) => input.timestamp + 260 <= serverPlayerUpdate.timestamp);
    }

    public PlayerState simulatePlayerState() {
        var ret = new PlayerState();

        ret.playerPosition = lastServerUpdate.playerPosition;
        ret.playerId = lastServerUpdate.playerId;
        ret.health = lastServerUpdate.health;
        ret.action = lastServerUpdate.action;
        ret.aoeCenterPosition = lastServerUpdate.aoeCenterPosition;
        ret.timestamp = lastServerUpdate.timestamp;

        pendingPlayerInputs.ForEach(input => {
            ret.playerPosition.x += input.grid_delta_x;
            ret.playerPosition.z += input.grid_delta_y;

            ret.playerPosition.x = Math.Max(ret.playerPosition.x, -50f);
            ret.playerPosition.x = Math.Min(ret.playerPosition.x, 50f);

            ret.playerPosition.z = Math.Max(ret.playerPosition.z, -50f);
            ret.playerPosition.z = Math.Min(ret.playerPosition.z, 50f);
        });

        return ret;
    }

    public bool inputsIsEmpty() {
        return pendingPlayerInputs.Count == 0;
    }
}
