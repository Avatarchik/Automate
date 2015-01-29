using System;
using UnityEngine;
using System.IO;

namespace UnitySlot {
    public static class PhoneUtils {
        public static string GetiPhoneDocumentsPath () { 
            // Your game has read+write access to /var/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/Documents 
            // Application.dataPath returns              
            // /var/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/myappname.app/Data 
            // Strip "/Data" from path 
            string path; 
            #if UNITY_IPHONE || UNITY_EDITOR
                path = Application.dataPath.Substring (0, Application.dataPath.Length - 5); 
                // Strip application name 
                path = path.Substring (0, path.LastIndexOf ('/')) + "/Documents"; 
            #elif UNITY_ANDROID
                path = Application.persistentDataPath;
                path = path.Substring (0, path.LastIndexOf ('/')) + "/slots"; 
                AndroidUtil.CreateDirectory(path);
            #endif
            return path; 
        }

        public static byte[] GetBytes (string str) {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy (str.ToCharArray (), 0, bytes, 0, bytes.Length);
            return bytes;
        }

    }
}

