using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySlot {
    public class CurrentGameState {

        public CurrentGameState () {
        }

        //game id
        public string Game;
        public double Bet;
        public int Lines;
        public double MinBet = 1;

        //Main game
        public bool IsDone = false;
        public bool IsError = false;
        public List<int> ReelSymbols = new List<int> ();
        public List<int> WinLines = new List<int> ();
        public List<int> WinLinesLength = new List<int> ();
        public Dictionary<int, double> LinesScore = new Dictionary<int, double> ();
        public double TotalWinScore;

        //Bonus games
        public string[] BonusKeywords;
        public string CurrentSuperGame;
        public bool StartSuperGame;

        //Double
        public int DealerDoubleCard;
        public int[] DoubleCards;
        public int DoubleSelectedCardIndex;
        public bool IsDoubleWin;
        public bool IsDoubleForward;

        //User data
        public double Balance;
        public Int64 Experience;
        public Int64 Coins;

        public virtual void Clear () {
            IsDone = false;
            IsError = false;
            ReelSymbols.Clear ();
            WinLines = new List<int> ();
            LinesScore.Clear ();
            TotalWinScore = 0;
            IsDoubleForward = false;
            IsDoubleWin = false;
            StartSuperGame = false;
        }
    }
}
