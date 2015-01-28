using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public static class GamePrefs {

    private static readonly Hashtable playerPrefsHashtable = new Hashtable ();
    private static BinaryFormatter bFormatter = new BinaryFormatter ();
    private static byte[] bytes;

    static GamePrefs () {
        //PlayerPrefs.DeleteAll();
        Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
    }

    static void CheckSecretBytes () {
        if (bytes == null) {
            bytes = ASCIIEncoding.ASCII.GetBytes ("vclub" + SystemInfo.deviceUniqueIdentifier.Substring (0, 3));
        }
    }
    
    public static void Add (string key, object value) {
        Loom.DispatchToMainThread (() => {
            CheckSecretBytes ();
            if (!playerPrefsHashtable.ContainsKey (key)) {
                playerPrefsHashtable.Add (key, value);
            } else {
                playerPrefsHashtable [key] = value;
            }
            var m = new MemoryStream ();
            bFormatter.Serialize (m, value);
            var serialized = Convert.ToBase64String (m.GetBuffer ());
            var encryptedValue = Encrypt (serialized);
            Loom.DispatchToMainThread (() => {
                PlayerPrefs.SetString (key, encryptedValue);
            });
            m.Close ();
        });
    }
    
    public static object Get (string key) {
        try {
            Loom.DispatchToMainThread (() => {
                CheckSecretBytes();
            });

            if (playerPrefsHashtable.ContainsKey (key)) {
                return playerPrefsHashtable [key];
            }
            var data = Convert.ToString (Loom.DispatchToMainThreadReturn (() => {
                return PlayerPrefs.GetString (key);
            }));
            //If not blank then load it
            if (!String.IsNullOrEmpty (data)) {
                var decryptedValue = Decrypt (data);
                //Create a memory stream with the data
                var m = new MemoryStream (Convert.FromBase64String (decryptedValue));
                //Load back the scores
                var obj = bFormatter.Deserialize (m);
                Loom.DispatchToMainThread (() => {
                    if (playerPrefsHashtable.Contains(key)) {
                        playerPrefsHashtable[key] = obj;
                    } else {
                        playerPrefsHashtable.Add (key, obj);
                    }
                });

                m.Close ();
                return obj;
            }
        } catch (CryptographicException e) {
            PlayerPrefs.DeleteKey (key);
        } catch (TypeLoadException e) {
            PlayerPrefs.DeleteKey (key);
        }
        return null;
    }

    public static bool GetBool (string key, bool defaultValue) {
        var result = Get (key);
        return result != null ? (bool)result : defaultValue;
    }
    
    public static int GetInt (string key, int defaultValue) {
        var result = Get (key);
        return result != null ? (int)result : defaultValue;
    }

    public static string GetString (string key) {
        var result = Get (key);
        return result != null ? (string)result : "";
    }

    public static string GetString (string key, string defaultValue) {
        var result = Get (key);
        return result != null ? (string)result : defaultValue;
    }

    public static float GetFloat (string key, float defaultValue) {
        var result = Get (key);
        return result != null ? (float)result : defaultValue;
    }
    
    private static string Encrypt (string originalString) {
        if (String.IsNullOrEmpty (originalString)) {
            return "";
        }
        
        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider ();
        MemoryStream memoryStream = new MemoryStream ();
        CryptoStream cryptoStream = new CryptoStream (memoryStream, cryptoProvider.CreateEncryptor (bytes, bytes), CryptoStreamMode.Write);
        StreamWriter writer = new StreamWriter (cryptoStream);
        writer.Write (originalString);
        writer.Flush ();
        cryptoStream.FlushFinalBlock ();
        writer.Flush ();
        return Convert.ToBase64String (memoryStream.GetBuffer (), 0, (int)memoryStream.Length);
    }
    
    private static string Decrypt (string cryptedString) {
        if (String.IsNullOrEmpty (cryptedString)) {
            return "";
        }
        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider ();
        MemoryStream memoryStream = new MemoryStream (Convert.FromBase64String (cryptedString));
        CryptoStream cryptoStream = new CryptoStream (memoryStream, cryptoProvider.CreateDecryptor (bytes, bytes), CryptoStreamMode.Read);
        StreamReader reader = new StreamReader (cryptoStream);
        return reader.ReadToEnd ();
    }

}
