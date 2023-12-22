namespace Game {
    using Communication.Protobuf;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public class GameState {
        private Zone zone { get; }
        private long playerTimestamp { get; }
        private long serverTimestamp { get; }
        private List<Player> players { get; }
        private List<Projectile> projectiles { get; }
        public List<Item> items { get; }
        private List<KillEvent> killfeed { get; }

        public GameState(Communication.Protobuf.GameState protobufGameState) {
            this.zone = new Zone(protobufGameState.ZoneInfo);
            this.playerTimestamp = protobufGameState.PlayerTimestamp;
            this.serverTimestamp = protobufGameState.ServerTimestamp;
            this.players = fromProtobuf(protobufGameState.Players.ToList());
            this.projectiles = fromProtobuf(protobufGameState.Projectiles.ToList());
            this.items = fromProtobuf(protobufGameState.Items.ToList());
            this.killfeed = fromProtobuf(protobufGameState.Killfeed.ToList());
        }

        public class KillEvent {
            private KillEntity killedByEntity { get; }
            private ulong killedById { get; }
            private ulong killed { get; }

            public enum KillEntity {
                Player,
                Item,
                Zone
            }

            public KillEvent(Communication.Protobuf.KillEvent protobufKillEvent) {
                this.killedByEntity = fromProtobuf(protobufKillEvent.KilledByEntity);
                this.killedById = protobufKillEvent.KilledById;
                this.killed = protobufKillEvent.Killed;
            }

            private static KillEntity fromProtobuf(Communication.Protobuf.KillEntity entity) {
                switch (entity) {
                    case Communication.Protobuf.KillEntity.Player: return KillEntity.Player;
                    case Communication.Protobuf.KillEntity.Item: return KillEntity.Item;
                    case Communication.Protobuf.KillEntity.Zone: return KillEntity.Zone;
                    default: throw new InvalidEnumArgumentException(nameof(entity), (int)entity, entity.GetType());
                }
            }
        }

        private static List<Player> fromProtobuf(List<Communication.Protobuf.Player> players) {
            return players.ConvertAll<Player>(player => new Player(player));
        }

        private static List<Projectile> fromProtobuf(List<Communication.Protobuf.Projectile> projectiles) {
            return projectiles.ConvertAll<Projectile>(projectile => new Projectile(projectile));
        }

        private static List<Item> fromProtobuf(List<Communication.Protobuf.Item> items) {
            return items.ConvertAll<Item>(item => new Item(item));
        }

        private static List<KillEvent> fromProtobuf(List<Communication.Protobuf.KillEvent> killEvents) {
            return killEvents.ConvertAll<KillEvent>(killEvent => new KillEvent(killEvent));
        }
    }
}
