using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

[Serializable]
public class TimeMachineClip : PlayableAsset, ITimelineClipAsset
{
	[HideInInspector]
    public TimeMachineBehaviour template = new TimeMachineBehaviour ();

    [Header("Start")]
    public TimelineControllerAction Start;
    
    
   // [Space]
    [Header("End")]
    public TimelineControllerAction End;
	
    [HideInInspector]
    public TimelineClip clip;

    public TimelineControllerEvents events;


    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TimeMachineBehaviour>.Create (graph, template);
        TimeMachineBehaviour clone = playable.GetBehaviour ();
        clone.clip = clip;
        clone.Enter = Start;
        clone.Exit = End;
        return playable;
    }
}
