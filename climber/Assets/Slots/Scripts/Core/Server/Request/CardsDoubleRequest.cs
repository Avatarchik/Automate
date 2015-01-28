using System;

namespace Core.Server.Request {
    public sealed class CardsDoubleRequest : SlotRequest {

        private int SelectedCard;

        public CardsDoubleRequest (int selectedCard) : base("double") {
            SelectedCard = selectedCard;
        }

        public override void FillRequest (HttpRequest request) {
            base.FillRequest(request);
            request.AddParam (new HttpParam ("card", SelectedCard));
        }
    }
}
