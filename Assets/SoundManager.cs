using System;
using UnityEngine;

namespace UnitySlot {

    public class SoundManager {
        public SoundManager () {

        }

        private static SoundManager _instance;

        public static SoundManager Instance { 
            get { 
                if (_instance == null) { 
                    _instance = new SoundManager ();
                }

                return _instance;
            }
        }

        SoundPlayer controller { 
            get { 
                return GameObject.FindGameObjectWithTag (SoundPlayer.Tag).GetComponent<SoundPlayer>();
            }
        }

        public bool IsClipExists (string clipname) { 
            if (controller != null) 
                return controller.Exists (clipname);

            Debug.LogError ("Not SoundPlayer exists in the scene");
            return false;
        }

        public void Play (string clipname) {
            if (controller != null) {
                controller.Play (clipname);
                return;
            }

            Debug.LogError ("Not SoundPlayer exists in the scene");
        }

        public void Stop (string clipname) {
            if (controller != null) {
                controller.Stop (clipname);
                return;
            }

            Debug.LogError ("Not SoundPlayer exists in the scene");
        }


    }
}

