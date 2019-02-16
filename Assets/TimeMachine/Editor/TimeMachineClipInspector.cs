using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimeMachineClip))]
public class TimeMachineClipInspector : Editor
{
    private TimeMachineClip clip;

    public override void OnInspectorGUI()
    {
        clip = target as TimeMachineClip;

        GetAction(clip.Start, "Start");

        //EditorGUILayout.Space();

        GetAction(clip.End, "End");
    }

    private void GetAction(TimelineControllerAction action, string lable)
    {
        GUILayout.BeginVertical("box");

        EditorGUILayout.LabelField(lable, EditorStyles.boldLabel);

        action.Action = (TimelineControllerAction.TimeMachineAction)
            EditorGUILayout.EnumPopup("Action", action.Action);

        switch (action.Action)
        {
            case TimelineControllerAction.TimeMachineAction.None:
                break;

            case TimelineControllerAction.TimeMachineAction.JumpToClip:

                var names = clip.clip.parentTrack.GetClips().Select(c => c.displayName).ToList();

                var selectedIndex = names.IndexOf(action.markerToJumpTo);

                action.markerToJumpTo = names[EditorGUILayout.Popup(
                    "Clip Name",
                    selectedIndex == -1 ? 0 : selectedIndex,
                    names.ToArray()
                )];

                GetCondition(action);
                break;
            case TimelineControllerAction.TimeMachineAction.Pause:
                GetCondition(action);
                break;

            case TimelineControllerAction.TimeMachineAction.TriggerEvent:
                if (clip.events)
                {
                    var eNames = clip.events.events.Select(e => e.Name).ToList();
                    var indexOf = eNames.IndexOf(action.EventName);
                    action.EventName = eNames[EditorGUILayout.Popup(
                        "Event Name",
                        indexOf == -1 ? 0 : indexOf,
                        eNames.ToArray()
                    )];
                }
                else
                    action.EventName = EditorGUILayout.TextField("Event name", action.EventName);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        GUILayout.EndVertical();
    }

    private static void GetCondition(TimelineControllerAction action)
    {
        action.Conditional = EditorGUILayout.Toggle("Is Conditional", action.Conditional);
        if (action.Conditional)
            action.conditionFlag = EditorGUILayout.Toggle("Condition", action.conditionFlag);
    }
}