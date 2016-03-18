using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DrawTail))]
public class DrawTailEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		this.DrawDefaultInspector ();
		DrawTail myTarget = (DrawTail)target;
		
		myTarget.TailLength = EditorGUILayout.FloatField("Tail Length", myTarget.TailLength);
		myTarget.SmoothFactor = EditorGUILayout.FloatField("Smooth Factor", myTarget.SmoothFactor);
	}
}