using UnityEngine;
using System.Collections;
using Core.Server;

public class SlotMenuControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BetAdd() { 
        var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

        if (g != null) {
            g.BroadcastMessage ("BetMessage", 1, SendMessageOptions.RequireReceiver);
        }
    }

    public void BetSub() { 
        var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

        if (g != null) {
            g.BroadcastMessage ("BetMessage", -1, SendMessageOptions.RequireReceiver);
        }
    }

    public void ShowInfo(){
        var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

        if (g != null) {
            g.BroadcastMessage ("ShowInfoMessage", SendMessageOptions.RequireReceiver);
        }
    }

    public void MaxBet() {
        var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

        if (g != null) {
            g.BroadcastMessage("MaxBetMessage", SendMessageOptions.RequireReceiver);
        }
    }

    public void Spin() { 
        var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

        if (g != null) {
            g.BroadcastMessage("SpinMessage", SendMessageOptions.RequireReceiver);
        }
    }

    public void LinesAdd() {
        var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

        if (g != null) {
            g.BroadcastMessage("LinesMessage", 1, SendMessageOptions.RequireReceiver);
        }
    }

    public void LinesSub() {
        var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

        if (g != null) {
            g.BroadcastMessage("LinesMessage", -1, SendMessageOptions.RequireReceiver);
        }
    }

    public void Double() { 
        var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

        if (g != null) {
            g.BroadcastMessage("DoubleMessage", SendMessageOptions.RequireReceiver);
        }
    }

    public void Autoplay() { 
        var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

        if (g != null) {
            g.BroadcastMessage("AutoplayMessage", SendMessageOptions.RequireReceiver);
        }
    }

    public void OpenGamehall() {
        var slotLoader = GameObject.Find ("SlotMenuLoader");
        if (slotLoader) {
            DestroyImmediate(slotLoader);
        }
        SessionData.Instance.LoadGamehall();
        Resources.UnloadUnusedAssets();
    }
}
