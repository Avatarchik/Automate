using UnityEngine;
using System.Collections;
using System;

public static class GOExtentions { 

    public static ExecuteOnTime AddDelayedActionToObject(this GameObject obj, TimeSpan time, EventHandler action) {
        return ExecuteOnTime.AddDelayedActionToObject (obj, time, action);
    }   
}
