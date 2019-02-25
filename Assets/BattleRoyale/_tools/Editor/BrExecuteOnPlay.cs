using System.Collections;
using System.Collections.Generic;
using JetBrains.Rider.Unity.Editor;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class BrExecuteOnPlay
{
    static BrExecuteOnPlay()
    {
        Debug.Log("SingleEntryPoint. Up and running");
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }
     
    private static void OnPlayModeChanged(PlayModeStateChange playModeStateChange)
    {
        if (BrPickupManager.Instance != null)
        {
            BrPickupManager.Instance.CollectAllPickups();
            EditorUtility.SetDirty(BrPickupManager.Instance);
        }
    }
}
