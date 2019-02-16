using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimelineControllerAction
{
    public enum TimeMachineAction
    {
        None,
        JumpToClip,
        Pause,
        TriggerEvent
    }
    
    public TimeMachineAction Action;
    public bool Conditional;
    public string markerToJumpTo;
    public string EventName;
    public bool conditionFlag = true;

    private bool ConditionMet() => !Conditional || conditionFlag;

    public void RunAction(TimeMachineMixerBehaviour mixer)
    {
        switch (Action)
        {
            case TimeMachineAction.None:
                break;
            
            case TimeMachineAction.JumpToClip:
                if (mixer.markerClips.TryGetValue(markerToJumpTo, out var time))
                    if (ConditionMet())
                    {
                        mixer.director.Pause();
                        mixer.director.time = time;
                        //mixer.director.Evaluate();
                        mixer.director.Play();
                    }
                break;
            
            case TimeMachineAction.Pause:
                if (ConditionMet())
                    mixer.director.Pause();
                break;

            case TimeMachineAction.TriggerEvent:
                if (mixer.events != null) 
                    mixer.events.RunEvent(EventName);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
}