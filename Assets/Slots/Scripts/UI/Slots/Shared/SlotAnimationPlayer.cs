using UnityEngine;
using System.Collections;
using UnitySlots;
using System.Linq;
using System.Collections.Generic;

[RequireComponent (typeof(SpriteRenderer))]
public class SlotAnimationPlayer : MonoBehaviour {

    public GameObject AnimationSource = null;
    private SlotAnimation currentAnimation;
    public SlotAnimationLibrary library;
    private bool _animate;
    int _animationCode;
    public int StartFrame;
    public AnimationLoop loopType = AnimationLoop.Default;
    public Sprite DefaultPlaceHolder;

    public IEnumerable<string> animations {
        get { 
            var arr = new List<string> ();

            if (AnimationSource != null) {
                var comps = AnimationSource.GetComponents<SlotAnimation> ();

                foreach (var a in comps) { 
                    arr.Add (a.ClipName);
                }
            }

            if (library != null)
                arr.AddRange (library.GetAnimations ());

            return arr;
        } 
    }

    public bool Animate {
        get { 
            return _animate;
        }
        set { 
            if (_animate == value)
                return;
            // если не указан не один источник или пустое имя ролика
            if (string.IsNullOrEmpty (m_clipName) || (AnimationSource == null && library == null)) { 
                Debug.LogError ("[" + gameObject.name + "] Clip or animation source are not specified - " + m_clipName);
                _animate = false;
                ActivatePlaceHolder ();
                return;
            }

            currentAnimation = GetAnimation (m_clipName);

            if (currentAnimation == null) { 
                Debug.Log ("Animation [" + m_clipName + "] is not found in given sources");
                _animate = false;
                ActivatePlaceHolder ();
                return;
            }

            if (value) { 
                _animationCode = currentAnimation.Animate (gameObject, StartFrame, loopType);
            } else {
                currentAnimation.StopAnimation (_animationCode);

                ActivatePlaceHolder ();
            }

            _animate = value;
        }
    }


    // Эта ебля с тремя переменными нужна чтобы работал кастомный инспектор для этого компонента
    [HideInInspector]
    public string ClipName;
    string m_clipName;

    protected string InternalClipName {
        get {
            return m_clipName;
        }
        set {
            var b = Animate;
            if (m_clipName != value && Animate)
                Animate = false;

            m_clipName = value;

            Animate = b;
        }
    }

    SlotAnimation GetAnimation (string clipName) {

        if (AnimationSource != null) { 
            var comps = AnimationSource.GetComponents<SlotAnimation> ();

            foreach (var a in comps) { 
                if (a.ClipName == clipName)
                    return a;
            }
        }

        if (library != null)
            return library.GetAnimation (clipName);
        else
            return null;
    }

    void ActivatePlaceHolder () {
        if (DefaultPlaceHolder != null) {
            var sr = gameObject.GetComponent<SpriteRenderer> ();
            if (sr != null) {
                sr.sprite = DefaultPlaceHolder;
            }
        }
    }

    void SetClipName (string clipName) {
        ClipName = clipName;
        InternalClipName = clipName;
    }

    public bool TryPlayClip(string clipName) { 

        if (!animations.Any (c => c == clipName))
            return false;

        SetClipName (clipName);
        Animate = true;

        return true;
    }

    // Update is called once per frame
    void Update () {
        InternalClipName = ClipName;
    }
}
