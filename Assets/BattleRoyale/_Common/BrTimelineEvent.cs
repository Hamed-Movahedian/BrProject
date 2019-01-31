using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class BrTimelineEvent : MonoBehaviour
{
    public bool Condition=true;
    public List<NamedEvent> Events;
    private PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }
    private void OnValidate()
    {
        if(director==null)
            Start();
    }

    public void Play()
    {
        if(director!=null)
            director.Play();
    }
    public void Pause()
    {
        if(director!=null)
            director.Pause();
    }
    public void Resume()
    {
        if(director!=null)
            director.Resume();
    }

    public void SetCondition(bool value)
    {
        Condition = value;
    }

    public void ToggleCondition()
    {
        Condition = !Condition;
    }
    
    public void ConditionalJump(int frame)
    {
        if(Condition)
            Jump(frame);
    }
    public void Jump(int frame)
    {
        if (director)
            director.time = frame / 60d;
    }

    public void CustomEvent(string Name)
    {
        Events.FirstOrDefault(e=>e.Name==Name)?.Action.Invoke();
    }
}
[Serializable]
public class NamedEvent
{
    public string Name;
    public UnityEvent Action;
}
