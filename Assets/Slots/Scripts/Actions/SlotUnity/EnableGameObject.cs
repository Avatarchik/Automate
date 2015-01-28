using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory ("SlotUnity")]
public class EnableGameObject : FsmStateAction {
    public FsmGameObject gameObject;

    public override void OnEnter () {
        if (gameObject.Value != null) {
            gameObject.Value.SetActive (true);
        }
        Finish ();
    }
}
