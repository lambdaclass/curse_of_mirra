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
        private List<ActionTracker> actions { get; }
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

        public class ActionTracker {
            private Action action { get; }
            private ulong duration { get; }
            private string skillKey { get; }

            public ActionTracker(Action action, ulong duration, string skillKey) {
                this.action = action;
                this.duration = duration;
                this.skillKey = skillKey;
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
            this.characterName = protobufPlayer.CharacterName;
            this.position = new Position(protobufPlayer.Position);
            this.status = fromProtobuf(protobufPlayer.Status);
            this.actions = fromProtobuf(protobufPlayer.Action.ToList());
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

        private static List<ActionTracker> fromProtobuf(List<Communication.Protobuf.ActionTracker> actions) {
            return actions.ConvertAll<Player.ActionTracker>(actionTracker => {
                Action newAction;
                switch (actionTracker.PlayerAction.Action) {
                    case Communication.Protobuf.PlayerActionEnum.UsingSkill: 
                        newAction = Player.Action.UsingSkill;
                        break;
                    case Communication.Protobuf.PlayerActionEnum.Nothing: 
                        newAction = Player.Action.Nothing;
                        break;
                    case Communication.Protobuf.PlayerActionEnum.Moving: 
                        newAction = Player.Action.Moving;
                        break;
                    default: throw new InvalidEnumArgumentException(nameof(actionTracker.PlayerAction.Action), (int)actionTracker.PlayerAction.Action, actionTracker.PlayerAction.Action.GetType());
                }
                return new Player.ActionTracker(newAction, actionTracker.Duration, actionTracker.PlayerAction.ActionSkillKey);
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
