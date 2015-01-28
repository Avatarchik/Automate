using UnityEngine;
using System.Collections;
using UnitySlot;

public class RCBSectorsController : MonoBehaviour {

    public SlotAnimationPlayer[] sectors;

    RCBonusReelsController reelController;

    bool _animate;

    int currentSector = 0;

    public bool ShowWin = false;

    public float speedDelay = 0.1f;

    public bool Stopped = false;

    void MoveToNextPos () {
        sectors [currentSector].Animate = false;

        currentSector++;

        if (currentSector == sectors.Length)
            currentSector = 0;

        sectors [currentSector].Animate = true;
    }

    int getTargetDistance () {
        if (currentSector == GameState.FreeGame.NewPosition)
            return 0;

        if (currentSector < GameState.FreeGame.NewPosition)
            return GameState.FreeGame.NewPosition - currentSector;

        return (sectors.Length - currentSector) + GameState.FreeGame.NewPosition;
    }

    void AnimateWinning () {
        if (GameState.FreeGame.IsWin) { 

            var fCBSectorInfo = sectors [GameState.FreeGame.NewPosition].gameObject.GetComponent<FCBSectorInfo> ();

            for (int i = 0; i < 3; i++) { 
                if (GameState.FreeGame.MidSymbols [i] == fCBSectorInfo.symbolCode) { 
                    reelController.winBoxes [i].enabled = true;
                }
            }

        }
    }

    IEnumerator AnimateCoroutine () {

        Stopped = false;

        //
        // Крутить пока не прийдет ответ от сервера
        while (_animate) { 

            MoveToNextPos ();

            yield return new WaitForSeconds (speedDelay);
        }

        //
        // Покрутить еще немнго после прихода ответа (для того чтобы при быстром ответе все равно немного покрутилось)
        var i = Random.Range (12, 23);

        for (int k = 0; k < i; k++) { 
            MoveToNextPos ();

            yield return new WaitForSeconds (speedDelay);
        }

        //
        // Докрутить если дистанция меньше 10 секторов
        if (getTargetDistance () < 10) { 

            for (int k = 0; k < 18; k++) { 
                MoveToNextPos ();

                yield return new WaitForSeconds (speedDelay);
            }

        }

        // Расстояние за которое начнется замедление кручения
        i = Random.Range (7, 12);

        while (currentSector != GameState.FreeGame.NewPosition) { 
            MoveToNextPos ();

            var dist = getTargetDistance ();

            if (dist != 0) {
                if (dist > i)
                    yield return new WaitForSeconds (speedDelay);
                else {
                    yield return new WaitForSeconds (0.2f);
                }

            } else
                yield return new WaitForSeconds (0.5f);
        }

        // Крутилка остановлена
        Stopped = true;

        // Ждем пока не поступит разрешение на отображение выигрыша
        while (!ShowWin)
            yield return new WaitForEndOfFrame ();

        ShowWin = false;

        AnimateWinning ();

        GameState.FreeGame.IsDone = false;

    }

    IEnumerator Init () {

        yield return new WaitForSeconds (0.1f);

        sectors [0].Animate = true;

    }

    // Use this for initialization
    void Start () {

        StartCoroutine (Init ());
    }

    public void StartSpin (RCBonusReelsController reel) {
        if (reelController == null)
            reelController = reel;

        _animate = true;
        StartCoroutine (AnimateCoroutine ());
    }

    public void EndSpin () {
        _animate = false;
    }
}
