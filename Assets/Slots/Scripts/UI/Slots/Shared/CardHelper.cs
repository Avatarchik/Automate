using UnityEngine;

namespace UnitySlot {

    public enum CardSuit {
        Diamonds = 0,
        Hearts = 1,
        Spades = 2,
        Clubs = 3,
    }

    public enum CardValue {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14,
    }

    public class CardDescriptor
    {
        public CardValue Value;
        public CardSuit Suit;
        public bool IsJoker;

        public bool IsSuitBlack {
            get {
                return Suit > CardSuit.Hearts;
            }
        }

        public int IntValue {
            get {
                return (int)Value;
            }
        }

        public float NormalizedValue {
            get {
                return 1 / (13f) * (float)IntValue;
            }
        }

        public override string ToString() {
            return IsJoker ? "Card [Joker]" : string.Format("Card [{0} {1}]", Value, Suit);
        }

        public static CardDescriptor FromIndex (int index) {
            Debug.Log (CardHelper.GetCard (index).ToString ());
            return CardHelper.GetCard (index);
        }
    }

    public static class CardHelper {

        const int OFFSET = 2;

        const int SUIT_DIAMONDS = 0;
        const int SUIT_HEARTS = 1;
        const int SUIT_SPADES = 2;
        const int SUIT_CLUBS = 3;
        const int SUIT_COUNT = 4;

        const int CARD_TWO = 0;
        const int CARD_THREE = 1;
        const int CARD_FOUR = 2;
        const int CARD_FIVE = 3;
        const int CARD_SIX = 4;
        const int CARD_SEVEN = 5;
        const int CARD_EIGHT = 6;
        const int CARD_NINE = 7;
        const int CARD_TEN = 8;
        const int CARD_JACK = 9;
        const int CARD_QUEEN = 10;
        const int CARD_KING = 11;
        const int CARD_ACE = 12;
        const int CARD_COUNT = 13;

        const int CARD_JOKER = SUIT_COUNT * CARD_COUNT;

        const int MAX_INDEX = CARD_JOKER + 1;

        static bool isJoker(int index) {
            return index - OFFSET == CARD_JOKER;
        }

        static int getCardIndex(int index) {
            if (isJoker(index)) {
                return CARD_JOKER;
            } else {
                var result = index % CARD_COUNT;
                if (result == 0 || result == 1) {
                    return result == 0 ? 13 : 14;
                } else {
                    return result;
                }
            }
        }

        static int getSuitIndex(int index) {
            return (index - OFFSET) / CARD_COUNT;
        }

        public static CardDescriptor GetCard(int index) {
            var cd = new CardDescriptor();

            int cardIndex = getCardIndex(index);
            if (cardIndex == CARD_JOKER) {
                cd.IsJoker = true;
            } else {

                cd.Value = (CardValue)cardIndex;
                cd.Suit = (CardSuit)getSuitIndex (index);
            }
            //Debug.Log(string.Format("Index [{0}], CardValue [{1}], CardSuit [{2}]", index, cd.Value, cd.Suit));
            return cd;
        }
    }
}