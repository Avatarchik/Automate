using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Server.Response {
    public class BonusDoorResponse : BonusSafeResponse {

        public BonusDoorResponse (JObject o) : base(o) {
            Balance = (Int64)JsonUtil.GetFloat (o, "balance");
            Coef = (int)JsonUtil.GetInt (o, "coef");
            Score = JsonUtil.GetDouble (o, "scores");
            Lives = (int)JsonUtil.GetInt (o, "life");
        }

    }
}
