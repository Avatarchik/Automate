using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_ANDROID
public static class AndroidUtil {

    static AndroidJavaClass AndroidUtilClass;
    static AndroidJavaClass UnityPlayer;
    static AndroidJavaObject CurrentActivity;
    static AndroidJavaObject CurrentContext;

    static AndroidUtil () {
        AndroidJNI.AttachCurrentThread ();
        AndroidUtilClass = new AndroidJavaClass ("com.vclub.utils.AndroidUtil");

        UnityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
        CurrentContext = CurrentActivity.Call<AndroidJavaObject> ("getApplicationContext");

    }

    public static string GetEmail () {
        var email = AndroidUtilClass.CallStatic<string> ("getEmail", new object[] {
            CurrentContext
        });
        Debug.Log (string.Format ("Device email [{0}]", email));
        return email;
    }

    public static string GetDeviceId () {
        var deviceId = AndroidUtilClass.CallStatic<string> ("getDeviceId", new object[] {
            CurrentContext
        });
        Debug.Log (string.Format ("Device ID [{0}]", deviceId));
        return deviceId;
    }

    public static string ReadPersistentSetting (string key, string defaultValue) {
        var value = AndroidUtilClass.CallStatic<string> ("readPersistentSetting", new string[] {
            key,
            defaultValue
        });
        return value;
    }

    public static void WritePersistentSetting (string key, string defaultValue) {
        AndroidUtilClass.CallStatic ("writePersistentSetting", new string[] {
            key,
            defaultValue
        });
    }

    public static void CreateDirectory (string path) {
        AndroidUtilClass.CallStatic ("createDirectory", new string[] {
            path
        });
    }
}

#endif