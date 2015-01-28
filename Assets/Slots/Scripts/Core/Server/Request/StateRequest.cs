using System;

namespace Core.Server.Request {
    public sealed class StateRequest : BaseRequest {

        public string Game;

        public StateRequest(string game) {
            this.Game = game;
        }

        public override void FillRequest (HttpRequest request) {
            if (!SessionData.Instance.IsFun) {
                request.AddParam(new HttpParam("game", Game));
                request.AddParam(new HttpParam("key", SessionData.Instance.SessionKey));
                request.AddParam(new HttpParam("action", "state"));
            }
        }

    }
}
