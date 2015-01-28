using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using UnityEngine;

namespace Core.Server {
    public sealed class AsyncHttpClient : AbstractHttpClient {

        private static readonly AsyncHttpClient asyncHttpClient = new AsyncHttpClient ();

        private AsyncHttpClient () {
        }

        public static AsyncHttpClient Client {
            get { return asyncHttpClient; }
        }

        private static string GetBaseUrl () {
			return ServerTypeExt.GetServerUrl (ServerType.Unused);
        }
        
        public static string GetAbsoluteUrl (string relativeUrl) {
			if(relativeUrl.StartsWith("http://")) {
				return relativeUrl;
			} else {
				return GetBaseUrl () + relativeUrl;
			}        
        }

        /*
         * executes HTTP POST request
         * request - request data (headers, cookies etc)
         * serverErrorHandler - handler for server errors
         */
        public static void DoPost (HttpRequest request, Action<WebException> serverErrorHandler, Action<String, WebClient> responseHandler) {
            UnityEngine.Debug.Log ("DoPost");
            long bitesNum = 0;
            var webClient = new CookieAwareWebClient (NewCookieContainer (request.CookieCollection), serverErrorHandler);
            
            // Adds headers
            AddHeaders (webClient, request.Headers);
            webClient.Headers [HttpRequestHeader.ContentType] = AppUrlEncoded;
            // creates parameters for POST
            var nameValueCollection = CreateParamsCollection (request.Params);

            //Download progress
            webClient.DownloadProgressChanged += (s, e) =>
            {
                bitesNum += e.BytesReceived;
                UnityEngine.Debug.Log ("TotalBytesDownloaded: " + bitesNum + " of " + e.TotalBytesToReceive);
            };

            //Download complete
            webClient.UploadValuesCompleted += (s, e) =>
            {
                //cheks if exception occurred durin async operation
                if (e.Error != null) {
                    Debug.Log (e.Error.GetBaseException ());
                } else {
                    //logs response, invokes repsonse handler
                    var response = Encoding.Default.GetString (e.Result);
                    if (responseHandler != null) {
                        responseHandler.Invoke (response, webClient);
                    }
                }
            };
            LogsRequest (request);
            webClient.UploadValuesAsync (request.Uri, "POST", nameValueCollection);
        }

        /*
         * executes HTTP Put request
         * request - request data (headers, cookies etc)
         * serverErrorHandler - handler for server errors
         */
        public static void DoPut(HttpRequest request, Action<WebException> serverErrorHandler, Action<String, WebClient> responseHandler) {
            UnityEngine.Debug.Log("DoPut");
            long bitesNum = 0;
            var webClient = new CookieAwareWebClient(NewCookieContainer(request.CookieCollection), serverErrorHandler);

            // Adds headers
            AddHeaders(webClient, request.Headers);
            webClient.Headers[HttpRequestHeader.ContentType] = AppUrlEncoded;
            // creates parameters for POST
            var nameValueCollection = CreateParamsCollection(request.Params);

            //Download progress
            webClient.DownloadProgressChanged += (s, e) => {
                bitesNum += e.BytesReceived;
                UnityEngine.Debug.Log("TotalBytesDownloaded: " + bitesNum + " of " + e.TotalBytesToReceive);
            };

            //Download complete
            webClient.UploadValuesCompleted += (s, e) => {
                //cheks if exception occurred durin async operation
                if (e.Error != null) {
                    Debug.Log(e.Error.GetBaseException());
                } else {
                    //logs response, invokes repsonse handler
                    var response = Encoding.Default.GetString(e.Result);
                    if (responseHandler != null) {
                        responseHandler.Invoke(response, webClient);
                    }
                }
            };
            LogsRequest(request);
            webClient.UploadValuesAsync(request.Uri, "PUT", nameValueCollection);
        }

        public static void DoGet (HttpRequest request, Action<WebException> serverErrorHandler, Action<byte[], WebClient> responseHandler) {
            long bitesNum = 0;

            DoGet (request, serverErrorHandler, responseHandler, (s, e) => {
                bitesNum += e.BytesReceived;
                Debug.Log ("TotalBytesDownloaded: " + bitesNum + " of " + e.TotalBytesToReceive);
            });

        
        }

        public static void DoGet (HttpRequest request, Action<WebException> serverErrorHandler, Action<String, WebClient> responseHandler) {
            long bitesNum = 0;

            DoGet (request, serverErrorHandler, (s, e) =>
            {
                string response = Encoding.Default.GetString (s);
                if (responseHandler != null) {
                    responseHandler.Invoke (response, e);
                }
            }, (s, e) =>
            {
                bitesNum += e.BytesReceived;
                Debug.Log ("TotalBytesDownloaded: " + bitesNum + " of " + e.TotalBytesToReceive);
            });
        }

        public static void DoGet (HttpRequest request, Action<WebException> serverErrorHandler, 
                                  Action<byte[], WebClient> responseHandler, DownloadProgressChangedEventHandler DownloadProgressChanged) {
            UnityEngine.Debug.Log ("DoGet");

            var webClient = new CookieAwareWebClient (NewCookieContainer (request.CookieCollection), serverErrorHandler);
            
            // Adds headers
            AddHeaders (webClient, request.Headers);

            //Download progress
            webClient.DownloadProgressChanged += DownloadProgressChanged;

            //Download complete
            webClient.DownloadDataCompleted += (s, e) =>
            {
                //cheks if exception occurred durin async operation
                if (e.Error != null) {
                    UnityEngine.Debug.Log (e.Error.GetBaseException ());
                } else {
                    //logs response, invokes repsonse handler
                    UnityEngine.Debug.Log ("Complete");
                    //string response = Encoding.Default.GetString(e.Result);
                    if (responseHandler != null) {
                        byte[] response = e.Result;
                        responseHandler.Invoke (response, webClient);
                    }
                }
            };
            LogsRequest (request);
            webClient.DownloadDataAsync (request.Uri);
        }
  
    }
}
