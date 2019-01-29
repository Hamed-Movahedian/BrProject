using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public  class BrRunIntroWindow :EditorWindow
{
    void OnGUI()
    {
        // Use the Object Picker to select the start SceneAsset
        EditorSceneManager.playModeStartScene = (SceneAsset)EditorGUILayout.ObjectField(new GUIContent("Start Scene"), EditorSceneManager.playModeStartScene, typeof(SceneAsset), false);

        if (GUILayout.Button("Close"))
            Close();
    }

    [MenuItem("Battle Royal/Run Intro &%i")]
    static void Open()
    {
        GetWindow<BrRunIntroWindow>();
    }
}