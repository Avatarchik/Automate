using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using Core.Server;
using UnitySlot;

public partial class SlotController : MonoBehaviour {

    public int RiskStep;

    protected void InitDouble() { 
        gameState = SlotGameState.Risk;
        isWin = false;
        SetDoubleGameButtonState ();

        var rc = doubleGameContainer.GetComponent<RiskController> ();
        rc.InitScene ();
	
		SetSpinCollider (false);
    }

    public void RiskWait (bool wait) {
        if (wait)
            SetAllButtonsState (false);
        else
            SetDoubleGameButtonState ();
    }

    public void RiskGameEnded()  { 
        SetActiveMainGameContainer ();
        gameState = SlotGameState.Main;
        slotLinesManager.ResetState ();
        SetAllButtonsState (true);
    }
   
}