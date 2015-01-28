using UnityEngine;
using Core.Server;

public class DebugConstant : MonoBehaviour {

    public const bool IS_PROD = true;
    public bool IsFun;
    public bool IsShowWelcomePopup;

    void Awake () {
        GamePrefs.Add (Constants.SettingsShowWelcomePopup, IsShowWelcomePopup);
        var useDebug = GamePrefs.GetBool (Constants.SettingsUseDebugParam, true);
        if (useDebug) {
            SessionData.Instance.IsFun = IsFun;
        }
    }

    void Start () {
    }

    //TODO Пусть тут полежит временно
//    public void SetUserData () {
        //            var request = NewHttpRequest (UserDataPath);
        //            UnityEngine.Debug.Log (request.Uri);
        //            AsyncHttpClient.Client.DoGet (request, ServerErrorHandler, (s, client) =>
        //            {
        //                Loom.DispatchToMainThread (() =>
        //                {
        //                    UnityEngine.Debug.Log (s);
        //                    UserData userData = JsonConvert.DeserializeObject<UserData> (s);
        //                    User.Level = userData.Data.Level.LevelNumber;
        //                    User.Coins = userData.Data.Account.BalanceFun;
        //                    User.Experience = userData.Data.Level.Exp;
        //                    User.MaxExperience = userData.Data.Level.MaxExp;
        //                    User.MinExperience = userData.Data.Level.MinExp;
        //                    User.CurrentStatus = convertStatusType (userData.Data.Status.CurrentStatus.Name);
        //                    User.NextStatus = convertStatusType (userData.Data.Status.NextStatus.Name);
        //                    User.CurrentStatusPoints = userData.Data.Status.Points;
        //                    User.NextStatusPoints = userData.Data.Status.NextStatus.Points;
        //                    UnityEngine.Debug.Log (User.Level);
        //                });
        //                
        //            });
        //        }
}
