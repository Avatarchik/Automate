using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CustomEditor (typeof(CurrentSlotInfo))]
public class SlotInfoEditor : Editor
{
		bool lindexes;

		bool[] dots;

		bool[] lines;

		CurrentSlotInfo slotInfo;

		SerializedObject _si;
		SerializedProperty _flags;

		void OnEnable ()
		{
				_si = new SerializedObject (target);
				lines = new bool[_si.FindProperty ("lines").arraySize];
		}

		string szArrayItem = "linesIndex.Array.data[{0}]";

		static bool GetBit (int x, int bitnum)
		{
				if (bitnum < 0 || bitnum > 31)
						throw new ArgumentOutOfRangeException ("Invalid bit number");

				return (x & (1 << bitnum)) != 0;
		}

		static void SetBit (ref int x, int bitnum, bool val)
		{
				if (bitnum < 0 || bitnum > 31)
						throw new ArgumentOutOfRangeException ("Invalid bit number");

				if (val)
						x |= (int)(1 << bitnum);
				else
						x &= ~(int)(1 << bitnum);
		}


		void SetBool (int i, int index, bool b)
		{
				int val = _si.FindProperty (String.Format (szArrayItem, i)).intValue;
				SetBit (ref val, index, b);

				_si.FindProperty (String.Format (szArrayItem, i)).intValue = val;
		}

		bool GetBool (int i, int index)
		{
				return GetBit (_si.FindProperty (String.Format (szArrayItem, i)).intValue, index);
		}

		public override void OnInspectorGUI ()
		{
				_si.Update ();

				slotInfo = (CurrentSlotInfo)_si.targetObject;

				DrawDefaultInspector ();

				lindexes = EditorGUILayout.Foldout (lindexes, "Line indexes");
				EditorGUI.indentLevel++;
				if (lindexes)
						for (int i = 0; i < slotInfo.lines.Length; i++) {

								lines [i] = EditorGUILayout.Foldout (lines [i], "Line " + (i + 1));
								if (lines [i]) { 
										EditorGUI.indentLevel++;

										for (int line = 0; line < 3; line++) {


												EditorGUILayout.BeginHorizontal ();

												for (int k = 0; k < slotInfo.reelsNum; k++) {
														var index = k * 3 + line;
														var b = EditorGUILayout.Toggle (GetBool (i, index));

														SetBool (i, index, b);
												}

												EditorGUILayout.EndHorizontal ();

										}
										EditorGUI.indentLevel--;
								}
							
						}


				EditorGUI.indentLevel--;


				if (GUI.changed)
						_si.ApplyModifiedProperties ();
		}
}