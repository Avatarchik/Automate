using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CustomEditor (typeof(SlotAnimationLibrary))]
public class SlotAnimationLibraryEditor : Editor {

		public override void OnInspectorGUI () {
				SlotAnimationLibrary library = target as SlotAnimationLibrary;

				if (library != null) { 
						List<string> list = library.GetAnimations ().ToList ();

						if (list.Count == 0) { 
								EditorGUILayout.HelpBox ("There are no animations in this library", MessageType.Warning);
						} else { 
								EditorGUILayout.Separator ();
								EditorGUILayout.LabelField ("Animations list", EditorStyles.boldLabel, GUILayout.ExpandWidth (true));
								EditorGUILayout.Separator ();
						}

						foreach (var s in list) { 
								EditorGUILayout.BeginHorizontal ();
								EditorGUILayout.PrefixLabel (s);
								EditorGUILayout.ObjectField (library.GetAnimation (s), typeof(SlotAnimation), true);
								EditorGUILayout.EndHorizontal ();
						}
				}
		}
}