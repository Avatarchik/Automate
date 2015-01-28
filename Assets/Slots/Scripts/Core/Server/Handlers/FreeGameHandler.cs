using Core.Server.Request;
using Core.Server.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;

namespace Core.Server.Handlers {
    public sealed class FreeGameHandler : BaseJsonHandler<FreeGameRequest, FreeGameResponse> {
        const string FUN_METHOD_URL = "/mobile/game/freegame/{0}";

        public FreeGameHandler (FreeGameRequest request) : base(request) {
        }

        public override string GetUrl () {
            return string.Format(FUN_METHOD_URL, Request.Game);
        }

        public override RequestType GetRequestType () {
            return RequestType.Post;
        }

        protected override FreeGameResponse Deserialize (JObject o) {
            return new FreeGameResponse (o);
        }
    }
}
