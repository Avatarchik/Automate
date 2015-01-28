using System;

namespace Core.Server.Request {
    public sealed class EmptyRequest : BaseRequest {

        public EmptyRequest() {
        }

        public override void FillRequest(HttpRequest request) {
        }
    }
}
