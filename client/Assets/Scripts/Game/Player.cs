namespace Game {
    using Communication.Protobuf;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public class Player {
        private ulong id { get; }
        private Position position { get; }
        private long health { get; }
        private long speed { get; }
        private float size { get; }
        private float direction { get; }
        private Status status { get; }
        private ulong killCount { get; }
        private ulong deathCount { get; }
        private ulong actionDurationMs { get; }
        private string characterName { get; }
        private List<(Action, string)> actions { get; }
        private List<Cooldown> cooldowns { get; }
        private List<Item> inventory { get; }
        private List<Effect> effects { get; }

        public enum Status {
            Alive,
            Dead
        }

        public enum Action {
            Moving,
            Nothing,
            UsingSkill
        }

        public class Cooldown {
            private string skillKey { get; }
            private ulong remainingMs { get; }

            public Cooldown(string skillKey, ulong remainingMs) {
                this.skillKey = skillKey;
                this.remainingMs = remainingMs;
            }
        }

        public Player(Communication.Protobuf.Player protobufPlayer) {
            this.id = protobufPlayer.Id;
            this.health = protobufPlayer.Health;
            this.speed = protobufPlayer.Speed;
            this.size = protobufPlayer.Size;
            this.direction = protobufPlayer.Direction;
            this.killCount = protobufPlayer.KillCount;
            this.deathCount = protobufPlayer.DeathCount;
            this.actionDurationMs = protobufPlayer.ActionDurationMs;
            this.characterName = protobufPlayer.CharacterName;
            this.position = new Position(protobufPlayer.Position);
            this.status = fromProtobuf(protobufPlayer.Status);
            this.actions = fromProtobuf(protobufPlayer.Actions.ToList());
            this.cooldowns = fromProtobuf(protobufPlayer.Cooldowns.ToList());
            this.inventory = fromProtobuf(protobufPlayer.Inventory.ToList());
            this.effects = fromProtobuf(protobufPlayer.Effects.ToList());
        }

        private static Player.Status fromProtobuf(Communication.Protobuf.PlayerStatus status) {
            switch (status) {
                case Communication.Protobuf.PlayerStatus.Alive: return Player.Status.Alive;
                case Communication.Protobuf.PlayerStatus.Dead: return Player.Status.Dead;
                default: throw new InvalidEnumArgumentException(nameof(status), (int)status, status.GetType());
            }
        }

        private static List<(Player.Action, string)> fromProtobuf(List<Communication.Protobuf.PlayerAction> actions) {
            return actions.ConvertAll<(Player.Action, string)>(action => {
                switch (action.Action) {
                    case Communication.Protobuf.PlayerActionEnum.UsingSkill: return (Player.Action.UsingSkill, action.ActionSkillKey);
                    case Communication.Protobuf.PlayerActionEnum.Nothing: return (Player.Action.Nothing, null);
                    case Communication.Protobuf.PlayerActionEnum.Moving: return (Player.Action.Moving, null);
                    default: throw new InvalidEnumArgumentException(nameof(action.Action), (int)action.Action, action.Action.GetType());
                }
            });
        }

        private static List<Cooldown> fromProtobuf(List<Communication.Protobuf.SkillCooldown> cooldowns) {
            return cooldowns.ConvertAll<Cooldown>(cooldown => new Cooldown(cooldown.SkillKey, cooldown.CooldownMs));
        }

        private static List<Item> fromProtobuf(List<Communication.Protobuf.Item> inventory) {
            return inventory.ConvertAll<Item>(item => new Item(item));
        }

        private static List<Effect> fromProtobuf(List<Communication.Protobuf.EffectInfo> effects) {
            return effects.ConvertAll<Effect>(effect => new Effect(effect));
        }
    }
}
