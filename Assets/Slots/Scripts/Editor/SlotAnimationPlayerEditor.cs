using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CustomEditor (typeof(SlotAnimationPlayer))]
[CanEditMultipleObjects]
public class SlotAnimationPlayerEditor : Editor
{  

		string[] _choices;
		int _choiceIndex = 0;

	SerializedObject _player;

	SerializedProperty _animation;

		public override void OnInspectorGUI ()
		{
				DrawDefaultInspector ();


				_player = new SerializedObject (target);
				_animation = _player.FindProperty ("ClipName");

				SlotAnimationPlayer player = _player.targetObject as SlotAnimationPlayer;

				if (player != null) { 

						if (player.animations != null) { 
								List<string> list = player.animations.ToList ();
								list.Insert (0, "< Defined in runtime >");

								_choices = list.ToArray ();

								var ci = list.IndexOf (_animation.stringValue);

								if (ci < 0)
										ci = 0;

								_choiceIndex = EditorGUILayout.Popup ("Animation", ci, _choices);

								if (_choiceIndex > 0)
										_animation.stringValue = _choices [_choiceIndex];
						}

						if (GUILayout.Button ("Play", GUILayout.Height (25))) {
								if (Application.isPlaying)
										player.Animate = true;
								else
										Debug.Log ("Animations is available only in play mode");
						}

						if (GUILayout.Button ("Stop", GUILayout.Height (25))) {
								if (Application.isPlaying)
										player.Animate = false;
								else
										Debug.Log ("Animations is available only in play mode");
						}

						_player.ApplyModifiedProperties ();
				}

		}
}
