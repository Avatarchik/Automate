using System.Collections.Generic;

namespace UnitySlot {
    public class FreeGameState : CurrentGameState {

        public FreeGameState () {
        }

        public int Lives;
        public double Score;

        public bool IsWin {
            get {
                return Score > 0;
            }
        }

        //Params for Fruit slot
        public List<int> MidSymbols = new List<int> ();
        public int NewPosition;

        //
        public int TotalScatterGame;
        public int CurrentScatterGame;
        public double ScatterScore;

        public virtual void Clear () {
            Lives = 0;
            Score = 0;
            MidSymbols.Clear ();
            NewPosition = 0;
        }

        public virtual void Reset () {
            Clear ();
            base.Clear ();

            TotalScatterGame = 0;
            CurrentScatterGame = 0;
            ScatterScore = 0;
        }
    }
}
