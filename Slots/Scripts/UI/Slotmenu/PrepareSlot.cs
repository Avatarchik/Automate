using UnityEngine;
using System.Collections;

public class PrepareSlot : MonoBehaviour {

    bool SuccessLoaded;

    void Awake () {

    }

    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (!SuccessLoaded) {
            var mainGameContainer = GameObject.FindGameObjectWithTag ("MainGameContainer");
            if (mainGameContainer) {
                var gameController = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");
                if (gameController) {
                    gameController.GetComponent<SlotController> ().enabled = true;
                    gameController.GetComponent<SlotLinesManager> ().enabled = true;
                    gameController.GetComponent<SlotReelsManager> ().enabled = true;
                }    
                SuccessLoaded = true;
            }
        }
    }
}
