using UnityEngine;
using System.Collections;

public class RiskPickersController : MonoBehaviour {

    public SpriteRenderer[] pickers;

    void Awake() { 
        ResetState ();
    }

    public void ResetState() { 
        foreach (var p in pickers) { 
            p.enabled = false;
        }
    }

    public void Activate1() { 
        pickers [0].enabled = true;
    }

    public void Activate2() { 
        pickers [1].enabled = true;
    }

    public void Activate3() { 
        pickers [2].enabled = true;
    }

    public void Activate4() { 
        pickers [3].enabled = true;
    }

}
