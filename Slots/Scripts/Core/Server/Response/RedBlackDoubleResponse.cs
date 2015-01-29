using Newtonsoft.Json.Linq;

namespace Core.Server.Response {
    public class RedBlackDoubleResponse : BaseResponse {

        public bool IsWin {
            get;
            set;
        }

        public RedBlackDoubleResponse (JObject o) : base(o) {
            IsWin = JsonUtil.GetInt (o, "win", 0) > 0;
        }

    }
}
