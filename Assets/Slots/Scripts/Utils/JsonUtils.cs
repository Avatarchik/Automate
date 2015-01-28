using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;

public static class JsonUtil {

    public static string GetString (JObject jObj, string tag) {
        return (string)jObj [tag] ?? null;
    }

    public static Int64 GetInt (JObject jObj, string tag) {
        return jObj [tag] != null ? (Int64)jObj [tag] : 0;
    }
    
    public static Int64 GetInt (JObject jObj, string tag, int defaultValue) {
        return jObj [tag] != null ? (Int64)jObj [tag] : defaultValue;
    }

    public static float GetFloat (JObject jObj, string tag) {
        return jObj [tag] != null ? (float)jObj [tag] : 0f;
    }

    public static double GetDouble (JObject jObj, string tag) {
        double result;
        try {
            result = jObj [tag] != null ? (double)jObj [tag] : 0.0;
        } catch (InvalidCastException e) {
            result = Convert.ToDouble (JsonUtil.GetInt (jObj, tag));
        } catch (ArgumentException e) {
            return Convert.ToDouble(GetString(jObj, tag));
        }
        return result;
    }

    public static double GetDouble (JObject jObj, string tag, double defaultValue) {
        double result;
        try {
            result = jObj [tag] != null ? (double)jObj [tag] : defaultValue;
        } catch (InvalidCastException e) {
            result = Convert.ToDouble (JsonUtil.GetInt (jObj, tag));
        }
        return result;
    }

    public static double GetStatusDouble (JObject jObj, string tag) {
        double result = 0.0;
        if (jObj [tag] != null) {
            string o = Regex.Replace (GetString (jObj, tag), "(\\.0{1,})", "");
            try {
                result = Convert.ToDouble (o);
            } catch (InvalidCastException e) {
                result = Convert.ToDouble (Convert.ToInt64 (o));
            }
        }
        return result;
    }

    public static JObject GetJObject (JObject jObj, string tag) {
        return (JObject)jObj [tag] ?? null;
    }

    public static JArray GetJArray (JObject jObj, string tag) {
        return (JArray)jObj [tag] ?? null;
    }

    public static List<T> JarrayToList<T> (JObject jObj, string tag) {
        var result = new List<T> ();
        var array = GetJArray (jObj, tag);
        if (array != null) {
            foreach (JToken token in array) {
                var item = (T)Convert.ChangeType (token.ToString (), typeof(T));
                result.Add (item);
            }
        }
        return result;
    }
}
