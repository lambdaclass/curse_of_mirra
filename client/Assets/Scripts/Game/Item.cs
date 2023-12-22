namespace Game {
    using Communication.Protobuf;

    public class Item {
        public ulong id { get; }
        public string name { get; }
        public float size { get; }
        public Position position { get; }

        public Item(Communication.Protobuf.Item protobufItem) {
            this.id = protobufItem.Id;
            this.name = protobufItem.Name;
            this.size = protobufItem.Size;
            this.position = new Position(protobufItem.Position);
        }
    }
}
