using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace Core.Server {
    public abstract class AbstractHttpClient {

        protected static readonly string AppUrlEncoded = "application/x-www-form-urlencoded";

        /*
         * Adds headers to request
         */
        protected static void AddHeaders (WebClient webClient, List<HttpHeader> headers) {
            foreach (var header in headers) {
                webClient.Headers.Add (header.Name, header.Value);
            }
        }

        /*
        * creates container with cookies
        */
        protected static CookieContainer NewCookieContainer (CookieCollection cookieCollection) {
            var cookieContainer = new CookieContainer ();
            cookieContainer.Add (cookieCollection);
            return cookieContainer;
        }

        /*
         * Creates collection with parameters for POST method
         */
        protected static NameValueCollection CreateParamsCollection (List<HttpParam> httpParams) {
            var reqparms = new NameValueCollection ();
            foreach (var httpParam in httpParams) {
                reqparms.Add (httpParam.Name, httpParam.Value);
            }
            return reqparms;
        }

        /*
        * Logs request data
        */
        protected static void LogsRequest (HttpRequest httpRequest) {
            Console.WriteLine ("Request " + httpRequest.Uri);
            foreach (var item in httpRequest.Params) {
                Console.WriteLine (item.ToString ());
            }
        }

    }
}
