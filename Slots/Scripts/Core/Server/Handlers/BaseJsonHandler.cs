using Core.Server.Request;
using Core.Server.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnitySlot;

namespace Core.Server.Handlers {
    public  abstract class BaseJsonHandler<TRequest , TResponse> : BaseHandler where TRequest : BaseRequest where TResponse : BaseResponse {

        const string ERROR_VALIDATION = "validation";
        const string FIELD_STATE = "code";
        const string FIELD_ERROR = "error";
        const string FIELD_ERROR_ARRAY = "errors";
        const string FIELD_DOMAIN = "domain";
        const string FIELD_MESSAGE = "message";
        const string FIELD_SYSNAME = "sysname";

        const string XProtocolHeader = "x-protocol";
        const string SlotAgentHeader = "slot-agent";
        const string UserAgentHeader = "user-agent";

        /*
        * delegate to handle server errors
        */
        protected readonly Action<WebException> ServerErrorHandler;
        protected readonly Action<String, WebClient> ServerSuccessHandler;
        protected const string TrParam = "tr[]";
        protected TRequest Request;
        protected HttpRequest PreparedRequest;
        Action<String, WebClient> OnOkListener = new Action<string, WebClient> (OnSuccess);

        public static BaseJsonHandler<TRequest, TResponse> Handler {
            get;
            set;
        }

        protected static Action<TResponse> OnSuccessResultListener {
            get;
            set;
        }

        protected static Action<Exception> OnErrorListener {
            get;
            set;
        }

        protected BaseJsonHandler (TRequest request) {
            ServerErrorHandler = LogsServerError;
            ServerErrorHandler += HandleServerError;
            Request = request;
            Handler = this;
        }

        /*
         * creates http request model with common values
         */
        public HttpRequest CreateHttpRequest (string requestUrl) {
            var session = SessionData.Instance;
            var request = new HttpRequest (new Uri (requestUrl));
            if (session.IsFun) {
                if (session.Credential != null) {
                    request.AddHeader(new HttpHeader(HttpRequestHeader.Authorization.ToString(), session.Credential));
                    Debug.Log("session.Credential " + session.Credential);
                }
                request.AddHeader(new HttpHeader(SlotAgentHeader, session.UserAgentHeaderVal));
            } else {
                request.AddHeader (new HttpHeader (UserAgentHeader, "android_client;com.vegaslot-mobi"));
                request.AddHeader (new HttpHeader (XProtocolHeader, "vclub"));
            }
            request.AddHeader (new HttpHeader (XProtocolHeader, session.XProtocol));
            request.AddCookies (session.CookieCollection);
            return request;
        }

        public void FillRequest (HttpRequest request) {
            Request.FillRequest (request);
        }

