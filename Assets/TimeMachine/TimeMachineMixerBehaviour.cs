using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeMachineMixerBehaviour : PlayableBehaviour
{
    public Dictionary<string, double> markerClips;
    public PlayableDirector director;
    private TimeMachineBehaviour _lastClip;
    public TimelineControllerEvents events;

    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
        _lastClip = null;
    }

    public override void OnPlayableCreate(Playable playable)
    {
        director = (playable.GetGraph().GetResolver() as PlayableDirector);
        events = director.GetComponent<TimelineControllerEvents>();
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        _lastClip = null;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount();
        TimeMachineBehaviour clip = null;

        for (int i = 0; i < inputCount; i++)
            if (playable.GetInputWeight(i) > 0f)
            {
                ScriptPlayable<TimeMachineBehaviour> inputPlayable =
                    (ScriptPlayable<TimeMachineBehaviour>) playable.GetInput(i);
                clip = inputPlayable.GetBehaviour();
                break;
            }

        if (director.state != PlayState.Playing || info.evaluationType != FrameData.EvaluationType.Playback)
        {
            _lastClip = null;
            return;
        }

        if (clip != _lastClip)
        {
            //Debug.Log($"({_lastClip?.clip.displayName}) => ({clip?.clip.displayName})");

            if (_lastClip != null)
            {
                _lastClip.Exit.RunAction(this);
                _lastClip = null;
            }
            else
            {
                clip?.Enter.RunAction(this);
                _lastClip = clip;
            }        
        }
    }
}