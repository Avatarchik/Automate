using System;

namespace Core.Server.Request {
    public sealed class SpinRequest : SlotRequest {

        public SpinRequest () : base("spin") {
        }
    }
}
