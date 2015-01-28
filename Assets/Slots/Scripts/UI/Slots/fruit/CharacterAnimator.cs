using UnityEngine;
using System.Collections;
using System;

namespace UnitySlot {

    public abstract class CharacterAnimator : MonoBehaviour {
        public abstract void RiskWinAnimate ();

        public abstract void RiskLoseAnimate ();

        public abstract void ResetState ();

        public abstract void RiskWinForward ();
    }

}
