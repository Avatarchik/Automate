using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnitySlot;

public class SlotLinesManager : MonoBehaviour {
    private SlotReelsManager slotReelsManager;
    private GameObject slotContainer;
    private Transform linesParent;
    private Sprite[] lines;
    private Sprite[] indicators;
    private List<GameObject> linesList = new List<GameObject> ();
    private List<GameObject> indicatorsList = new List<GameObject> ();
    private float indicatorsDelay = 5.5f;
    private int currentLine = 0;
    private bool isLinesShowed = false;
    private bool isWinnigLinesShowed = false;
    private bool isBlinkEnd = false;
    private bool isBlinkIndicators;
    private int numBlinkToEnd = 1;
    private int numBlink = 0;
    private float blinkDelay;
    private float lineDelay;
    private float nextBlinkTime = 0;
    private float nextLineTime = 0;
    private int currentWinLineKey = 0;
    bool _notified;

    bool stopAnimation;
    

    // Use this for initialization
    void Start () {
        SetStartValue ();
        CreateLines ();
        CreateIndicators ();
    }

    void NotifySpinCompleted () {
        if (_notified)
            return;

        var c = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer").GetComponent<SlotController> ();
        c.SpinCompleted ();

        _notified = true;
    }

    void StopWinLineAnimation () {
        slotReelsManager.AnimateWinLineCells (false);
    }

    void StartWinLineAnimation () {
        if (GameState.CurrentGame.WinLines != null && GameState.CurrentGame.WinLines.Count > 0) {
            StopWinLineAnimation ();
            slotReelsManager.AnimateWinLineCells (true, currentWinLineKey - 1);
        }
    }
    
    // Update is called once per frame
    void Update () {
        if (stopAnimation)
            return;

        if (slotReelsManager.isEndAnimateReels ()) {
            if (!isWinnigLinesShowed && Time.time > nextLineTime) {
                nextLineTime = Time.time + lineDelay;
                ShowWinningLine ();
                StartWinLineAnimation ();
            }

            if (isWinnigLinesShowed) { 
                var infoBar = GameObject.FindGameObjectWithTag ("InfoBarSlotMenu");
                infoBar.GetComponent<SlotMenuInfoBar> ().Win = (float)GameState.CurrentGame.TotalWinScore;
                StopWinLineAnimation ();
            }

            if (Time.time > nextBlinkTime && isWinnigLinesShowed) {
                nextBlinkTime = Time.time + blinkDelay;
                BlinkIndicators ();
            }
        } else {
            nextLineTime = Time.time + lineDelay;
            nextBlinkTime = Time.time + blinkDelay;
        }
    }

    void BlinkIndicators () {
        numBlink++;
        if (numBlink > numBlinkToEnd) {
            isBlinkEnd = true;
        }
        if (GameState.CurrentGame.WinLines != null && GameState.CurrentGame.WinLines.Count > 0) {
            for (int i = 0; i < GameState.CurrentGame.WinLines.Count; i++) {
                bool state = !indicatorsList [GameState.CurrentGame.WinLines [i] - 1].activeSelf;
                indicatorsList [GameState.CurrentGame.WinLines [i] - 1].SetActive (state);
            }
        } else {
            isBlinkEnd = true;
        }
    }

    void ShowWinningLine () {
        if (GameState.CurrentGame.WinLines != null && GameState.CurrentGame.WinLines.Count > 0) {
            if (currentWinLineKey < GameState.CurrentGame.WinLines.Count) {
                linesList [GameState.CurrentGame.WinLines [currentWinLineKey] - 1].SetActive (true);

                var line = GameState.CurrentGame.WinLines [currentWinLineKey];
                var score = GameState.CurrentGame.LinesScore [line];

                //
                // TODO Вставить выигрыш по линии вместо константы!
                BroadcastMessage ("WinMessage", new WMWinInfo { line = line, amount = (float)score });

                string snd = string.Format ("main:win:line:{0}", line);
                SoundManager.Instance.Play (snd);

                var infoBar = GameObject.FindGameObjectWithTag ("InfoBarSlotMenu");
                infoBar.GetComponent<SlotMenuInfoBar> ().Win = (float)score;

                currentWinLineKey++;
            } else {
                NotifySpinCompleted ();
                isWinnigLinesShowed = true;
            }
        } else {
            NotifySpinCompleted ();
            isWinnigLinesShowed = true;
        }
    }

