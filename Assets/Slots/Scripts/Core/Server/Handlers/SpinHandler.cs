using Core.Server.Request;
using Core.Server.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;

namespace Core.Server.Handlers {
    public class SpinHandler : BaseJsonHandler<SpinRequest, SpinResponse> {
        const string FUN_METHOD_URL = "/mobile/game/spin/{0}";

        public SpinHandler (SpinRequest request) : base(request) {
        }

        public override string GetUrl () {
            return string.Format(FUN_METHOD_URL, Request.Game);
        }

        public override RequestType GetRequestType () {
            return RequestType.Post;
        }

        protected override SpinResponse Deserialize (JObject o) {
            return new SpinResponse (o);
        }
    }
}
