using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TimeMachineBehaviour : PlayableBehaviour
{
    public TimelineControllerAction Enter;
    public TimelineControllerAction Exit;

    public TimelineClip clip;

}