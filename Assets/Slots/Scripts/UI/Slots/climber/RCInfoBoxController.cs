using UnityEngine;
using System.Collections;
using System;
using UnitySlot;

public class RCInfoBoxController : MonoBehaviour {

    public SpriteRenderer sprite;
    public GameObject WinLineInfo;
    public GameObject RiskText;

    public UILabel LineNumberLabel;
    public UILabel LineScoreLabel;

    public bool isDouble;

    public GameObject DoubleToText;
    public UILabel DoubleToValue;

    // Use this for initialization
    void Start () {
        ResetState ();
        ShowSprite ();
    }

    void ShowTakeOrWin () {
        ResetState ();
        RiskText.SetActive (true);
    }

    void ShowSprite () {
        ResetState ();

        if (isDouble)
            return;
            
        sprite.enabled = true;
    }

    public void DoubleNextStepMessage () 
    {
        if (isDouble)
            ShowDouble ((float)GameState.CurrentGame.TotalWinScore * 2);
    }


    public void DoubleMessage () { 

        ShowSprite ();
    }

    public void ShowDouble (float value)
    { 
        ResetState ();
        DoubleToValue.text = value.ToString ();
        DoubleToText.SetActive (true);
        DoubleToValue.enabled = true;
    }

    void ResetState () { 
        if (!isDouble)
        {
            RiskText.SetActive (false);

            WinLineInfo.SetActive (false);
            sprite.enabled = false;
        } else { 
            DoubleToText.SetActive (false);
            DoubleToValue.enabled = false;
        }
    }

    ExecuteOnTime timer;

    public void WinMessage (WMWinInfo info) {
        ResetState ();
//        if (info.Risk) { 
//            ShowDouble (info.amount * 2);
//        } else { 
            WinLineInfo.SetActive (true);
            LineNumberLabel.text = info.line.ToString ();
            LineScoreLabel.text = info.amount.ToString ();
          
            if (timer != null)
                timer.Stop ();

            timer = gameObject.AddDelayedActionToObject (new TimeSpan (0, 0, 1), (s, e) => ShowTakeOrWin ());
//        }
    }

    public void SpinMessage () {
        ShowSprite ();
    }

}