        /*
         * Logs server error
         */
        protected void LogsServerError (WebException exception) {
            UnityEngine.Debug.Log (String.Format ("Exception message : {0}", exception.Message));
            UnityEngine.Debug.Log (String.Format ("Exception status : {0}", exception.Status));
            LogUtil.E (Handler, string.Format ("URL [{0}] exception message [{1}] status [{2}]",
            AsyncHttpClient.GetAbsoluteUrl (GetUrl ()), exception.Message, exception.Status));
            using (var errorResponse = (HttpWebResponse) exception.Response) {
                if (errorResponse != null) {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream())) {
                        var error = reader.ReadToEnd ();
                        HttpStatusCode errorCode = ((HttpWebResponse)exception.Response).StatusCode;
                        LogUtil.E (Handler, string.Format ("Handler [{0}] failed: [{1}] error code [{2}]",
                        AsyncHttpClient.GetAbsoluteUrl (GetUrl ()), error, errorCode));
                    }
                }
            }
            GameState.CurrentGame.IsDone = true;
            GameState.CurrentGame.IsError = true;
            //TODO show error connection popup
        }

        /*
         * Handles server error
         */
        protected virtual void HandleServerError (WebException exception) {
            HttpStatusCode errorCode = ((HttpWebResponse)exception.Response).StatusCode;
            LogUtil.E (Handler, string.Format ("Handler [{0}] failed: [{1}] error code [{2}]",
            AsyncHttpClient.GetAbsoluteUrl (GetUrl ()), exception.Message, errorCode));
            switch (errorCode) {
                case HttpStatusCode.Forbidden :
                OnErrorListener.Invoke(exception);
                break;
            }
        }

        public abstract string GetUrl ();

        public abstract RequestType GetRequestType ();

        protected abstract TResponse Deserialize (JObject o);

        public BaseJsonHandler<TRequest, TResponse> AddOkListener (Action<TResponse> listener) {
            OnSuccessResultListener = listener;
            return this;
        }

        public BaseJsonHandler<TRequest, TResponse> AddErrorListener (Action<Exception> listener) {
            OnErrorListener = listener;
            return this;
        }

        public Action<TResponse> GetSuccessResultListener () {
            return OnSuccessResultListener;
        }

        public Action<Exception> GetErrorListener () {
            return OnErrorListener;
        }

        public override void DoRequest ()
        {
           
            var url = AsyncHttpClient.GetAbsoluteUrl (this.GetUrl ());
           
            PreparedRequest = this.CreateHttpRequest (url);
            this.FillRequest (PreparedRequest);
            LogUtil.D (Handler, string.Format ("URL: {0}\nRequest:{1}",
            PreparedRequest.Uri.AbsoluteUri, PreparedRequest.RequestParams ()));
            switch (this.GetRequestType ()) {
                case RequestType.Get:
                AsyncHttpClient.DoGet (PreparedRequest, ServerErrorHandler, OnOkListener);
                break;
                case RequestType.Post:
                AsyncHttpClient.DoPost (PreparedRequest, ServerErrorHandler, OnOkListener);
                break;
                case RequestType.Put:
                AsyncHttpClient.DoPut(PreparedRequest, ServerErrorHandler, OnOkListener);
                break;
            }
        }

        static void OnSuccess (string s, WebClient client) {
            Debug.Log (string.Format ("{0} Response: Message [{1}]", Handler.GetType ().Name, s));
            if (!string.IsNullOrEmpty (s)) {
                JObject response = JObject.Parse (s);
                if (!ProcessErrorObject (response, null)) {
                    JObject data = JsonUtil.GetJObject (response, "data");
                    TResponse handlerResponse = Handler.Deserialize (data);
                    if (OnSuccessResultListener != null) {
                        OnSuccessResultListener.Invoke (handlerResponse);
                        var uri = Handler.PreparedRequest.Uri;
                        var cookieCollection = ((CookieAwareWebClient)client).CookieContainer.GetCookies (uri);
                        SessionData.Instance.CookieCollection.Add (cookieCollection);
                    }
                }
            }
        }

        static bool ProcessErrorObject (JObject o, Exception exception) {
            bool result = false;
            if (o [FIELD_ERROR] != null) {
                JObject errorJObject = JsonUtil.GetJObject(o, FIELD_ERROR);

                if (errorJObject != null) {
                    int state = (int)JsonUtil.GetInt(errorJObject, FIELD_STATE);
                    JArray errors = JsonUtil.GetJArray(errorJObject, FIELD_ERROR_ARRAY);

                    if (errors.Count > 0) {
                        // Process simple error
                        bool isProcessed = false;

                        foreach (JObject error in errors) {
                            ProcessSimpleError(error, state, exception);
                            isProcessed = true;
                            break;
                        }
                        result = true;
                    } else if (state != (int)HttpStatusCode.OK) {
                        OnErrorListener.Invoke(new ParseResponseException("Unknown error occurred!", state, "unknown", exception));
                    }
                }
            }
            return result;
        }

        static void ProcessSimpleError (JObject o, int state, Exception exception) {
            string domain = JsonUtil.GetString(o, FIELD_DOMAIN);
            string message = JsonUtil.GetString(o, FIELD_MESSAGE);
            LogUtil.E(Handler, string.Format("Error, name: [{0}], msg: [{1}]", domain, message));
            GameState.CurrentGame.IsDone = true;
            GameState.CurrentGame.IsError = true;
            //TODO show error connection popup
            OnErrorListener.Invoke(new ParseResponseException(message, state, message, exception));
        }

        static void ProcessValidationError (JObject o, int state, Exception exception) {
            Exception ex = exception;
            // Process validation errors (may be several)
            JObject fields = JsonUtil.GetJObject (o, ERROR_VALIDATION);
            IEnumerable<JProperty> names = fields.Properties ();
            foreach (JProperty name in names) {
                string fieldName = name.Name;
                JObject field = JsonUtil.GetJObject (fields, fieldName);
                string domain = JsonUtil.GetString (field, FIELD_DOMAIN);
                string message = JsonUtil.GetString (field, FIELD_MESSAGE);
                ValidationException e = new ValidationException(message, state, domain, fieldName, exception);
                LogUtil.E (Handler, e.ToString ());
                ex = e;
            }
            OnErrorListener.Invoke (ex);
        }
    }
}
