using Core.Server.Handlers;
using Core.Server.Request;
using System;
using System.Collections;
using UnityEngine;
using UnitySlot;

public class RiskController : MonoBehaviour 
{
    public CardController[] Cards;
    public RiskPickersController pickerController;
    public CharacterAnimator animator;

    public GameObject mainGameContainer;
    public GameObject doubleGameContainer;
    private bool canOpenCard = false;

    double lastScore;

    SlotController _sc;

    SlotController slotController 
    {
        get { 
            if (_sc == null)
            { 
                var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

                if (g != null)              
                    _sc = g.GetComponent<SlotController>();
               
            }

            return _sc;
        }
    }

    public void InitScene () 
    { 
        animator.ResetState ();
        pickerController.ResetState ();
        ResetState ();

        lastScore = GameState.CurrentGame.TotalWinScore;

        Cards [0].card = CardDescriptor.FromIndex (GameState.CurrentGame.DealerDoubleCard);
        Cards [0].openCard = true;
        canOpenCard = true;
    }

    void OnEnable () 
    {
        if (slotController != null)
            slotController.gameObject.BroadcastMessage ("DoubleNextStepMessage", SendMessageOptions.DontRequireReceiver);
    }

    IEnumerator OpenCards () { 
        var cardnum = GameState.CurrentGame.DoubleSelectedCardIndex;
        Cards [cardnum].card = CardDescriptor.FromIndex (GameState.CurrentGame.DoubleCards [cardnum]);
        Cards [cardnum].openCard = true;

        yield return new WaitForSeconds (0.7f);

        for (int i = 1; i < GameState.CurrentGame.DoubleCards.Length; i++) {

            var card = GameState.CurrentGame.DoubleCards [i];
            Cards [i].card = CardDescriptor.FromIndex (card);
            Cards [i].openCard = true;
        }

        yield return new WaitForSeconds (2);

        ResetState ();
        pickerController.ResetState ();
        if (!GameState.CurrentGame.IsDoubleWin) {
            GameState.CurrentGame.Clear ();
            ReturnToMainGame ();
        } else {

            yield return new WaitForSeconds (0.15f);

            slotController.RiskStep++;

            Cards [0].card = CardDescriptor.FromIndex (GameState.CurrentGame.DealerDoubleCard);
            Cards [0].openCard = true;

            slotController.gameObject.BroadcastMessage ("DoubleNextStepMessage", SendMessageOptions.DontRequireReceiver);

            yield return new WaitForSeconds (0.5f);

            slotController.RiskWait (false);
        }

    }

    void UpdateInfoBar () {
        var infoBar = GameObject.FindGameObjectWithTag ("InfoBarSlotMenu");
        infoBar.GetComponent<SlotMenuInfoBar> ().Win = (float)GameState.CurrentGame.TotalWinScore;
    }

    void makeRequest (int cardnum) {


        if (slotController.doubleType == DoubleGameType.FiveCard) { 
            canOpenCard = false;
            ServerHelper.DoubleFiveCardsGameRequest (cardnum, () => Loom.DispatchToMainThread (() => {
                if (GameState.CurrentGame.IsDoubleWin) {
                    UpdateInfoBar ();

                    if (GameState.CurrentGame.IsDoubleForward)
                        animator.RiskWinForward ();
                    else
                        animator.RiskWinAnimate ();
                } else { 
                    animator.RiskLoseAnimate ();
                }

                slotController.RiskWait (true);
                
                StartCoroutine (OpenCards ()); 
            }));
        }
    }

    void ReturnToMainGame () 
    {
        Debug.LogError(" Line For Delete! ");
        /////////////////////////////////////////////////////////
        slotController.RiskStep = 0;
        /////////////////////////////////////////////////////////
        ResetState ();
       
        slotController.RiskGameEnded ();
    }

    public void FirstCardTapped () { 
        if (canOpenCard) {
            pickerController.Activate1 ();
            makeRequest (1);
        }
    }

    public void SecondCardTapped () { 
        if (canOpenCard) {
            pickerController.Activate2 ();
            makeRequest (2);
        }
    }

    public void ThirdCardTapped () { 
        if (canOpenCard) {
            pickerController.Activate3 ();
            makeRequest (3);
        }
    }

    public void FouthCardTapped () { 
        if (canOpenCard) {
            pickerController.Activate4 ();
            makeRequest (4);
        }
    }

    public void ResetState () {
        foreach (var c in Cards) {
            c.openCard = false;
        }

        canOpenCard = true;
    }

}
