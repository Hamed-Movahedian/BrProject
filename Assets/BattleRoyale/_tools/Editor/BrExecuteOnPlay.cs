using UnityEditor;

[InitializeOnLoad]
public class BrExecuteOnPlay
{
    static BrExecuteOnPlay()
    {
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
