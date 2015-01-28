using UnityEngine;
using System.Collections;

public class OnSlotLoaded : MonoBehaviour {

    void Awake () { 
        //
        // позиционирование слота внутри окна в слот меню. Тег нужен чтобы найти првильный контейнер
        var g = GameObject.FindGameObjectWithTag ("SlotMenuGameContainer");

        if (g != null) {

            var slotTransform = gameObject.transform;
            slotTransform.parent = g.transform;
            slotTransform.localPosition = Vector3.zero;
            slotTransform.localRotation = Quaternion.identity;
            slotTransform.localScale = new Vector3 (101, 101, 100);
        }
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
    
    }
}
