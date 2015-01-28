using UnityEngine;

public class PlaySFX : MonoBehaviour {
    public enum Trigger {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
        Custom,
    }

    public AudioClip audioClip;
    public Trigger trigger = Trigger.OnClick;
    bool mIsOver = false;
    #if UNITY_3_5
        public float volume = 1f;
        public float pitch = 1f;
    
#else
    [Range (0f, 1f)] public float volume = 1f;
    [Range (0f, 2f)] public float pitch = 1f;
    #endif
    void OnHover (bool isOver) {
        if (trigger == Trigger.OnMouseOver) {
            if (mIsOver == isOver)
                return;
            mIsOver = isOver;
        }

//        if (enabled && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
//            SoundManager.PlaySFX (gameObject, audioClip, false, volume, pitch);
    }

    void OnPress (bool isPressed) {
        if (trigger == Trigger.OnPress) {
            if (mIsOver == isPressed)
                return;
            mIsOver = isPressed;
        }

//        if (enabled && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
//            SoundManager.PlaySFX (gameObject, audioClip, false, volume, pitch);
    }

    void OnClick () {
//        if (enabled && trigger == Trigger.OnClick)
//            SoundManager.PlaySFX (gameObject, audioClip, false, volume, pitch);
    }

    void OnSelect (bool isSelected) {
        if (enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
            OnHover (isSelected);
    }

    public void Play () {
//        SoundManager.PlaySFX (gameObject, audioClip, false, volume, pitch);
    }
}