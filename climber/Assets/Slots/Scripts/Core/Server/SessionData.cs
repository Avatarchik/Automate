using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Net;
using UnitySlot;

namespace Core.Server {
    /*
     * storage for client data
     */
    public class SessionData {

        private static SessionData instance;

        public string Login { get; set; }

        public string Email { get; set; }

        public string UserAgentHeaderVal { get; set; }

        public string XProtocol { get; set; }

        public string TrValue { get; set; }

        public string Credential { get; set; }

        /********************************************************
         *                  USER PROFILE
         ********************************************************/
        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Surname { get; set; }

        public string Birthday { get; set; }

        public string Country { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public bool IsPlayMusic {
            get {
                return GamePrefs.GetBool (Constants.SettingsMusic, true);
            }
            set {
                GamePrefs.Add (Constants.SettingsMusic, value);
            }
        }

        public bool IsPlaySound {
            get {
                return GamePrefs.GetBool (Constants.SettingsSound, true);
            }
            set {
                GamePrefs.Add (Constants.SettingsSound, value);
            }
        }

        public bool IsShowNotification {
            get {
                return GamePrefs.GetBool (Constants.SettingsNotification, true);
            }
            set {
                GamePrefs.Add (Constants.SettingsNotification, value);
            }
        }

        public bool IsAutoplay {
            get {
                return GamePrefs.GetBool (Constants.SettingsAutoplay, true);
            }
            set {
                GamePrefs.Add (Constants.SettingsAutoplay, value);
            }
        }

        public bool IsApplyEula {
            get {
                return GamePrefs.GetBool(Constants.SettingsApplyEula, false);
            }
            set {
                GamePrefs.Add(Constants.SettingsApplyEula, value);
            }
        }

        public string PreviousPopup { get; set; }

        public SlotHolder FunSlotList { get; set; }

        public ServerType ServerType { get; set; }
        //TODO: temporary solution
        public bool IsRegistered { 
            get {
                return GamePrefs.GetBool (Constants.SettingsRegistered, false);
            }
        }

        public bool IsFun { get; set; }

        public bool IsLoggenIn { get; set; }
        public string SessionKey { get; set; }

        private double MoneyReal = 0.0;
        private double MoneyFun = 0.0;

        /*
         * coockie required for server
         */
        public CookieCollection CookieCollection { get; set; }

        private SessionData () {
            CookieCollection = new CookieCollection ();
        }

        public static SessionData Instance {
            get { 
                if (instance == null) {
                    instance = new SessionData ();
                }
                return instance; 
            }
        }

        public void StartSession () {
            System.Random r = new System.Random ();
            byte[] key = new byte[16];
            r.NextBytes (key);
            SessionKey = StringUtil.BytesToString(key);
        }

        public AsyncOperation LoadGamehall () {
            var loadingScene = string.Format ("Gamehall_{0}", SessionData.Instance.IsFun ? "fun" : "real");
            return Application.LoadLevelAsync (loadingScene);
        }

        /********************************************************
         *                  USER LEVEL
         ********************************************************/
        

        /*Fun bets*/
        public List<double> FunBets { get; set; }

        public double DefaultBet { get; set; }

        public int NextLevelBonus { get; set; }


    }
}
