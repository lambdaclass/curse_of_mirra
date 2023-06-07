using System.Collections.Generic;

public class EntityUpdates {
    public struct PlayerState
    {
        public enum PlayerAction
        {
            Nothing = 0,
            Attacking = 1,
            AttackingAOE = 2,
        }

        public long x;
        public long y;
        public int player_id;
        public long health;
        public PlayerAction action;
        public long aoe_x;
        public long aoe_y;
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
            lastServerUpdate.x += input.grid_delta_x;
            lastServerUpdate.y += input.grid_delta_y;
        });

        return lastServerUpdate;
    }
}
