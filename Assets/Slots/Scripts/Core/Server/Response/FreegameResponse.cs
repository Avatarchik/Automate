using Newtonsoft.Json.Linq;

namespace Core.Server.Response {

  public class FreeGameResponse : SpinResponse {

    public FreeGameResponse(JObject o) : base(o) {

    }

  }
}