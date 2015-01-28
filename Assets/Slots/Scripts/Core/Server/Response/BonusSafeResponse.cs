using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Server.Response {
    public class BonusSafeResponse : BaseResponse {

        public double Score {
            get;
            set;
        }

        public int Coef {
            get;
            set;
        }

        public int Lives {
            get;
            set;
        }

        public int StartDoor {
            get;
            set;
        }

        public int Item {
            get;
            set;
        }

        public int Dice {
            get;
            set;
        }

        public BonusSafeResponse (JObject o) : base(o) {
            Balance = (Int64)JsonUtil.GetFloat (o, "balance");
            StartDoor = (int)JsonUtil.GetInt (o, "startDoor");
            Coef = (int)JsonUtil.GetInt (o, "coef");
            Score = JsonUtil.GetDouble (o, "scores");
            Lives = (int)JsonUtil.GetInt (o, "life");
            Item = (int)JsonUtil.GetInt (o, "item");
            Dice = (int)JsonUtil.GetInt (o, "dice");
        }

    }
}
