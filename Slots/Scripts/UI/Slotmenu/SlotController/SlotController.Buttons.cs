using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using Core.Server;
using UnitySlot;

public partial class SlotController : MonoBehaviour {
    public GameObject[] bottomButtons;
    public GameObject DoubleButton;
    public GameObject AutoPlayButton;
    public GameObject AutoPlayActiv;
    public GameObject StartButton;
    private bool isButtonSetState = false;
    private bool isWin = false;
    private bool isAutoplay = false;


    void SetButtonState (GameObject button, bool state) { 
        var comp = button.GetComponents<UIButton> ();

        foreach (var c in comp) { 
            //
            // Ничему не удивляемся - так надо :) 
            c.isEnabled = !state;
            c.isEnabled = state; 
        }
    }

    void SetDoubleGameButtonState () { 
        SetAllButtonsState (false);
        SetButtonState (StartButton, true);
    }

    void SetSpinCollider (bool state) { 

        var go = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");
        if (go != null)
            go.transform.parent.gameObject.collider.enabled = state;
    }

    void SetAllButtonsState (bool state) {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag ("SlotButton");
        foreach (GameObject button in buttons) {
            SetButtonState (button, state);
        }

        SetButtonState (DoubleButton, false);
        SetButtonState (AutoPlayButton, state || isAutoplay);

        SetSpinCollider (state);
    }

    void SetWinButtonState () {
        SetAllButtonsState (true);
        for (int i = 0; i < bottomButtons.Length; i++) {
            if (bottomButtons [i] != null) {
                SetButtonState (bottomButtons [i], false);
            }
        }

        SetButtonState (DoubleButton, true);
    }

    void SetActiveMainGameContainer (bool active = true) {
        SetSpinCollider (active);
        mainGameContainer.SetActive (active);
        if (active) {
            doubleGameContainer.SetActive (!active);
            bonusGameContainer.SetActive (!active);
            infoGameContainer.SetActive (!active);
        }
    }

    void SetActiveDoubleGameContainer () {
        InitDouble ();

        mainGameContainer.SetActive (false);
        doubleGameContainer.SetActive (true);
        bonusGameContainer.SetActive (false);
        infoGameContainer.SetActive (false);
    }

    public void SetActiveInfoGameContainer () { 
        if (infoGameContainer.activeSelf) {
            SetActiveMainGameContainer ();
        } else { 
            infoGameContainer.SetActive (true);
            SetActiveMainGameContainer (false);
        }
    }

    void SetActiveBonusGameContainer () { 
        SetSpinCollider (false);
        AutoPlayActiv.SetActive (false);
        gameState = SlotGameState.Bonus;
        mainGameContainer.SetActive (false);
        doubleGameContainer.SetActive (false);
        bonusGameContainer.SetActive (true);
        infoGameContainer.SetActive (false);

        gameObject.BroadcastMessage ("BonusSpinMessage", SendMessageOptions.DontRequireReceiver);
    }

    void SetAutoplayButtonStateMessage () {
        AutoPlayButton.SetActive (SessionData.Instance.IsAutoplay);
        isAutoplay = false;
        AutoPlayActiv.SetActive (false);

        SetButtonState (AutoPlayButton, SessionData.Instance.IsAutoplay);
    }
}