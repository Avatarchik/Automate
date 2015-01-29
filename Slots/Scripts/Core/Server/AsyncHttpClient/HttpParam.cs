using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Server {
    public class HttpParam {

        public string Name { get; private set; }

        public string Value { get; private set; }

        public HttpParam (string name, string value) {
            Name = name;
            Value = value;
        }

        public HttpParam (string name, int value) {
            Name = name;
            Value = Convert.ToString (value);
        }

        public HttpParam (string name, double value) {
            Name = name;
            Value = NumberUtil.FormatDouble (value);
        }

        public bool NotEmpty() {
            return !string.IsNullOrEmpty (Name) && !string.IsNullOrEmpty (Value);
        }

        public override string ToString () {
            return String.Format ("{0}={1}", Name, Value);
        }
    }
}
