using UnityEngine;
using System.Collections;
using System;
using UnitySlot;

public class FruitAnimator : CharacterAnimator {

    DateTime lastRoll;
    Animator anim;

	// Use this for initialization
	void Start () {
        lastRoll = DateTime.Now;
        anim = GetComponent<Animator> ();
	}
	
    public void WinMessage() {
        anim.SetTrigger ("Winner");
    } 

	// Update is called once per frame
	void Update () {
        var t = DateTime.Now - lastRoll;

        if (t.TotalSeconds >= 7) {
            anim.SetTrigger ("EyeRoll");
            lastRoll = DateTime.Now;
            anim.SetFloat ("RollDirection", UnityEngine.Random.value);
        }
	}

    public override void RiskWinAnimate() { 
        anim.SetTrigger ("Win");
        anim.SetTrigger ("Winner");
    }

    public override void RiskLoseAnimate() { 
        anim.SetTrigger ("Loose");
        anim.SetTrigger ("LooseBase");
    }

    public override void ResetState() { 
        anim.SetTrigger ("Reset");
    }

    public override void RiskWinForward() { 
        anim.SetTrigger ("Forward");
        anim.SetTrigger ("Winner");
    }
}
