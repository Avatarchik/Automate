using Core.Server;
using Core.Server.Handlers;
using Core.Server.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnitySlot;

public partial class SlotController : MonoBehaviour 
{
    protected int maxLines = 1;
    protected int minLines = 1;
    protected int linesStep = 0;
    protected int minBet = 0;
    protected int maxBet = 1;
    protected int CurrentBetIndex = 1;

    protected List<double> CurrentBets;
    protected static List<double> RealBets = new List<double>
    {
        0.05d, 0.1d, 0.15d, 0.20d, 0.25d, 0.3d, 0.35d, 0.40d, 0.45d, 0.50d, 0.75d, 1.00d, 1.25d, 2.50d, 5.00d, 10.00d
    };

    private void SetBets (List<double> bets, int defaultBets) 
    {
        CurrentBets = bets;
        CurrentBetIndex = defaultBets;
        maxBet = CurrentBets.Count () - 1;
    }

    IEnumerator GoToBonus () {
        slotLinesManager.ResetState ();
        slotLinesManager.HideAllLines ();
        yield return new WaitForSeconds (0.1f);
      
        //BroadcastMessage ("BonusMessage", SendMessageOptions.DontRequireReceiver);
        BroadcastMessage("BonusMessage", new List<double>(),SendMessageOptions.DontRequireReceiver);
       
        slotReelsManager.AnimateBonusCells (true);

        yield return new WaitForSeconds (3f);

        SetActiveBonusGameContainer ();
        slotReelsManager.AnimateBonusCells (false);
    }


    public void SpinCompleted () {
        if (GameState.CurrentGame.StartSuperGame) {
            StartCoroutine (GoToBonus ());
        }
    }

    void Spin () {
        SoundManager.Instance.Play ("main:spin:loop");
        BroadcastMessage ("SpinStartMessage", SendMessageOptions.DontRequireReceiver);

        ServerHelper.Spin ((response) => {
            string popupName = null;

            //If user get new level
            if (response.Bets != null) {
                SessionData.Instance.NextLevelBonus = response.NextLvBonus;
                SessionData.Instance.FunBets = response.Bets;
                SetBets (response.Bets, CurrentBetIndex);
                popupName = Constants.NewLevelPopup;
                if (response.NextSlot != null) {
                    popupName = Constants.NewLevelAndSlotPopup;
                }
            }

            //If user get new status
            if (User.CurrentStatus != response.Status) {
                popupName = Constants.NewStatusPopup;
            }

            if (GameState.CurrentGame.StartSuperGame) {
                isAutoplay = false;
            }

            if (popupName != null) {
                // если лезет какой то попап - надо остановить барабаны
                if (isAutoplay) 
                    SetAutoplayButtonStateMessage();
            }

            PopupManager.Open(popupName);

        }, () => {
            if (isAutoplay) 
                SetAutoplayButtonStateMessage();

            SetAllButtonsState(true);
        });


        SetAllButtonsState (false);
        slotReelsManager.Spin ();
    }

}