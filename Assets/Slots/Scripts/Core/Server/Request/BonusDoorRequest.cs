using System;
using UnitySlot;


namespace Core.Server.Request {
    public sealed class BonusDoorRequest : SlotRequest {

        public BonusDoorRequest () : base("door") {
        }

        public override void FillRequest (HttpRequest request) {
            base.FillRequest(request);
            if (SessionData.Instance.IsFun && GameState.BonusGame.UseGarageSuperKey) {
                request.AddParam (new HttpParam ("key", 1));
                GameState.BonusGame.UseGarageSuperKey = false;
            } else {
            }
        }
    }
}
