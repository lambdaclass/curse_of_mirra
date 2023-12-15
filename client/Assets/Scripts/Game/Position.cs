namespace Game {
    using Communication.Protobuf;

    public class Position {
        private float x { get; }
        private float y { get; }

        public Position(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public Position(Communication.Protobuf.Position protobufPosition) {
            this.x = protobufPosition.X;
            this.y = protobufPosition.X;
        }
    }
}
