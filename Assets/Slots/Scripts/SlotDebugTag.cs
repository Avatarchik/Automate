using Core.Server;
using Core.Server.Handlers;
using Core.Server.Request;
using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

[Serializable]
public enum TestUsers {
    [Description("Basic c2xvdC11bml0eS10ZXN0QGdtYWlsLmNvbTpPMVFFb0doQWhRR29jTlo=")]
    slot_unity_test,
    [Description("Basic c2xvdC11bml0eS10ZXN0MUBnbWFpbC5jb206TzFRRW9HaEFoUUdvY05a")]
    slot_unity_test1,
    [Description("Basic c2xvdC11bml0eS10ZXN0MkBnbWFpbC5jb206TzFRRW9HaEFoUUdvY05a")]
    slot_unity_test2,
    [Description("Basic c2xvdC11bml0eS10ZXN0M0BnbWFpbC5jb206TzFRRW9HaEFoUUdvY05a")]
    slot_unity_test3,
    [Description("Basic c2xvdC11bml0eS10ZXN0NEBnbWFpbC5jb206TzFRRW9HaEFoUUdvY05a")]
    slot_unity_test4,
    [Description("Basic c2xvdC11bml0eS10ZXN0NUBnbWFpbC5jb206TzFRRW9HaEFoUUdvY05a")]
    slot_unity_test5,
    [Description("Basic c2xvdC11bml0eS10ZXN0NkBnbWFpbC5jb206TzFRRW9HaEFoUUdvY05a")]
    slot_unity_test6,
    [Description("Basic c2xvdC11bml0eS10ZXN0N0BnbWFpbC5jb206TzFRRW9HaEFoUUdvY05a")]
    slot_unity_test7,
    [Description("Basic c2xvdC11bml0eS10ZXN0OEBnbWFpbC5jb206TzFRRW9HaEFoUUdvY05a")]
    slot_unity_test8,
    [Description("Basic c2xvdC11bml0eS10ZXN0OUBnbWFpbC5jb206TzFRRW9HaEFoUUdvY05a")]
    slot_unity_test9,
    [Description("Basic c2xvdC11bml0eS10ZXN0MTBAZ21haWwuY29tOk8xUUVvR2hBaFFHb2NOWg==")]
    slot_unity_test10,
    [Description("Basic c2xvdC11bml0eS10ZXN0MTFAZ21haWwuY29tOk8xUUVvR2hBaFFHb2NOWg==")]
    slot_unity_test11,
    [Description("Basic c2xvdC11bml0eS10ZXN0MTJAZ21haWwuY29tOk8xUUVvR2hBaFFHb2NOWg==")]
    slot_unity_test12,
    [Description("Basic c2xvdC11bml0eS10ZXN0MTNAZ21haWwuY29tOk8xUUVvR2hBaFFHb2NOWg==")]
    slot_unity_test13,
    [Description("Basic c2xvdC11bml0eS10ZXN0MTRAZ21haWwuY29tOk8xUUVvR2hBaFFHb2NOWg==")]
    slot_unity_test14
}

public class SlotDebugTag : MonoBehaviour {

    public string SlotSceneName = "fruit";
    public bool LoadInSlotmenu = true;
    public TestUsers TestUser;

    void Start () {
        if (Application.loadedLevelName != "Slotmenu" && LoadInSlotmenu) {
            Loom.CreateThreadPoolScheduler ();
            GamePrefs.Add (Constants.SettingsSelectedSlot, SlotSceneName);
            SessionData.Instance.IsFun = true;
            Login ();
        }
    }

    private void Login () {
        var credentials = GetPassword(TestUser);
        GamePrefs.Add (Constants.SettingsCredential, credentials);
        SessionData.Instance.Credential = credentials;
        SessionData.Instance.UserAgentHeaderVal = Constants.Android;
        Loom.DispatchToMainThread (() => {
            setupGameMode ();
        });
    }

    public static string GetPassword(TestUsers user) {
        FieldInfo fi = user.GetType().GetField(user.ToString());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes != null && attributes.Length > 0 ? attributes[0].Description : user.ToString();
    }

    void setupGameMode () {
        SessionData session = SessionData.Instance;
        var handler = new StateHandler (new StateRequest (SlotSceneName)).AddOkListener ((response) => {
            Loom.DispatchToMainThread (() => {
                session.FunBets = response.Bets;
                session.DefaultBet = response.DefaultBet;
                Loom.DispatchToMainThread (() => {
                    LoadScene ();
                });
            });
        });

        handler.AddErrorListener (exception => {
            Debug.Log ("Failed user data = " + exception.Message);
        });

        handler.DoRequest ();
    }

    void LoadScene () {
        Debug.Log ("=======================================================================================================");
        Debug.Log ("----- WARNING! Make sure to enable Main, Double and Bonus game roots to evade errors in slotmenu. -----");
        Debug.Log ("=======================================================================================================");
        Application.LoadLevel ("Slotmenu");
        Debug.Log (SlotSceneName + " is loaded in slotmenu for test");
        Application.LoadLevelAdditive (SlotSceneName);
    }
}
