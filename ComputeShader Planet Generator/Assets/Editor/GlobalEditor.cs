using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Global))]
public class GlobalEditor : Editor {
    Global main;
    private void OnEnable() {
        main = (Global)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        GUILayout.Space(5);
        if (GUILayout.Button("Recalculate")) {
            if (EditorApplication.isPlaying && !EditorApplication.isPaused) {
                main.Recalculate();
            } else {
                EditorUtility.DisplayDialog("Tried to build a planet.", "The build try failed. You can only build a planet while playmode is running.", "Cancel");
            }
        }
    }
}
