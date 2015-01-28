using System;
using UnitySlot;


namespace Core.Server.Request {
    public abstract class SlotRequest : BaseRequest {

        public string Game;
        protected string Action;
        protected double Bet;
        protected double BetLine;
        protected int Lines;
        protected double Min;

        public SlotRequest (string action) {
            this.Action = action;
            this.Game = GameState.CurrentGame.Game;
            this.BetLine = GameState.CurrentGame.Bet;
            this.Bet = GameState.CurrentGame.Bet * GameState.CurrentGame.Lines;
            this.Lines = GameState.CurrentGame.Lines;
            this.Min = GameState.CurrentGame.MinBet;
        }

        public override void FillRequest (HttpRequest request) {
            if (SessionData.Instance.IsFun) {
                request.AddParam(new HttpParam("linesCount", Lines));
                request.AddParam(new HttpParam("betLine", (int)BetLine));
            } else {
                request.AddParam (new HttpParam ("key", SessionData.Instance.SessionKey));
                request.AddParam (new HttpParam ("game", Game));
                request.AddParam (new HttpParam ("action", Action));
                request.AddParam (new HttpParam ("lines", Lines));
                request.AddParam (new HttpParam ("betline", BetLine));
                request.AddParam (new HttpParam ("bet", Bet));
                request.AddParam (new HttpParam ("min", Min));
            }
        }
    }
}
