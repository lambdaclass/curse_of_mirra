namespace Game {
    using Communication.Protobuf;

    public class Zone {
        private float radius { get; }
        private Position center { get; }

        public Zone(Communication.Protobuf.ZoneInfo protobufZone) {
            this.radius = protobufZone.Radius;
            this.center = new Position(protobufZone.Center);
        }
    }
}
