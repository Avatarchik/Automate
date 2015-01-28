namespace Core.Server {
    public class HttpHeader {
        /*
         * header name
         */
        public string Name { get; private set; }
        /*
         * header value
         */
        public string Value { get; private set; }

        public HttpHeader (string name, string value) {
            Value = value;
            Name = name;
        }
    }
}
