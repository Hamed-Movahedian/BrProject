using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class BrSettingsWindow : EditorWindow
{
    public static int RunCount = 4;
    private static string lastBuildPath="";


    void OnGUI()
    {
        RunCount = EditorGUILayout.IntField("Instance Count", RunCount);
        // Use the Object Picker to select the start SceneAsset
        EditorSceneManager.playModeStartScene = (SceneAsset) EditorGUILayout.ObjectField(new GUIContent("Start Scene"),
            EditorSceneManager.playModeStartScene, typeof(SceneAsset), false);

        if (GUILayout.Button("Close all"))
        {
            CloseAllInstances();
        }
        
        if (GUILayout.Button("Build & Run"))
        {
            CloseAllInstances();
            BuildPlayerOptions buildPlayerOptions=new BuildPlayerOptions();
            buildPlayerOptions=BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(buildPlayerOptions);
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }
        
        if(lastBuildPath!="")
            if (GUILayout.Button("Run Last Build"))
                RunLastBuild();
            
    }

    [MenuItem("Battle Royal/Settings &%s")]
    static void Open()
    {
        GetWindow<BrSettingsWindow>();
    }

    static void RunLastBuild()
    {
        CloseAllInstances();
        OnPostprocessBuild(BuildTarget.StandaloneWindows64,@"C:\Users\Hamed\Documents\Projects\APKs\Br\BattleRoyal.exe");
    }

    static void CloseAllInstances()
    {
        var processes = Process.GetProcessesByName("BattleRoyal");
        foreach (var process in processes)
        {
            process.CloseMainWindow();
        }
    }

    [ContextMenu("")]
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        lastBuildPath=pathToBuiltProject;
        if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
        {
            var width = 1920 / 2;
            var height = 1080 / 2;
            
            List<Process> processes=new List<Process>();

            for (int i = 0; i < RunCount; i++)
            {
                var process = new Process();
                process.StartInfo.FileName = pathToBuiltProject;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                processes.Add(process);
                Thread.Sleep(1000);

                IntPtr window1 = FindWindow(null, "BattleRoyal");
                
                SetWindowPos(
                    window1, //process.MainWindowHandle, 
                    0, 
                    width*(i%2), 
                    height*(i/2), 
                    width, height,  0x0040);
            }

        }
    }
    
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

#endif
}