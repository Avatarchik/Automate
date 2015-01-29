using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySlot;

namespace Core.Server.Response {
    public class SpinResponse : BaseResponse {

        public List<int> Symbols {
            get;
            set;
        }

        public List<int> WinLines {
            get;
            set;
        }

        public List<int> WinLinesLength {
            get;
            set;
        }

        public List<double> Scores {
            get;
            set;
        }

        public int CardForDouble {
            get;
            set;
        }

        public int Freegame {
            get;
            set;
        }

        public List<double> Bets {
            get;
            set;
        }

        public int NextLvBonus {
            get;
            set;
        }

        public String NextSlot {
            get;
            set;
        }

        //Real
        public bool ScatterWin {
            get;
            set;
        }

        public double DemoBonusPropgress {
            get;
            set;
        }

        public bool IsBonusReceived {
            get;
            set;
        }

        public StatusType Status {
            get;
            set;
        }


        public SpinResponse (JObject o) : base(o) {
            var spinData = SessionData.Instance.IsFun ? JsonUtil.GetJObject (o, "spinData") : o;
            Symbols = JsonUtil.JarrayToList<int> (spinData, "symbols");
            WinLines = JsonUtil.JarrayToList<int> (spinData, "winLines");
            WinLinesLength = JsonUtil.JarrayToList<int> (spinData, "winLinesLength");
            Scores = JsonUtil.JarrayToList<double> (spinData, "scores");
            CardForDouble = (int)JsonUtil.GetInt (o, "cardForDouble", - 1);
            Freegame = (int)JsonUtil.GetInt (o, "freeGame");

            if (JsonUtil.GetJObject (o, "nextLvData") != null) {
                var nextLvData = JsonUtil.GetJObject (o, "nextLvData");
                Bets = JsonUtil.JarrayToList<double> (nextLvData, "bets");
                NextLvBonus = (int)JsonUtil.GetInt (nextLvData, "nextLvBonus");
                if (JsonUtil.GetString(nextLvData, "nextSlot") != null) {
                    NextSlot = JsonUtil.GetString(nextLvData, "nextSlot");
                }
            }

            if (JsonUtil.GetJObject(o, "user") != null) {
                var user = JsonUtil.GetJObject(o, "user");
                Level = JsonUtil.GetInt(user, "level");

                if (JsonUtil.GetJObject(user, "expData") != null) {
                    JObject expData = JsonUtil.GetJObject(user, "expData");
                    Exp = (Int64)JsonUtil.GetFloat(expData, "exp");
                    MinExperience = (Int64)JsonUtil.GetFloat(expData, "curLvExp");
                    MaxExperience = (Int64)JsonUtil.GetFloat(expData, "nexLvExp");
                }

                Balance = (Int64)JsonUtil.GetFloat(user, "balance");
                var statusData = JsonUtil.GetJObject(user, "statusData");
                Points = (Int64)JsonUtil.GetFloat(statusData, "points");
            }

            //parse bonus game
            GameState.CurrentGame.StartSuperGame = false;
            GameState.CurrentGame.CurrentSuperGame = null;
            var bonusKeywords = GameState.CurrentGame.BonusKeywords;
            if (bonusKeywords != null && bonusKeywords.Length > 0) {
                foreach (string keyword in bonusKeywords) {
                    if (o[keyword] != null && (int)JsonUtil.GetInt(o, keyword) == 1) {
                        GameState.CurrentGame.StartSuperGame = true;
                        GameState.CurrentGame.CurrentSuperGame = keyword;
                        break;
                    }
                }
            }

            //real
            DemoBonusPropgress = JsonUtil.GetDouble (o, "vega_mobi_fun_progress", 0);
            IsBonusReceived = JsonUtil.GetInt (o, "bonus_ready", 0) == 1;
            ScatterWin = JsonUtil.GetInt (o, "scatter_win", 0) == 1;

        }
    }
}
