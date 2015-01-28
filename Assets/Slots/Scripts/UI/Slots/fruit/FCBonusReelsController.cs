using UnityEngine;
using System.Collections;
using UnitySlot;
using System.Collections.Generic;

public class FCBonusReelsController : MonoBehaviour {
    enum ReelState {
        stopped,
        spinning,
        stopping,
    }

    public Sprite[] symbols;

    bool _spinning;

    SpriteRenderer[] slots;
    public SpriteRenderer[] winBoxes;

    public float speed;

    public bool stop;
    public GameObject SlotsParent;

    ReelState[] reelSpinning = new ReelState[3];
    SpriteRenderer[] topSlots = new SpriteRenderer[3];

    public bool IsSpinning { 
        get { return _spinning; }
    }

    // Use this for initialization
    void Start () {
		Debug.LogError (gameObject.name);
        slots = SlotsParent.GetComponentsInChildren<SpriteRenderer> ();

    }

    public void EndSpin () { 
        _spinning = false;
    }
	
    // Update is called once per frame
    void Update () {

//        if (stop) {
//            GameState.CurrentGame.ReelSymbols = new List<int> { 3, 2, 1 };
//            _spinning = false;
//        }
	
        SpinReel (0);
        SpinReel (1);
        SpinReel (2);
    }

    void StopReelAt (int num, int symbol) {
        // Символы с сервера индексируются начиная с 1
        topSlots [num].sprite = symbols [symbol - 1];

        reelSpinning [num] = ReelState.stopping;
    }

    void SpinReel (int num) {
        if (reelSpinning [num] == ReelState.stopped)
            return;

        for (int i = num; i < 9; i += 3) {
            var s = slots [i];

            var f = -speed;
            if (reelSpinning [num] == ReelState.stopping)
                f /= 5;

            s.transform.localPosition += new Vector3 (0, f, 0);

            if (reelSpinning [num] == ReelState.stopping && s == topSlots [num]) { 

                if (Mathf.Abs (s.transform.localPosition.y) < speed / 10) {
                    var v = new Vector3 (s.transform.localPosition.x, -1, s.transform.localPosition.z);

                    for (int k = num; k < 9; k += 3) {
                        var x = slots [k];
                        if (s != x) {
                            x.transform.localPosition = v;
                            v.y = 1;
                        }
                    }

                    v.y = 0;
                    s.transform.localPosition = v;

                    reelSpinning [num] = ReelState.stopped;

                    return;
                }
            }

            if (s.transform.localPosition.y < -1.15 && reelSpinning [num] == ReelState.spinning) { 
                s.transform.localPosition += new Vector3 (0, 3f, 0);
                s.sprite = symbols [Random.Range (0, symbols.Length - 1)];

                topSlots [num] = s;
            }
        }
    }

    IEnumerator SpinCoroutine () {
        _spinning = true;

        Debug.Log ("speen");

        var stopReel = 0;

        // TODO помеять на проверку IsDone в классе CurrentGameState
        while (_spinning) {

            yield return new WaitForFixedUpdate ();
        }

        StopReelAt (0, GameState.FreeGame.MidSymbols [0]);
        yield return new WaitForSeconds (0.5f);
        StopReelAt (1, GameState.FreeGame.MidSymbols [1]);
        yield return new WaitForSeconds (0.5f);
        StopReelAt (2, GameState.FreeGame.MidSymbols [2]);
        yield return new WaitForSeconds (0.5f);

        _spinning = false;
    }

    void HideWinBoxes () {
        foreach (var w in winBoxes) {
            w.enabled = false;
        }
    }

    public void StartSpin () { 
        if (_spinning)
            return;

        HideWinBoxes ();

        reelSpinning = new []{ ReelState.spinning, ReelState.spinning, ReelState.spinning };
        StartCoroutine (SpinCoroutine ());
    }
}
