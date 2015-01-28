using UnityEngine;
using System.Collections;
using UnitySlot;

public class RCBonusGameController : MonoBehaviour
{

    public RCBonusReelsController reels;    
   
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

    public void EndBonusGame () 
    {
        slotController.BonusGameEnded ();
    }

    void UpdateInfoBar () {
        infoBar.Win = (float)GameState.FreeGame.Score;
        // TODO показать текущий выигрыш в строке статуса
        Debug.Log ("Add info bar update method implementation!");
    }

   

}
