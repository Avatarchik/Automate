﻿using Core.Server.Request;
using Core.Server.Response;
using Newtonsoft.Json.Linq;

namespace Core.Server.Handlers {
    public sealed class RedBlackDoubleHandler : BaseJsonHandler<CardsDoubleRequest, RedBlackDoubleResponse> {
        const string FUN_METHOD_URL = "/mobile/game/double/{0}";

        public RedBlackDoubleHandler (CardsDoubleRequest request) : base(request) {
        }

        public override string GetUrl () {
            return string.Format(FUN_METHOD_URL, Request.Game);
        }

        public override RequestType GetRequestType () {
            return RequestType.Post;
        }

        protected override RedBlackDoubleResponse Deserialize (JObject o) {
            return new RedBlackDoubleResponse (o);
        }
    }
}
