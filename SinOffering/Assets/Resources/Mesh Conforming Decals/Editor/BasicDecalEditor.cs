using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BasicDecal))]
public class RuntimeDecalEditor : Editor {

	public override void OnInspectorGUI() {

		BasicDecal decal = target as BasicDecal;
		if(GUILayout.Button("Reset")) {
			decal.Awake();
			decal.ResetDecal();
		}
		DrawDefaultInspector();
	}
}
