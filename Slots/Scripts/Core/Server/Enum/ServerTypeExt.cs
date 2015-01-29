using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Server {
    public static class ServerTypeExt {
        public static string GetServerUrl (this ServerType serverType) {
            return "http://54.171.148.252/slots";
        }

    }
}
