using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory ("SlotUnity")]
public class SlotRadioSelector : FsmStateAction {
    public FsmOwnerDefault RadioSelector;
    public FsmInt SlotIndex;

    public override void OnEnter () {
        GameObject go = RadioSelector.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : RadioSelector.GameObject.Value;
        if (go != null) {
            go.SendMessage ("RadioButtonSelectSlot", SlotIndex.Value, SendMessageOptions.DontRequireReceiver);
        }
        Finish ();
    }
}
