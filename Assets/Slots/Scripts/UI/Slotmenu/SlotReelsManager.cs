using System.Collections.Generic;
using UnityEngine;
using UnitySlot;
using System.Linq;
using System;

[System.Serializable]
class ReelInfo
{
    public GameObject reel;
    public Vector2 slotSize;
    public List<GameObject> slot = new List<GameObject>();
    public bool spinning = true;
    public float targetPosition = 0;
    public int currentStep = 1;
    public int endStep = 0;
}

public class SlotReelsManager : MonoBehaviour {    
    private bool isSpinComplete = false;
    private GameObject slotContainer;
    private Sprite[] icons;
    // TODO число должно браться из CurrentSlotInfo, вместо того чтобы быть здесь захардкожено
    private int reelsNum = 5;
    //TODO тут предполагается что в барабане 3 позиции, не уверен что так надо, но вероятнее всего это надо захардкодить
    private int rowsNum = 3;
    private int different;
    private float verticalOffset;
    private float horizontalOffset;
    private float slotSpeed;
    private bool isRebound;
    CurrentSlotInfo slotInfo;
    private List<ReelInfo> reelsList = new List<ReelInfo> ();
    
//    private int iconSize = 10;
    private bool spinning = false;

    private GameObject [] ResultSlots;

    void Start () {
        SetStartValue ();
        while (slotContainer.transform.childCount > 0)
            DestroyImmediate(slotContainer.transform.GetChild(0).gameObject);
        CreateReels ();
        CreateSlots ();
        SetReelsStartValue ();

        ResultSlots = new GameObject[reelsNum * rowsNum];
    }

    void Update () {
        if (spinning && !GameState.CurrentGame.IsError) {
            for (int i = 0; i < reelsList.Count; i++) {
                if (reelsList [i].spinning) {

                    float stepPosition = -reelsList [i].slotSize.y * reelsList [i].currentStep;

                    float yPos = Mathf.Clamp (reelsList [i].reel.transform.localPosition.y - slotSpeed * Time.deltaTime, stepPosition, reelsList [i].reel.transform.localPosition.y);
                    if (!GameState.CurrentGame.IsDone) {
                        reelsList [i].endStep = GetEndStep (reelsList [i].currentStep, i);
                        reelsList [i].targetPosition = -GetEndStep (reelsList [i].currentStep, i) * reelsList [i].slotSize.y;
                    }

                    reelsList [i].reel.transform.localPosition = new Vector3 (reelsList [i].reel.transform.localPosition.x,
                            yPos);

                    float stopReelPosition = GetStopReelPosition(i);

                    if (reelsList[i].reel.transform.localPosition.y <= stopReelPosition)
                    {
                        reelsList [i].reel.transform.localPosition = new Vector3 (reelsList [i].reel.transform.localPosition.x,
                            stopReelPosition);

                        StopReel (i);
                    }

                    //Set next slot icon
                    if (reelsList [i].reel.transform.localPosition.y == stepPosition) {
                        float minPosition = 0;
                        float maxPosition = 0;
                        int minSlotIndex = 0;
                        for (int r = 0; r < reelsList[i].slot.Count; r++) {
                            if (reelsList [i].slot [r].transform.localPosition.y > maxPosition) {
                                maxPosition = reelsList [i].slot [r].transform.localPosition.y;
                            }
                        }

                        minPosition = maxPosition;

                        for (int r = 0; r < reelsList[i].slot.Count; r++) {
                            if (reelsList [i].slot [r].transform.localPosition.y < minPosition) {
                                minPosition = reelsList [i].slot [r].transform.localPosition.y;
                                minSlotIndex = r;
                            }
                        }
                        reelsList [i].slot [minSlotIndex].transform.localPosition = new Vector3 (reelsList [i].slot [minSlotIndex].transform.localPosition.x, maxPosition + reelsList [i].slotSize.y);
                        SetSlotIcon (i, reelsList [i].slot [minSlotIndex]);
                        reelsList [i].currentStep++;
                    }
                }
            }
        }

        if (isRebound)
        {
            for (int i = 0; i < reelsList.Count; i++)
            {
                if (!reelsList[i].spinning && reelsList[i].reel.transform.localPosition.y < reelsList[i].targetPosition)
                {
                    float yPos = Mathf.Clamp(reelsList[i].reel.transform.localPosition.y + slotSpeed*Time.deltaTime,
                        reelsList[i].reel.transform.localPosition.y, reelsList[i].targetPosition);
                    reelsList[i].reel.transform.localPosition = new Vector3(reelsList[i].reel.transform.localPosition.x,
                        yPos);
                    if (i == reelsList.Count - 1)
                    {
                        SoundManager.Instance.Stop ("main:spin:loop");

                        isSpinComplete = true;
                    }
                }
            }
        }
        
    }

