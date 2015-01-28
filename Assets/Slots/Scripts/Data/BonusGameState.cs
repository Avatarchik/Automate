using System.Collections.Generic;

namespace UnitySlot {
    public class BonusGameState : FreeGameState {

        public BonusGameState () {
        }

        //request param
        public bool UseGarageSuperKey;

        //response param
        public int Coef;
        public int Item;
        public int Dice;
        public bool StartDoorGame;

        public virtual void Clear () {
            Coef = 0;
            Item = 0;
            Dice = 0;
            StartDoorGame = false;
            UseGarageSuperKey = false;
        }

        public virtual void Reset () {
            Clear ();
            base.Clear ();
        }
    }
}
