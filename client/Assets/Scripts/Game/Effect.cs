namespace Game {
    using Communication.Protobuf;

    public class Effect {
        private string name { get; }
        private ulong remaining_ms { get; }

        public Effect(Communication.Protobuf.EffectInfo protobufEffect) {
            this.name = protobufEffect.Name;
            this.remaining_ms = protobufEffect.RemainingMs;
        }
    }
}
