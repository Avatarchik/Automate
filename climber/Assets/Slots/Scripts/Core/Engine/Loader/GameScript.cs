using UnitySlot;
using UnityEngine;

public class GameScript: MonoBehaviour {
    GameHolder game;
    SlotHolder slots;
    static GameScript _instance;

    public static GameScript Instance {
        get {
            if (_instance == null) {
                _instance = new GameScript ();
            }
            return _instance;
        }
    }

    void Start () {

        game = XmlUtil.Deserialize<GameHolder> ("description");
        slots = XmlUtil.Deserialize<SlotHolder> ("cache/slots");

        Debug.Log (string.Format ("Slots: {0}", slots.ToString ()));
    }
}


