using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Core.Server.Response {
    public sealed class StateResponse : BaseResponse {

        public List<double> Bets {
            get;
            set;
        }

        public double DefaultBet {
            get;
            set;
        }

        public StateResponse (JObject o) : base(o) {
            Bets = JsonUtil.JarrayToList<double>(o, "bets");
            DefaultBet = JsonUtil.GetDouble(o, "defaultBet");
        }
    }
}
