namespace Game {
    using Communication.Protobuf;

    public class Item {
        private ulong id { get; }
        private string name { get; }
        private float size { get; }
        private Position position { get; }

        public Item(Communication.Protobuf.Item protobufItem) {
            this.id = protobufItem.Id;
            this.name = protobufItem.Name;
            this.size = protobufItem.Size;
            this.position = new Position(protobufItem.Position);
        }
    }
}
