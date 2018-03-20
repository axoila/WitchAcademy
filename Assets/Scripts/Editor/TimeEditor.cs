using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimeManager))]
public class TimeEditor : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		if(GUILayout.Button("Reset Time"))
        {
            TimeManager manager = target as TimeManager;
			manager.Reset();
        }
	}
}
