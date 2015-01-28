using System;
using System.Net;

namespace Core.Server {
    internal sealed class CookieAwareWebClient : WebClient {
        /*
         * delegate to handle server errors
         */
        private readonly Action<WebException> _errorHandler;

        public CookieContainer CookieContainer { get; private set; }
        /*
         * container for cookies
         */
        public CookieAwareWebClient (CookieContainer container, Action<WebException> errorHandler) {
            CookieContainer = container;
            _errorHandler = errorHandler;
        }

        /**
         *  Adds cookies in request
         */
        protected override WebRequest GetWebRequest (Uri address) {
            var webRequest = base.GetWebRequest (address);
            var request = webRequest as HttpWebRequest;
            if (request != null) {
                request.CookieContainer = CookieContainer;
            }
            return webRequest;
        }

        /*
         * initializes collection with cookies 
         */
        protected override WebResponse GetWebResponse (WebRequest request, IAsyncResult result) {
            WebResponse webResponse;
            try {
                var response = base.GetWebResponse (request, result);
                ReadCookies (response);
                webResponse = response;
            } catch (WebException webException) {
                webResponse = webException.Response;
                if (_errorHandler != null)
                    _errorHandler (webException);
            }
            return webResponse;
        }

        /*
         * initializes collection with cookies 
         */
        protected override WebResponse GetWebResponse (WebRequest request) {
            WebResponse webResponse;
            try {
                var response = base.GetWebResponse (request);
                ReadCookies (response);
                webResponse = response;
            } catch (WebException webException) {
                webResponse = webException.Response;
                if (_errorHandler != null)
                    _errorHandler (webException);
            }
            return webResponse;
        }

        /*
         * reads cookies from response 
         */
        private void ReadCookies (WebResponse webResponse) {
            var response = webResponse as HttpWebResponse;
            if (response != null) {
                var cookies = response.Cookies;

                if (cookies.Count > 0) { // check if cookies are really sent
                    // creates container only with cookies from response
                    CookieContainer = new CookieContainer (cookies.Count);
                    CookieContainer.Add (cookies);
                }
            }
        }

    }
}
