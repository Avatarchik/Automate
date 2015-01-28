using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;

namespace Core.Server.Response {
    public class FruitFreeGameResponse : BaseResponse {

        public List<int> MidSymbols {
            get;
            set;
        }

        public int NewPosition {
            get;
            set;
        }

        public double Score {
            get;
            set;
        }

        public int Lives {
            get;
            set;
        }

        public FruitFreeGameResponse (JObject o) : base(o) {
            Balance = (Int64)JsonUtil.GetFloat (o, "balance");
            MidSymbols = JsonUtil.JarrayToList<int> (o, "midSymbols");
            NewPosition = (int)JsonUtil.GetInt (o, "shift");
            Score = JsonUtil.GetDouble (o, "scores");
            Lives = (int)JsonUtil.GetInt (o, "life");
        }

    }
}
