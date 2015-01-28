using UnityEngine;
using UnitySlot;

namespace UnitySlot {

    public static class GameState {

        private static object cgsLocker = new object ();
        private static object fgsLocker = new object ();
        private static object bgsLocker = new object ();

        private static CurrentGameState _cgsInstance;
        private static FreeGameState _fgsInstance;
        private static BonusGameState _bgsInstance;

        private static CurrentGameState CGSInstance {
            get {
                if (_cgsInstance == null) {
                    lock (cgsLocker) {
                        if (_cgsInstance == null) {
                            _cgsInstance = new CurrentGameState ();
                            
                        }
                    }
                }
                return _cgsInstance;
            }
        }

        private static FreeGameState FGSInstance {
            get {
                if (_fgsInstance == null) {
                    lock (fgsLocker) {
                        if (_fgsInstance == null) {
                            _fgsInstance = new FreeGameState ();
                        }
                    }
                }
                return _fgsInstance;
            }
        }

        private static BonusGameState BGSInstance {
            get {
                if (_bgsInstance == null) {
                    lock (bgsLocker) {
                        if (_bgsInstance == null) {
                            _bgsInstance = new BonusGameState ();
                        }
                    }
                }
                return _bgsInstance;
            }
        }

        public static CurrentGameState CurrentGame {
            get {
                return CGSInstance;
            }
        }

        public static FreeGameState FreeGame {
            get {
                return FGSInstance;
            }
        }

        public static BonusGameState BonusGame {
            get {
                return BGSInstance;
            }
        }
    }

}