using System;
using UnitySlot;


namespace Core.Server.Request {
    public sealed class FreeGameRequest : SlotRequest {

        public FreeGameRequest () : base("freegame") {
        }

    }
}
