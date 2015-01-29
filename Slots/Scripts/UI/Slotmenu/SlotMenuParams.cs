using UnityEngine;
using System.Collections;
using UnitySlot;

class SlotMenuParams : MonoBehaviour
{
    public string slotname;

    public SlotInfo slotInfo;

    public void Awake(){ 
        DontDestroyOnLoad (transform.gameObject);
    }




}

