using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class BrTimelineEvent : MonoBehaviour
{
    public bool Condition = true;
    public List<NamedEvent> Events;
    private PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void OnValidate()
    {
        if (director == null)
            Start();
    }

    public void Play()
    {
        if (director && director.state == PlayState.Playing)
            director.Play();
    }

    public void Pause()
    {
        if (director && director.state == PlayState.Playing)
        {
            director.Pause();
        }
    }

    public void Resume()
    {
        if (director && director.state == PlayState.Playing)
            director.Resume();
    }

    public void SetCondition(int value)
    {
        Condition = value == 1;
    }

    public void ToggleCondition()
    {
        Condition = !Condition;
    }

    public void ConditionalJump(int frame)
    {
        if (Condition && director && director.state == PlayState.Playing)
            Jump(frame);
    }

    public void Jump(int frame)
    {
        if (director && director.state == PlayState.Playing)
        {
            director.Pause();
            director.time = frame / 60d;
            director.Evaluate();
            director.Play();
        }
    }


    public void CustomEvent(string Name)
    {
        if (director && director.state == PlayState.Playing)
            Events.FirstOrDefault(e => e.Name == Name)?.Action.Invoke();
    }
}

[Serializable]
public class NamedEvent
{
    public string Name;
    public UnityEvent Action;
}