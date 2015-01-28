using UnityEngine;
using System.Collections;
using UnitySlot;

public class FCBonusGameController : MonoBehaviour {

    public FCBonusReelsController reels;
    public FCBLivesController lives;
    public FCBSectorsController sectors;
   
    SlotController _sc;

    SlotMenuInfoBar infoBar;

    SlotController slotController {
        get { 
            if (_sc == null) { 
                var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

                if (g != null)
                    _sc = g.GetComponent<SlotController> ();
            }

            return _sc;
        }
    }

    void Start () {
    
        var go = GameObject.FindGameObjectWithTag ("InfoBarSlotMenu");
        if (go != null) // Чисто для отладки в реальной игре такой объект 100 пудово есть
            infoBar = go.GetComponent<SlotMenuInfoBar> ();
    }

    IEnumerator BonusSpinAnimation () {
        reels.StartSpin ();
        sectors.StartSpin (reels);

        slotController.FreeSpin ();

        yield return new WaitForSeconds (0.2f);

        while (!GameState.FreeGame.IsDone) { 
            yield return new WaitForFixedUpdate ();
        }

        reels.EndSpin ();

        yield return new WaitForSeconds (1f);

        sectors.EndSpin ();

        yield return new WaitForSeconds (2f);

        lives.lives = GameState.FreeGame.Lives;

        sectors.ShowWin = true;

        while (!sectors.Stopped)
            yield return new WaitForEndOfFrame ();

        UpdateInfoBar ();

        yield return new WaitForSeconds (2.5f);

        if (GameState.FreeGame.Lives > 0)
            BonusSpinMessage ();
        else
            EndBonusGame ();

        // TODO run next spin if user have enough lives 

    }

    void EndBonusGame () {
        slotController.BonusGameEnded ();
    }

    void UpdateInfoBar () {
        infoBar.Win = (float)GameState.FreeGame.Score;
        // TODO показать текущий выигрыш в строке статуса
        Debug.Log ("Add info bar update method implementation!");
    }

    public void BonusSpinMessage () { 

        StartCoroutine (BonusSpinAnimation ());
       
    }

}
