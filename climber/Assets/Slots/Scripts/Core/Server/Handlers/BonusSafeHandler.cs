using Core.Server.Request;
using Core.Server.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;
using UnitySlot;

namespace Core.Server.Handlers {
    public class BonusSafeHandler : BaseJsonHandler<BonusSafeRequest, BonusSafeResponse> {
        const string FUN_METHOD_URL = "/mobile/game/{0}/{1}";
        const string REAL_METHOD_URL = "/game.json";

        public BonusSafeHandler (BonusSafeRequest request) : base(request) {
        }

        public override string GetUrl () {
            if (SessionData.Instance.IsFun) {
                return string.Format(FUN_METHOD_URL, GetGameType(), Request.Game);
            } else {
                return REAL_METHOD_URL;
            }
        }

        public override RequestType GetRequestType () {
            return RequestType.Post;
        }

        protected override BonusSafeResponse Deserialize (JObject o) {
            return new BonusSafeResponse (o);
        }

        private string GetGameType() {
            string game;
            if (Request.Game == "garage" || Request.Game == "pirate") {
                game = GameState.CurrentGame.CurrentSuperGame;
            } else {
                game = "safe";
            }
            return game;
        }
    }
}
