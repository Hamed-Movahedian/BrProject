using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TimelineControllerEvents : MonoBehaviour
{
    [Serializable]
    public class NamedEvent
    {
        public string Name;
        public UnityEvent Event;
    }

    public List<NamedEvent> events;

    public void RunEvent(string eventName)
    {
        events.FirstOrDefault(e=>e.Name==eventName)?.Event.Invoke();
    }
}

