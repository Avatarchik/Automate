using UnityEngine;
using System.Collections;
using UnitySlot;

public class CardController : MonoBehaviour {

    protected CardDescriptor _card;
    public CardDescriptor card {
        get { 
            return _card;
        }
        set { 
            _card = value;
            updateCard ();
        }
    }

    UIButton button;

    public SpriteRenderer JockerPart;
    public SpriteRenderer BackPart;
    public SpriteRenderer [] CardParts;
    public Sprite[] BlackValuesSprites;
    public Sprite[] RedValuesSprites;
    public Sprite[] BigSuitSprites;
    public Sprite[] SuitSprites;
    public Sprite[] TopRightCornerSprites;
    public Sprite Back;
    public Sprite Joker;

    public bool openCard {
        get {
            return _openCard;
        }
        set {
//            Debug.Log ("Card is" + card);
            if (card == null)
                _openCard = false;
            else
                _openCard = value;

//            Debug.Log ("Open card is " + value);
            button.isEnabled = !value;

            updateCard ();
        }
    }

    public bool _openCard = false;

    public void ResetState() { 
        BackPart.enabled = true;
        JockerPart.enabled = false;

        foreach (var p in CardParts) { 
            p.enabled = false;
        }
    } 

    protected void updateCard() {
        if (card == null)
            return;

        BackPart.enabled = !_openCard;
        JockerPart.enabled = card.IsJoker;

        if (card.IsJoker || !openCard)
            return;
        
        mapValues ();
    }

    protected void mapValues() {
        var values = RedValuesSprites;

        if (card.IsSuitBlack)
            values = BlackValuesSprites;

//        Debug.Log ("Card parts length " + card);

        CardParts [0].sprite = values[(int)card.Value - 2];
        CardParts [1].sprite = SuitSprites [(int)card.Suit];
        CardParts [2].sprite = BigSuitSprites [(int)card.Suit];

        if (card.Value > CardValue.Ten && card.Value != CardValue.Ace)
            CardParts [3].sprite = TopRightCornerSprites [((int)card.Value - 10) * (int)(card.Suit + 1)];
        else
            CardParts [3].sprite = TopRightCornerSprites [0];
    }

    void Start() { 
        JockerPart.sprite = Joker;
        BackPart.sprite = Back;
        _openCard = false;

        button = gameObject.GetComponent<UIButton> ();
    }
}


