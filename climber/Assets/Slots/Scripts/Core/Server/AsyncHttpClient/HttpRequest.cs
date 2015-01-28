using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Core.Server {
    /*
     * model for http request data
     */
    public class HttpRequest { 

        public Uri Uri { get; private set; }

        public CookieCollection CookieCollection { get; private set; }

        public List<HttpHeader> Headers { get; private set; }

        public List<HttpParam> Params { get; private set; }

        public HttpRequest (Uri uri) {
            Uri = uri;
            CookieCollection = new CookieCollection ();
            Headers = new List<HttpHeader> ();
            Params = new List<HttpParam> ();
        }

        /*
         * Adds coockie to request
         */
        public void AddCookie (Cookie cookie) {
            CookieCollection.Add (cookie);
        }

        /*
         * Adds coockies to request 
         */
        public void AddCookies (CookieCollection collection) {
            CookieCollection.Add (collection);
        }

        /*
         * Adds HTTP header to request
         */
        public void AddHeader (HttpHeader header) {
            Headers.Add (header);
        }

        /*
         * Adds HTTP headers to request
         */
        public void AddHeaders (List<HttpHeader> headers) {
            Headers.AddRange (headers);
        }

        /*
         * Adds Http parameter to request
         */
        public void AddParam (HttpParam param) {
            if (param.NotEmpty ()) {
                Params.Add (param);
            }
        }

        /*
         * Adds Http parameters to request
         */
        public void AddParams (List<HttpParam> httpParams) {
            Params.AddRange (httpParams);
        }

        public string RequestParams () {
            StringBuilder builder = new StringBuilder ();
            int count = Params.Count;
            for (int i = 0; i < count; i++) {
                builder.Append (Params [i].ToString ());
                if (i != count - 1) {
                    builder.Append ("&");
                }
            }
            return builder.ToString ();
        }

    }
}
