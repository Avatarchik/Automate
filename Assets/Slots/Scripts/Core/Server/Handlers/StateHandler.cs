using Core.Server.Request;
using Core.Server.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;

namespace Core.Server.Handlers {
    public sealed class StateHandler : BaseJsonHandler<StateRequest, StateResponse> {
        const string FUN_METHOD_URL = "/mobile/game/state/{0}";

        public StateHandler (StateRequest request) : base(request) {
        }

        public override string GetUrl () {
            return string.Format(FUN_METHOD_URL, Request.Game);
        }

        public override RequestType GetRequestType () {
            return RequestType.Get;
        }

        protected override StateResponse Deserialize (JObject o) {
            return new StateResponse (o);
        }
    }
}
