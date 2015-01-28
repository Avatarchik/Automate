using Core.Server;
using UnityEngine;

public class AudioController : MonoBehaviour {
    public enum SoundType {
        SingleSound,
        BackgoundMusic,
        OneShotLongSound,
        
    }

    public enum Trigger {
        OnClick,
        OnEnable,
        OnDisable,
        OnStart,
        Custom
    }

    public AudioClip audioClip;
    public SoundType soundType;
    public Trigger trigger;
    public bool isLoop = false;
    public string ClipName;

    private AudioSource sound;
    private bool _isPlaying = false;

    // Use this for initialization
    void Start () {
        SetAudioController (audioClip, soundType, isLoop, trigger);

        if (trigger.Equals (Trigger.OnStart)) {
            Play ();
        }
    }
	
    // Update is called once per frame
    void Update () {
        if (soundType.Equals (SoundType.BackgoundMusic)) {
            var session = SessionData.Instance;
            if (session.IsPlayMusic) {
                if (!_isPlaying) {
                    Play ();
                }
            } else {
                if (_isPlaying) {
                    Stop ();
                }
            }
        }
    }

    void OnClick () {
        if (trigger.Equals (Trigger.OnClick)) {
            Play ();
        }
    }

    void OnEnable () {
        if (trigger.Equals (Trigger.OnEnable)) {
            Play ();
        }
    }

    void OnDisable () {
        if (trigger.Equals (Trigger.OnDisable)) {
            Play ();
        }
    }

    void SetAudioController (AudioClip audioClip, SoundType soundType, bool loop, Trigger trigger) {
        this.trigger = trigger;
        SetAudioController (audioClip, soundType, loop);
    }

    public void SetAudioController (AudioClip audioClip, SoundType soundType, bool loop) {
        this.soundType = soundType;
        sound = gameObject.AddComponent<AudioSource> (); // gameObject.GetComponent<AudioSource> () == null ? gameObject.AddComponent<AudioSource> () : gameObject.GetComponent<AudioSource> (); 
        sound.spread = 360;
        sound.clip = audioClip;
        sound.loop = loop;
    }

    public void Stop () {
        sound.Stop ();
        if (soundType.Equals (SoundType.BackgoundMusic)) {
            _isPlaying = false;
        }
    }

    public void Play () {
        if (audioClip != null) {
            var session = SessionData.Instance;
            switch (soundType) {
            case SoundType.OneShotLongSound:
                if (session.IsPlaySound) {
                    sound.Play ();
                }
                break;
            case SoundType.BackgoundMusic:
                if (session.IsPlayMusic) {
                    sound.Play ();
                    _isPlaying = true;
                }
                break;
            case SoundType.SingleSound:
                if (session.IsPlaySound) {
                    if (isLoop) {
                        sound.Play ();
                    } else {
                        NGUITools.PlaySound (audioClip);
                    }
                }
                break;
            }
        } else {
            Debug.Log ("Set audio clip");
        }
    }
}