    float GetStopReelPosition(int key)
    {
        if (isRebound)
        {
            return reelsList[key].targetPosition - reelsList[key].slotSize.y/2;
        }
        return reelsList[key].targetPosition;
    }

    void SetStartValue () {
        slotContainer = GameObject.FindGameObjectWithTag ("SlotContainer");
        slotInfo = slotContainer.GetComponent<CurrentSlotInfo> ();

        icons = slotInfo.icons;
        different = slotInfo.different;
        verticalOffset = slotInfo.verticalOffset;
        horizontalOffset = slotInfo.horizontalOffset;
        slotSpeed = slotInfo.slotSpeed;
        isRebound = slotInfo.isRebound;
    }

    void SetSlotIcon (int key, GameObject gobject) {
        var spriteRenderer = gobject.GetComponent<SpriteRenderer> ();
        var animationPlayer = gobject.GetComponent<SlotAnimationPlayer> ();
        spriteRenderer.sprite = icons [UnityEngine.Random.Range (0, icons.Length)];
        int stepDifferent = reelsList [key].endStep - reelsList [key].currentStep;
        int currentIcon = 0;
        if (GameState.CurrentGame.IsDone && !GameState.CurrentGame.IsError) {
            if (stepDifferent <= 3 && stepDifferent > 0) {

                var index = key * 3 + (stepDifferent - 1);
                currentIcon = GameState.CurrentGame.ReelSymbols [index];
                spriteRenderer.sprite = icons [currentIcon - 1];
                animationPlayer.DefaultPlaceHolder = icons [currentIcon - 1];

                // всовываем нужный объект с картинкой по нужному индексу что бы он соответствовал ответу сервера
                ResultSlots [index] = spriteRenderer.gameObject;
            }
        }
    }
        
    void StopReel (int key) {
        SoundManager.Instance.Play ("main:spin:stop");
        reelsList [key].spinning = false;
        if (key == reelsList.Count - 1) {
            spinning = false;

            var go = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");
            if (go != null) 
                go.BroadcastMessage ("SpinCompleteMessage", SendMessageOptions.DontRequireReceiver);
        }

        if (!isRebound)
        {
            SoundManager.Instance.Stop ("main:spin:loop");

            isSpinComplete = true;
        }
    }

    int GetEndStep (int step, int increment) {

        return step + (increment * different) + 4;
    }

    void CreateReels () {
        for (int i = 0; i < reelsNum; i++) {
            ReelInfo reelInfo = new ReelInfo ();
            GameObject gm = new GameObject ();
            gm.name = "Reel" + i;
            gm.layer = LayerMask.NameToLayer(Constants.SettingsDefaultLayersName);
            gm.transform.parent = slotContainer.transform;

            gm.transform.localScale = Vector3.one;
            gm.transform.localPosition = Vector3.zero;
            gm.transform.localRotation = Quaternion.identity;
            reelInfo.reel = gm;
            reelsList.Add (reelInfo);
        }
    }

