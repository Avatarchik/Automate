using UnityEngine;
using System.Collections;
using System;

public class ExecuteOnTime : MonoBehaviour {

    private DateTime startTime;

    bool paused;

    public TimeSpan time;
    public EventHandler action;

    ExecuteOnTime (TimeSpan time, EventHandler action) { 
        this.time = time;
        this.action = action;
    }

	// Use this for initialization
	void Start () {
        Reset ();
	}

    public void Reset() { 
        startTime = DateTime.Now;
    }

    public void Pause() { 
        if (paused)
            return;

        time -= (DateTime.Now - startTime);
        paused = true;
    }

    public void Resume() { 
        if (!paused)
            return;

        startTime = DateTime.Now;
        paused = false;
    }

    public void Stop() { 
        Destroy (this);
    }
	
	// Update is called once per frame
	void Update () {
        if (paused)
            return;

        if (DateTime.Now - startTime >= time) {
            if (action  != null)
                action (gameObject, null);

            Destroy (this);
        }
	}

    public static ExecuteOnTime AddDelayedActionToObject(GameObject obj, TimeSpan time, EventHandler action) {
        var a = obj.AddComponent<ExecuteOnTime> ();
        a.time = time; 
        a.action += action;
        a.Reset ();

        return a;
    }
}

