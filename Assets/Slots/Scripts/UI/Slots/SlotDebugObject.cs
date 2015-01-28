using UnityEngine;
using System.Collections;

public class SlotDebugObject : MonoBehaviour {

    void Awake() { 
        var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

        if (g != null) {
            DestroyImmediate (gameObject);
        } else { 
            Debug.Log ("Running in debug mode. Object would be deleted when slot will be loaded into slotmenu.");
        }
    }

}
