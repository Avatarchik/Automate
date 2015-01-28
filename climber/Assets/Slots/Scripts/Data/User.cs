using System;
using System.Collections.Generic;
using Core.Server;
using UnityEngine;

namespace UnitySlot {

    public class User {
        private const string KEY = "user-info";
        private static UserInfo userInfo;

        private static UserInfo GetUserInfo () {
            if (userInfo == null) {
                //Init on first run.
                if (userInfo == null) {
                    userInfo = new UserInfo ();
                    GamePrefs.Add (KEY, true);
                } else {
                    var value = GamePrefs.Get (KEY);
                    userInfo = value as UserInfo;
                }
            }

            return userInfo;
        }

        public static StatusType CurrentStatus {
            get { return StatusType.Bronze;
            }
            set{ 
            }
        }

        public static bool IsSlotDownloaded (string id) {
            return GetUserInfo ().DownloadedSlots.Contains (id);
        }

        public static void AddCoins (Int64 coins) {
            Coins += coins;
        }

        public static void AddExperiens (Int64 expiriens) {
            Experience += expiriens;
        }

        public static Int64 Level {
            get {
                return GetUserInfo ().Level;
            }
            set {
                GetUserInfo ().Level = value;
                GamePrefs.Add (KEY, GetUserInfo ());
            }
        }

        public static double Coins {
            get {
                return GetUserInfo ().Coins;
            }
            set {
                Loom.DispatchToMainThread (() => {
                    double coins = value;
//            TopBar.RefreshCoinsWithAnimate (User.Coins, coins);
                    //TopBar.RefreshCoins (coins);
                    GetUserInfo ().Coins = coins;
                    GamePrefs.Add (KEY, GetUserInfo ());
                });
            }
        }

        public static double Experience {
            get {
                return GetUserInfo ().Experience;
            }
            set {
                GetUserInfo ().Experience = value;
                GamePrefs.Add (KEY, GetUserInfo ());
            }
        }

        public static double MaxExperience {
            get {
                return GetUserInfo ().MaxExperience;
            }
            set {
                GetUserInfo ().MaxExperience = value;
                GamePrefs.Add (KEY, GetUserInfo ());
            }
        }

        public static double MinExperience {
            get {
                return GetUserInfo ().MinExperience;
            }
            set {
                GetUserInfo ().MinExperience = value;
                GamePrefs.Add (KEY, GetUserInfo ());
            }
        }

        public static double PreviousStatusPoints {
            get {
                return GetUserInfo ().PreviousStatusPoints;
            }
            set {
                GetUserInfo ().PreviousStatusPoints = value;
                GamePrefs.Add (KEY, GetUserInfo ());
            }
        }

        public static double CurrentStatusPoints {
            get {
                return GetUserInfo ().CurrentStatusPoints;
            }
            set {
                GetUserInfo ().CurrentStatusPoints = value;
                GamePrefs.Add (KEY, GetUserInfo ());
            }
        }

        public static double NextStatusPoints {
            get {
                return GetUserInfo ().NextStatusPoints;
            }
            set {
                GetUserInfo ().NextStatusPoints = value;
                GamePrefs.Add (KEY, GetUserInfo ());
            }
        }

        public static void AddDownloadedSlot (string slotId) {
            GetUserInfo ().DownloadedSlots.Add (slotId);
            GamePrefs.Add (KEY, GetUserInfo ());
        }

        public static void ClearDownloadedSlot () {
            GetUserInfo ().DownloadedSlots.Clear ();
            GetUserInfo ().DownloadedSlots.Add ("fruit");
            GamePrefs.Add (KEY, GetUserInfo ());
        }

        [Serializable]
        private class UserInfo {
            private Int64 _level;
            private double _coins;
            private double _experience;
            private double _maxExperience;
            private double _minExperience;
            private double _previousStatusPoints;
            private double _currentStatusPoints;
            private double _nextStatusPoints;
            private List<string> _downloadedSlots;

            public UserInfo () {
                this._level = 1;
                this._coins = 700;
                this._experience = 0;
                this._maxExperience = 0;
                this._minExperience = 0;
                this._previousStatusPoints = 0;
                this._currentStatusPoints = 0;
                this._nextStatusPoints = 0;

                _downloadedSlots = new List<string> ();
                //Слот загруженный по умолчанию - Клубника
                _downloadedSlots.Add ("fruit");
            }

            public Int64 Level {
                get {
                    return this._level;
                }
                set {
                    this._level = value;
                }
            }

            public double Coins {
                get {
                    return this._coins;
                }
                set {
                    this._coins = value;
                }
            }

            public double Experience {
                get {
                    return this._experience;
                }
                set {
                    this._experience = value;
                }
            }

            public double MaxExperience {
                get {
                    return this._maxExperience;
                }
                set {
                    this._maxExperience = value;
                }
            }

            public double MinExperience {
                get {
                    return this._minExperience;
                }
                set {
                    this._minExperience = value;
                }
            }

            public double PreviousStatusPoints {
                get {
                    return this._previousStatusPoints;
                }
                set {
                    this._previousStatusPoints = value;
                }
            }

            public double CurrentStatusPoints {
                get {
                    return this._currentStatusPoints;
                }
                set {
                    this._currentStatusPoints = value;
                }
            }

            public double NextStatusPoints {
                get {
                    return this._nextStatusPoints;
                }
                set {
                    this._nextStatusPoints = value;
                }
            }

            public List<string> DownloadedSlots {
                get {
                    return this._downloadedSlots;
                }
                set {
                    this._downloadedSlots = value;
                }
            }
        }
    }
}