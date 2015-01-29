using Core.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnitySlot;

public partial class SlotController : MonoBehaviour {

  

    public void FreeSpin () {
//    var bet = CurrentBets [CurrentBetIndex];
        ServerHelper.FreeSpin ();

        //        SetAllButtonsState (false);
        //        slotReelsManager.Spin ();
    }


    public void BonusGameEnded () {
        SetActiveMainGameContainer ();
        gameState = SlotGameState.Main;
        slotLinesManager.ResetState ();

        if (GameState.FreeGame.TotalWinScore > 0) { 
            var bar = infoBar.GetComponent<SlotMenuInfoBar> ();
            bar.Win = (float)GameState.FreeGame.TotalWinScore;
            GameState.CurrentGame.TotalWinScore = GameState.FreeGame.TotalWinScore;
            SetWinButtonState ();
            isWin = true;
        } else
            SetAllButtonsState (true);
    }

}