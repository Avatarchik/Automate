using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public partial class LogUtil {

    public static bool Logoff = true;
    public static LogType Level = LogType.INFO; // Write file level
    static IDictionary<LogType, string> logMap = new Dictionary<LogType, string> ();

    public static string ApplicationPath {
        get;
        set;
    }

    static LogUtil () {
        logMap.Add (LogType.VERBOSE, " VERBOSE ");
        logMap.Add (LogType.DEBUG, " DEBUG ");
        logMap.Add (LogType.INFO, " INFO ");
        logMap.Add (LogType.WARN, " WARN ");
        logMap.Add (LogType.ERROR, " ERROR ");
    }
        
    private static void WriteLog (LogType type, string tag, string msg) {
        if (type >= Level) {
            string logPath = ApplicationPath + "/Logs/";
            Console.WriteLine (" PATH [{0}] ", logPath);
            if (!Directory.Exists (logPath)) {
                Directory.CreateDirectory (logPath);
            }
            try {
                msg = new StringBuilder ().Append ("\r\n").Append (GetDateformat (DateFormat.yyyyMMddHHmmss))
                        .Append (logMap [type]).Append (tag).Append (" ").Append (msg).ToString ();
                
                string fileName = new StringBuilder ().Append ("vclub-").Append (GetDateformat (DateFormat.yyyyMMdd))
                        .Append (".log").ToString ();
                
                RecordLog (logPath, fileName, msg);
            } catch (Exception e) {
                LogUtil.E ("LogUtil: ", e.Message);
            }
        }
    }
        
    private static void RecordLog (string logPath, string fileName, string msg) {
        try {
            string filePath = logPath + fileName;
            FileExist (filePath);
            string logTemp = string.Empty;
            logTemp += msg;
            FileStream fs = new FileStream (filePath, FileMode.Append);
            StreamWriter sw = new StreamWriter (fs, Encoding.Default);
            sw.WriteLine (logTemp);
            sw.Close ();
            fs.Close ();
        } catch (IOException) {
            RecordLog (logPath, fileName, msg);
        }
    }
        
    private static void FileExist (string path) {
        if (!File.Exists (path)) {
            FileStream fs = new FileStream (path, FileMode.Create);
            fs.Close ();
        }
    }
        
    private static string GetEnumValue (Enum value) {
        FieldInfo fi = value.GetType ().GetField (value.ToString ());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes (typeof(DescriptionAttribute), false);
        if (attributes != null && attributes.Length > 0) {
            return attributes [0].Description;
        } else {
            return value.ToString ();
        }
    }
        
    private static string GetDateformat (DateFormat pattern) {
        return DateTime.Now.ToString (GetEnumValue (pattern));
    }
    
    public enum LogType {
        VERBOSE,
        DEBUG,
        INFO,
        WARN,
        ERROR
    }
    
    public enum DateFormat {
        [Description("yyyy-MM-dd")]
        yyyyMMdd,
        [Description("yyyy-MM-dd HH:mm:ss")]
        yyyyMMddHHmmss
    }
    
    public static void V (object tag, string msg) {
        Debug.Log (string.Format (" {0} {1} {2}", "VERBOSE", tag.GetType().Name, msg));
//        WriteLog (LogType.VERBOSE, tag.GetType().Name, msg);
    }
        
    public static void D (object tag, string msg) {
        Debug.Log (string.Format (" {0} {1} {2}", "DEBUG", tag.GetType().Name, msg));
//        WriteLog (LogType.DEBUG, tag.GetType().Name, msg);
    }
        
    public static void I (object tag, string msg) {
        Debug.Log (string.Format (" {0} {1} {2}", "INFO", tag.GetType().Name, msg));
//        WriteLog (LogType.INFO, tag.GetType().Name, msg);
    }
        
    public static void W (object tag, string msg) {
        Debug.Log (string.Format (" {0} {1} {2}", "WARN", tag.GetType().Name, msg));
//        WriteLog (LogType.WARN, tag.GetType().Name, msg);
    }
        
    public static void E (object tag, string msg) {
        Debug.Log (string.Format (" {0} {1} {2}", "ERROR", tag.GetType().Name, msg));
//        WriteLog (LogType.ERROR, tag.GetType().Name, msg);
    }
}

