using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory ("SlotUnity")]
public class EnableComponent : FsmStateAction {
    public FsmOwnerDefault owner;
    [RequiredField]
    [UIHint (UIHint.ScriptComponent)]
    public FsmString component;
    public FsmBool enableComponent;

    public override void OnEnter () {
        GameObject go = owner.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : owner.GameObject.Value;
        if (go != null && !string.IsNullOrEmpty (component.Value)) {
            MonoBehaviour aComponent = go.GetComponent (component.Value) as MonoBehaviour;
            if (aComponent != null) {
                aComponent.enabled = enableComponent.Value;
            }
        }
        Finish ();
    }
}
