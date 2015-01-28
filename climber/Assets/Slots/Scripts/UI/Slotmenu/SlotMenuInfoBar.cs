using UnityEngine;
using System.Collections;
using UnitySlot;

public class SlotMenuInfoBar : MonoBehaviour {
    public GameObject LineInfo;
    public GameObject BetInfo;
    public GameObject WinInfo;
    public GameObject BetSumInfo;

    UILabel lineLabel;

    UILabel winLabel;

    UILabel betLabel;
    UILabel betSumLabel;

    // Use this for initialization
    void Start () {
        lineLabel = LineInfo.GetComponent<UILabel> ();
        betLabel = BetInfo.GetComponent<UILabel> ();
        winLabel = WinInfo.GetComponent<UILabel> ();
        betSumLabel = BetSumInfo.GetComponent<UILabel> ();
    }
	
    // Update is called once per frame
    void Update () {
        lineLabel.text = Lines.ToString ();
        betLabel.text = Bet.ToString ();
        winLabel.text = Win.ToString ();
        betSumLabel.text = BetSum.ToString ();

        GameState.CurrentGame.Bet = Bet;
        GameState.CurrentGame.Lines = Lines;
    }

    public int Lines;

    public float Bet;
    public float Win;
    public float BetSum;


}
