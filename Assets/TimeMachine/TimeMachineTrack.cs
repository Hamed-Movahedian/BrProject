using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.7366781f, 0.3261246f, 0.8529412f)]
[TrackClipType(typeof(TimeMachineClip))]
public class TimeMachineTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
		var scriptPlayable = ScriptPlayable<TimeMachineMixerBehaviour>.Create(graph, inputCount);

		TimeMachineMixerBehaviour b = scriptPlayable.GetBehaviour();
		b.markerClips = new System.Collections.Generic.Dictionary<string, double>();
		
		var events = (graph.GetResolver() as PlayableDirector)?.GetComponent<TimelineControllerEvents>();


		foreach (var c in GetClips())
		{
			TimeMachineClip clip = (TimeMachineClip)c.asset;
			
			clip.clip=c;

			clip.events = events;
			
			if(!b.markerClips.ContainsKey(c.displayName)) 
				b.markerClips.Add(c.displayName, (double) c.start);

		}

        return scriptPlayable;
    }


}
