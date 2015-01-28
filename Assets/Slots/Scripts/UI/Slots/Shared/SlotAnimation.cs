using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnitySlots;

//[ExecuteInEditMode]
public class SlotAnimation : MonoBehaviour {

    public string ClipName;

    public Sprite[] frames;

    public float fps = 30;

    private Dictionary<int, bool> activeAnimations = new Dictionary<int, bool> ();

    public AnimationLoop loop;

    public const int INVALID_CODE = 0xBADF00D;

    public int Animate (GameObject obj, int frame = 0, AnimationLoop loopType = AnimationLoop.Default)
    {
        // В анимированном объекте требуется наличие компонента для рендера спрайтов 
        if (obj != null && obj.GetComponent<SpriteRenderer> () != null) { 

            var id = (int)(Random.value * 10000);
            activeAnimations [id] = true;

//            Debug.Log ("Starting animation [" + ClipName + "] with id " + id);

            var c = StartCoroutine (AnimationCoroutine (new AnimationCoroutineParameter {
                id = id, 
                obj = obj,
                frame = frame,
                Loop = loopType == AnimationLoop.Default ? loop : loopType
            }));

            return id;
        }

        return INVALID_CODE;
    }

    public void StopAnimation (int code) { 
        if (code != INVALID_CODE && activeAnimations.ContainsKey (code)) {
            activeAnimations [code] = false;
        }
    }

    int IncrementFrame (int prevFrame, AnimationCoroutineParameter p) { 

        var i = prevFrame + p.step;

        if (p.Loop == AnimationLoop.FixedFrame) { 
            if (i > frames.Length)
                Debug.Log ("Fixed frame is out of range [" + ClipName + ":" + (frames.Length - 1) + "] current frame " + p.frame);
            return prevFrame;
        }

        if (i < 0) {

            switch (p.Loop) {

            case AnimationLoop.PingPong:
                { 
                    p.step *= -1;
                    i = i + p.step;
                    break;
                }
            case AnimationLoop.BackwardOneShot:
                {
                    StopAnimation (p.id);
                    return prevFrame;
                }
            default:
                return 0;
            }
        } else if (i >= frames.Length) { 
            switch (p.Loop) {

            case AnimationLoop.PingPong:
                { 
                    p.step *= -1;
                    i = i + p.step;
                    break;
                }
            case AnimationLoop.OneShot:
                {
                    StopAnimation (p.id);
                    return prevFrame;
                }
            case AnimationLoop.Loop: 
                {
                    return 0;
                }
            default:
                return frames.Length - 1;
            }
        }

        return i;
    }

    IEnumerator AnimationCoroutine (AnimationCoroutineParameter param) {

        var p = (AnimationCoroutineParameter)param;
        var sr = p.obj.GetComponent<SpriteRenderer> ();
        var currentFrame = p.frame;
        while (activeAnimations [p.id]) { 

//            Debug.Log ("Current frame " + currentFrame + " sprie name " + frames [currentFrame].name);
            currentFrame = IncrementFrame (currentFrame, p);

            if (currentFrame > frames.Length || currentFrame < 0)
                Debug.Log ("Fixed frame is out of range [" + ClipName + ":" + frames.Length + "] current frame " + currentFrame);

            sr.sprite = frames [currentFrame];

            yield return new WaitForSeconds (1.0f / fps);
        }

        activeAnimations.Remove (p.id);
    }
}

