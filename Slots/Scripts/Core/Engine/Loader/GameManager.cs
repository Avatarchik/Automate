using Core.Server;
using UnityEngine;
using System.Collections.Generic;

namespace UnitySlot {
    public class GameManager {
        static GameManager _instance;
        private GameHolder _game;
        private SlotHolder _slotHolder;

        public static GameManager Instance {
            get {
                if (_instance == null) {
                    _instance = new GameManager ();
                    _instance.Init ();
                }
                return _instance;
            }
        }

        public List<SlotInfo> GetSlots {
            get {
                return _slotHolder.Slots ?? new List<SlotInfo> ();
            }
        }

        public GameHolder GetGameHolder {
            get {
                return _game;
            }
        }

        void Init () {
            //game = XmlUtil.Deserialize<GameHolder> ("description");
            //TODO Добавил "SessionData.Instance.FunSlotList != null" чтобы слоты загружались в фановом геймхолле в дальнейшем надо заменить
            if (SessionData.Instance.IsFun && SessionData.Instance.FunSlotList != null) {
                _slotHolder = SessionData.Instance.FunSlotList;
            } else {
                _slotHolder = XmlUtil.Deserialize<SlotHolder>("cache/slots");
            }
            
            Debug.Log (string.Format ("Slots: {0}", _slotHolder.ToString ()));
        }
    }
}