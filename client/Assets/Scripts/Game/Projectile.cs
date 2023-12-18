namespace Game {
    using Communication.Protobuf;

    public class Projectile {
        private ulong id { get; }
        private string name { get; }
        private ulong damage { get; }
        private float speed { get; }
        private float size { get; }
        private Position position { get; }
        private float direction { get; }

        public Projectile(Communication.Protobuf.Projectile protobufProjectile) {
            this.id = protobufProjectile.Id;
            this.name = protobufProjectile.Name;
            this.damage = protobufProjectile.Damage;
            this.speed = protobufProjectile.Speed;
            this.size = protobufProjectile.Size;
            this.direction = protobufProjectile.Direction;
            this.position = new Position(protobufProjectile.Position);
        }
    }
}
