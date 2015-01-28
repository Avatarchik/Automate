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

public partial class SlotController : MonoBehaviour {
    private GameObject mainGameContainer;
    private GameObject doubleGameContainer;
    private GameObject bonusGameContainer;
    private GameObject infoGameContainer;
    private GameObject slotContainer;
    private SlotLinesManager slotLinesManager;
    private SlotReelsManager slotReelsManager;
    private string SlotId;
    private SlotGameState gameState = SlotGameState.Main;

    public DoubleGameType doubleType {
        get { 
            var si = slotContainer.GetComponent<CurrentSlotInfo> ();
            return si.DoubleGameType;
        }
    }

    GameObject infoBar;

    void Awake () {
        SlotId = GamePrefs.GetString (Constants.SettingsSelectedSlot);
        InitSlot ();
    }

    void Start () {
        SetAutoplayButtonStateMessage ();
        slotLinesManager = GetComponent<SlotLinesManager> ();
        slotReelsManager = GetComponent<SlotReelsManager> ();
        mainGameContainer = GameObject.FindGameObjectWithTag ("MainGameContainer");
        doubleGameContainer = GameObject.FindGameObjectWithTag ("DoubleGameContainer");
        bonusGameContainer = GameObject.FindGameObjectWithTag ("BonusGameContainer");
        infoGameContainer = GameObject.FindGameObjectWithTag ("InfoGameContainer");
        infoBar = GameObject.FindGameObjectWithTag ("InfoBarSlotMenu");
        SetActiveMainGameContainer ();

        slotContainer = GameObject.FindGameObjectWithTag ("SlotContainer");
        var si = slotContainer.GetComponent<CurrentSlotInfo> ();
        maxLines = si.maxLine;
        minLines = si.minLines;
        linesStep = si.LineStep;
        GameState.CurrentGame.Game = SlotId;
        var bonusGameKeyword = SessionData.Instance.IsFun ? si.FunBonusGameKeyword : si.BonusGameKeyword;
        GameState.CurrentGame.BonusKeywords = !si.IgnoreBonusGame ? bonusGameKeyword : null;
        UpdateInfoBar ();
    }

    void InitSlot () {
        if (!SessionData.Instance.IsFun) {
            //generate session key fro slot
            SessionData.Instance.StartSession ();
            var handler = new StateHandler (new StateRequest (SlotId)).AddErrorListener ((exception) => {
                if (exception.GetType () == typeof(ParseResponseException)) {
                    LogUtil.D (typeof(SlotController), string.Format ("Failed init slot state [{0}]", exception.Message));
                    ParseResponseException pre = (ParseResponseException)exception;
                    if ("THERE_IS_NO_SESSION_KEY".Equals (pre.GetSysName ())) {
                        //TODO выкидывать наверное из игры, надо логику на текущем клиенте курить
                        return;
                    }
                }
            });
            handler.DoRequest ();
        }
    }

    void UpdateInfoBar () {
        if (infoBar != null) {
            if (CurrentBets == null || CurrentBets.Count == 0) {
                SetBets (SessionData.Instance.IsFun ? SessionData.Instance.FunBets : RealBets,
                SessionData.Instance.IsFun ? SessionData.Instance.FunBets.IndexOf (SessionData.Instance.DefaultBet) : 0); //Для реала пока захардкодил
            }

            var bar = infoBar.GetComponent<SlotMenuInfoBar> ();
            bar.Lines = GameState.CurrentGame.Lines != 0 ? GameState.CurrentGame.Lines : maxLines;
            bar.Bet = (float)CurrentBets [CurrentBetIndex];
            bar.BetSum = (float)CurrentBets [CurrentBetIndex] * GameState.CurrentGame.Lines;
        }
    }

    void Update () {
        UpdateInfoBar ();

        if (GameState.CurrentGame.IsDone && slotLinesManager.isEndAnimationLines () && !isButtonSetState && !isAutoplay) {
            isButtonSetState = true;

            if (GameState.CurrentGame.StartSuperGame) {

                isAutoplay = false;

                SetAllButtonsState (false);
            } else {

                if (GameState.CurrentGame.WinLines != null && GameState.CurrentGame.WinLines.Count > 0) {
                    isWin = true;
                    SetWinButtonState ();
                } else {
                    SetAllButtonsState (true);
                }
            }

        }

        if (GameState.CurrentGame.IsDone && slotLinesManager.isEndAnimationLines () && isAutoplay) {
            SpinMessage ();
        }
    }

#region  Messages

    //Messages
    void SpinMessage () {

        SoundManager.Instance.Play ("main:button:start");

        Debug.Log ("SlotController Spin Message > " + gameState);

        if (gameState == SlotGameState.Risk) {
            RiskGameEnded ();
        } else if (gameState == SlotGameState.Main) {
            GameState.CurrentGame.Clear ();
            isButtonSetState = false;
            SetAllButtonsState (true);
            slotLinesManager.PreparetoSpin ();

            //
            // не понятнно нужен ли он тут ?
            SetActiveMainGameContainer ();
            if (!isWin) {
                Spin ();
            } else {
                isWin = false;
            }
        }
    }

    void LinesMessage (int val) {
        if (slotLinesManager.isCurrentLinesShowed ()) {
            GameState.CurrentGame.Lines += linesStep * val;
            if (GameState.CurrentGame.Lines > maxLines) {
                GameState.CurrentGame.Lines = maxLines;
            }
            if (GameState.CurrentGame.Lines < minLines) {
                GameState.CurrentGame.Lines = minLines;
            }
        }
        slotLinesManager.SetCurrentLines (GameState.CurrentGame.Lines);
        slotLinesManager.ShowCurrentIndicators ();
        slotLinesManager.ShowCurrentLines ();

        UpdateInfoBar ();
    }

    void BetMessage (int val) {

        CurrentBetIndex += val;
        if (CurrentBetIndex > maxBet) {
            CurrentBetIndex = maxBet;
        }
        if (CurrentBetIndex < minBet) {
            CurrentBetIndex = minBet;
        }

        UpdateInfoBar ();
    }

    void MaxBetMessage () {

        SoundManager.Instance.Play ("main:button:maxbet");

        GameState.CurrentGame.Lines = maxLines;
        CurrentBetIndex = maxBet;
        slotLinesManager.SetCurrentLines (GameState.CurrentGame.Lines);
        slotLinesManager.ShowCurrentIndicators ();
        SpinMessage ();
    }

    void DoubleMessage () {
        if (isWin) {
            //
            // Нужно для того чтобы все сообщения успели доставиться до всех абонентов. Иначе дабл дизаблит главный экран и сообщения до 
            // объектов внутри него не доходят.
            gameObject.AddDelayedActionToObject (new TimeSpan (100), (s, e) => {
                SetActiveDoubleGameContainer ();
                //                isWin = false;
            });
        }
    }

    void AutoplayMessage () {
        isAutoplay = !isAutoplay;
        if (isAutoplay) {
            AutoPlayActiv.SetActive (true);
            isWin = false;
            SpinMessage ();
        } else {
            AutoPlayActiv.SetActive (false);
        }
    }

#endregion

}


