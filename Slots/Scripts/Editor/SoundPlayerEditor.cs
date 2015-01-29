using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CustomEditor (typeof(SoundPlayer))]
public class SoundPlayerEditor : Editor {

		public override void OnInspectorGUI () {
				SoundPlayer player = target as SoundPlayer;

				if (player != null) { 
						List<string> list = player.GetClipNames ().ToList ();

						if (list.Count == 0) { 
								EditorGUILayout.HelpBox ("There are no sound clips in this object", MessageType.Warning);
						} else { 
								EditorGUILayout.Separator ();
								EditorGUILayout.LabelField ("Clips list", EditorStyles.boldLabel, GUILayout.ExpandWidth (true));
								EditorGUILayout.Separator ();
						}

						foreach (var s in list) { 
								EditorGUILayout.BeginHorizontal ();
								EditorGUILayout.PrefixLabel (s);
								EditorGUILayout.ObjectField (player.GetClip (s), typeof(AudioController), true);
								EditorGUILayout.EndHorizontal ();
						}
				}
		}
}
