using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GeneratePlanet))]
public class MainEditor : Editor
{
    GeneratePlanet main;
    private void OnEnable() {
        main = (GeneratePlanet)target;
    }
    
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        GUILayout.Space(5);
        if (GUILayout.Button("Build")) {
            if (EditorApplication.isPlaying && !EditorApplication.isPaused) {
                main.Build();
            } else {
                EditorUtility.DisplayDialog("Tried to build a planet.", "The build try failed. You can only build a planet while playmode is running.", "Cancel");
            }
        }
    }
}
