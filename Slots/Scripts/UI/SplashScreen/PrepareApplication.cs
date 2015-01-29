using Core.Server;
using Core.Server.Handlers;
using Core.Server.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySlot;

public class PrepareApplication : MonoBehaviour {
    AsyncOperation ao;
    bool SuccessLoadedList;

    void Start () {
#if UNITY_EDITOR

    SessionData.Instance.UserAgentHeaderVal = "android";
#endif

#if UNITY_IPHONE
        SessionData.Instance.UserAgentHeaderVal = "ios";
#endif

#if UNITY_ANDROID
        SessionData.Instance.UserAgentHeaderVal = "android";
#endif
        var isRegistered = GamePrefs.GetBool (Constants.SettingsRegistered, false);
        string referrer = null;
        if (!isRegistered) {
            #if UNITY_ANDROID

    var googleReferrer = PlayerPrefs.GetString ("googleReferrer");
            var defaultReferrer = GetDefaultReferrer();
            if (!string.IsNullOrEmpty (googleReferrer)) {
                Debug.Log (string.Format ("REFERRER: received google referrer [{0}]", googleReferrer));
                AndroidUtil.WritePersistentSetting(Constants.SettingsRefer, googleReferrer);
            } else if (!string.IsNullOrEmpty(defaultReferrer)) {
                Debug.Log (string.Format ("REFERRER: using default referrer [{0}]", defaultReferrer));
                AndroidUtil.WritePersistentSetting(Constants.SettingsRefer, defaultReferrer);
            }
            referrer = AndroidUtil.ReadPersistentSetting (Constants.SettingsRefer, "");
            SessionData.Instance.IsFun = true;//string.IsNullOrEmpty (referrer);
            GamePrefs.Add(Constants.SettingsUseDebugParam, false);
            #endif

        }

        SessionData.Instance.IsFun = true;//string.IsNullOrEmpty (referrer);
        LogUtil.ApplicationPath = Application.dataPath;

        
        StartCoroutine (Load ());
    }

    IEnumerator Load () {
        Debug.Log ("Loading gamehall...");
        ao = SessionData.Instance.LoadGamehall ();
        if (ao != null) {
            ao.allowSceneActivation = false;
            while (!ao.isDone) {
                Debug.Log (string.Format ("Loading gamehall precent [{0}%]", Mathf.RoundToInt (ao.progress * 100f)));
                if (ao.progress >= 0.9f && (SuccessLoadedList || !SessionData.Instance.IsFun)) {
                    //освобождаем память
                    Resources.UnloadUnusedAssets ();
                    Debug.Log ("Loading gamehall scene...ok");
                    ao.allowSceneActivation = true;
                }
                yield return 0;
            }
        }
    }

    string GetDefaultReferrer () {
        var slotVariables = XmlUtil.Deserialize<SlotVariables> ("variables");
        if (slotVariables != null) {
            return slotVariables.DefaultReferrer;
        }
        return null;
    }


}
 