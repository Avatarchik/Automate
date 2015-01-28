using UnityEngine;
using System.Collections;
using UnitySlots;
using System.Collections.Generic;
using System.Linq;

public class SlotAnimationLibrary : MonoBehaviour {

    public SlotAnimation GetAnimation (string clipName) {
        var r = gameObject.GetComponentsInChildren<SlotAnimation> ();

        return (from a in r
                      where  a.ClipName == clipName
                      select a).FirstOrDefault ();
    }

    public IEnumerable<string> GetAnimations () {

        var r = gameObject.GetComponentsInChildren<SlotAnimation> ();

        return (from a in r
                      select a.ClipName).ToList ();
    }
}

