using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

[Serializable]
public enum DoubleGameType {
    FiveCard,
    RedBlack,
}
    
[Serializable]
public class CurrentSlotInfo : MonoBehaviour {
    public Sprite[] icons;
    public Sprite[] lines;
    public Sprite[] indicators;
    public int[] linesIndex = new int[40]; // похуй, пусть будет 40 линий по 32 слота на поле
    public DoubleGameType DoubleGameType;
    public bool IgnoreBonusGame;
    public string[] FunBonusGameKeyword;
    public string[] BonusGameKeyword;
    public int reelsNum = 5;
    [Range(0, 20)]
    public int
        different;
    [Range(0, 20)]
    public float
        verticalOffset;
    [Range(0, 20)]
    public float
        horizontalOffset;
    [Range(0, 20)]
    public float
        slotSpeed;
    public bool isRebound;
    public int BonusSymbolCode;
    public bool isBlinkIndicators;
    public float blinkDelay;
    public float lineDelay;
    public int minLines = 1;
    public int maxLine = 9;
    public int LineStep = 2;
    public GameObject LinesContainer;
    public GameObject IndicatorsContainer;
    public SlotAnimationLibrary AnimationLibrary;
}
