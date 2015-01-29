using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UnitySlots {

    internal class AnimationCoroutineParameter {
        public int id;
        public GameObject obj;
        public int frame;
        public AnimationLoop Loop;
        public int step = 1;

        public override string ToString () {
            return string.Format ("[id = {0}, Object = {1}, Start Frame = {2}, Loop = {3}, Step = {4}]", id, obj.name, frame, Loop, step);
        }
    }
}