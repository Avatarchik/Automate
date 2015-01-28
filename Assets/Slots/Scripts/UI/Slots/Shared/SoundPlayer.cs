using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class SoundPlayer : MonoBehaviour {

    public const string Tag = "SoundPlayer";

    public bool Exists (string clipname) {
        return gameObject.GetComponentsInChildren<AudioController> ().Any (c => c.ClipName == clipname);
    }

    public void Play (string clipname) {
        var clip = GetClip (clipname);

        if (clip != null)
            clip.Play ();
    }

    public void Stop(string clipname) {  
        var clip = GetClip (clipname);

        if (clip != null)
            clip.Stop ();
    }

    public List<string> GetClipNames () {
        return (from c in gameObject.GetComponentsInChildren<AudioController> ()
                      select c.ClipName).ToList<string> ();
    }

    public AudioController GetClip (string clipname) {
        return (from c in gameObject.GetComponentsInChildren<AudioController> ()
                      where c.ClipName == clipname
                      select c).FirstOrDefault ();
    }

    // Use this for initialization
    void Start () {
        gameObject.tag = SoundPlayer.Tag;
    }
	
}
