using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnitySlots;

[CustomEditor (typeof(SlotAnimation))]
public class SlotAnimationEditor : Editor
{  

		string[] _choices;
		int _choiceIndex = 0;

		public override void OnInspectorGUI ()
		{
				DrawDefaultInspector ();

				SlotAnimation anim = target as SlotAnimation;

				if (anim != null) { 

						if (String.IsNullOrEmpty (anim.ClipName)) { 
								EditorGUILayout.HelpBox ("You need to specify clip name", MessageType.Error);
						}
						if (anim.loop == AnimationLoop.Default) { 
								EditorGUILayout.HelpBox ("You need to specify loop type ('Default' is wrong animation type)", MessageType.Error);
						}

						Repaint ();
				}


		}
}