    void CreateSlots () {
        for (int i = 0; i < reelsList.Count; i++) {
            for (int r = 0; r < 5; r++) {
                var gm = new GameObject ();
                var sprite = icons [UnityEngine.Random.Range (0, icons.Length)];

                gm.name = "Icon" + r;
                gm.layer = LayerMask.NameToLayer(Constants.SettingsDefaultLayersName);
                gm.transform.parent = reelsList [i].reel.transform;

                var sr = gm.AddComponent<SpriteRenderer> ();
                var ap = gm.AddComponent<SlotAnimationPlayer> ();
                ap.library = slotInfo.AnimationLibrary;
                ap.DefaultPlaceHolder = sprite;
                sr.sprite = sprite;
                sr.sortingOrder = 3;
                sr.sortingLayerName = Constants.SettingsSlotReelsLayerName;
                reelsList [i].slotSize = new Vector2 (sr.bounds.size.x + horizontalOffset, sr.bounds.size.y + verticalOffset);
                gm.transform.localScale = Vector3.one;
                reelsList [i].slot.Add (gm);
            }
        }
    }

    void SetReelsStartValue () {
        for (int i = 0; i < reelsList.Count; i++) {
            reelsList [i].spinning = true;
            reelsList [i].currentStep = 1;
            reelsList [i].reel.transform.localPosition = slotContainer.transform.position;
            for (int r = 0; r < 5; r++) {
                float xSize = reelsList [i].slotSize.x;
                float ySize = reelsList [i].slotSize.y;
                float startPositionY = -(ySize * (reelsList.Count - 1)) / 2;
                float startPositionX = -(xSize * (reelsList.Count - 1)) / 2;
                reelsList [i].slot [r].transform.localPosition = new Vector3 (i * xSize + startPositionX, r * ySize + startPositionY);
            }
            reelsList [i].reel.transform.localPosition = Vector3.zero;
        }
    }

    public void Spin () {
        SetReelsStartValue ();
        spinning = true;
        isSpinComplete = false;
    }

    public bool isEndAnimateReels()
    {
        if (isSpinComplete)
        {
            return true;
        }
        return false;
    }

    public void AnimateWinLineCells (bool play, int lineIndex = -1) {
        // проиграть анимацию
        if (play) { 

            if (GameState.CurrentGame.WinLines != null && lineIndex < GameState.CurrentGame.WinLines.Count && lineIndex >= 0) {

                var line = new List<int> ();
                var pos = slotInfo.linesIndex [GameState.CurrentGame.WinLines [lineIndex] - 1];

                // перебор всех элементов, сбор компонентов для линии по ее номеру (список всега слева направо, по возрастанию)
                for (int i = 0; i < reelsNum * rowsNum; i++) {
                    if (((1 << i) & pos) != 0)
                        line.Add (i);
                }

                // Если порядок другой - надо перевернуть список ячеек
                var winLength = GameState.CurrentGame.WinLinesLength [lineIndex];
                if (winLength < 0) { 
                    line.Reverse ();
                }

                // по длине выигрышной линии подсветить первые несколько элементов
                for (int i = 0; i < Math.Abs (winLength); i++) {
                    var itemIndex = line [i];
                    var sa = ResultSlots [itemIndex].GetComponent<SlotAnimationPlayer> ();
                    var animationName = String.Format ("{0}:{1}", GameState.CurrentGame.ReelSymbols [itemIndex], "win");
                 
                    sa.TryPlayClip (animationName);
                }
            }
        } else { // остановить анимацию
            for (int i = 0; i < reelsNum * rowsNum; i++) {
                ResultSlots [i].GetComponent<SlotAnimationPlayer> ().Animate = false;
            }
        }
    }

    public void AnimateBonusCells (bool play) 
    {
        
        if (play) {
            // перебор всех ячеек, попытка проиграть анимацию бонуса
            for (int i = 0; i < reelsNum * rowsNum; i++) {
              
                var sa = ResultSlots [i].GetComponent<SlotAnimationPlayer> ();
                if (GameState.CurrentGame.ReelSymbols.Count > 0 && GameState.CurrentGame.ReelSymbols[i] != null)
                {
                   
                    var animationName = String.Format ("{0}:{1}",
                                        GameState.CurrentGame.ReelSymbols [i], GameState.CurrentGame.CurrentSuperGame);
                
                    sa.TryPlayClip (animationName);
                }
            }
        } else {
            for (int i = 0; i < reelsNum * rowsNum; i++)
            {
              
                ResultSlots [i].GetComponent<SlotAnimationPlayer> ().Animate = false;
            }
        }
    }
}