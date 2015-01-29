using System;

namespace Core.Server.Request {
    [Serializable()]
    public abstract class BaseRequest {

        public abstract void FillRequest(HttpRequest request);
    }
}
