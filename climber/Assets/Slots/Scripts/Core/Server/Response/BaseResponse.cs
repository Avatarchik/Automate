using System;
using Newtonsoft.Json.Linq;
using UnitySlot;

namespace Core.Server.Response {
    [Serializable()]
    public abstract class BaseResponse {

        public double Balance {
            get;
            set;
        }
        public Int64 Exp {
            get;
            set;
        }

        public Int64 MinExperience {
            get;
            set;
        }

        public Int64 MaxExperience {
            get;
            set;
        }

        public Int64 Level {
            get;
            set;
        }
        public Int64 Points {
            get;
            set;
        }

        protected BaseResponse () {
        }

        public BaseResponse (JObject o) {
            SessionData session = SessionData.Instance;
            if (!session.IsFun) {
                if (o != null) {
                    Balance = JsonUtil.GetDouble (o, "balance");
                    if (Math.Abs (Balance) > 0.0) {
                        User.Coins = Balance;
                    }

                    JObject level = JsonUtil.GetJObject (o, "level");
                    JObject status = JsonUtil.GetJObject (o, "status");
                    JObject bonus = JsonUtil.GetJObject (o, "bonus");
                    //TODO признак получения бонуса, пока забиваем на него.
                    //JObject receivedBonus = JsonUtil.GetJObject (o, "bonus_up");
                    //if (receivedBonus != null) {
                    //    session.setReceivedBonus (receivedBonus.getString ("title"),
                    //    receivedBonus.getDouble ("sum"));
                    //}
                }
            }
        }
    }
}