    public void SetCurrentLines (int curLines) {
        currentLine = curLines - 1;
    }

    public void ResetState () { 
        HideAllLines ();
        isWinnigLinesShowed = true;
        isBlinkEnd = true;
        stopAnimation = true;
    }

    public void HideAllLines () {
        isLinesShowed = false;
        isWinnigLinesShowed = false;
        for (int i = 0; i < linesList.Count; i++) {
            linesList [i].SetActive (false);
        }
    }

    public void ShowCurrentLines () {
        isLinesShowed = true;

        var name = string.Format ("main:lines:select:{0}", currentLine + 1);
        SoundManager.Instance.Play (name);

        for (int i = 0; i < linesList.Count; i++) {
            if (i <= currentLine) {
                indicatorsList [i].SetActive (true);
                linesList [i].SetActive (true);
            } else {
                indicatorsList [i].SetActive (false);
                linesList [i].SetActive (false);
            }
        }
    }

    public void ShowCurrentIndicators () {
        for (int i = 0; i < indicatorsList.Count; i++) {
            if (i <= currentLine) {
                indicatorsList [i].SetActive (true);
            } else {
                indicatorsList [i].SetActive (false);
            }
        }
    }

    void CreateLines () {
        var parent = GameObject.FindGameObjectWithTag ("MainGameContainer");

        for (int i = 0; i < lines.Length; i++) {
            GameObject gm = new GameObject ();
            gm.name = "Line" + i;
            gm.layer = LayerMask.NameToLayer (Constants.SettingsDefaultLayersName);
            gm.SetActive (false);

            gm.AddComponent<SpriteRenderer> ();
            gm.GetComponent<SpriteRenderer> ().sprite = lines [i];
            gm.GetComponent<SpriteRenderer> ().sortingOrder = 5;
            gm.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.SettingsSlotAnimationOverlayLayer;

            gm.transform.parent = slotContainer.GetComponent<CurrentSlotInfo> ().LinesContainer.transform;
            gm.transform.localScale = Vector3.one;
            gm.transform.localPosition = Vector3.zero;
            gm.transform.localRotation = Quaternion.identity;

            linesList.Add (gm);
        }
        currentLine = linesList.Count - 1;
    }

    void CreateIndicators () {
        for (int i = 0; i < indicators.Length; i++) {
            GameObject gm = new GameObject ();
            gm.name = "Indicators" + i;
            gm.layer = LayerMask.NameToLayer (Constants.SettingsDefaultLayersName);
            gm.SetActive (true);

            gm.AddComponent<SpriteRenderer> ();
            gm.GetComponent<SpriteRenderer> ().sprite = indicators [i];
            gm.GetComponent<SpriteRenderer> ().sortingOrder = 5;
            gm.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.SettingsSlotAnimationOverlayLayer;

            gm.transform.parent = slotContainer.GetComponent<CurrentSlotInfo> ().IndicatorsContainer.transform;
            gm.transform.localScale = Vector3.one;
            gm.transform.localPosition = Vector3.zero;
            gm.transform.localRotation = Quaternion.identity;
            indicatorsList.Add (gm);
        }
    }

    void SetStartValue () {
        slotReelsManager = GetComponent<SlotReelsManager> ();
        slotContainer = GameObject.FindGameObjectWithTag ("SlotContainer");
        var si = slotContainer.GetComponent<CurrentSlotInfo> ();

        linesParent = slotContainer.transform.parent.transform;
        lines = si.lines;
        indicators = si.indicators;
        isBlinkIndicators = si.isBlinkIndicators;
        blinkDelay = si.blinkDelay;
        lineDelay = si.lineDelay;
    }

    public bool isEndAnimationLines () {
        if (isWinnigLinesShowed && isBlinkEnd) {
            return true;
        }
        return false;
    }

    public void PreparetoSpin () {
        isWinnigLinesShowed = false;
        isBlinkEnd = false;
        numBlink = 0;
        nextBlinkTime = 0;
        nextLineTime = 0;
        currentWinLineKey = 0;
        HideAllLines ();
        ShowCurrentIndicators ();
        stopAnimation = false;
        _notified = false;
    }

    public bool isCurrentLinesShowed () {
        return isLinesShowed;
    }
}
