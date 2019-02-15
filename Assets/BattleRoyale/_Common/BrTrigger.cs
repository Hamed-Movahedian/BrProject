using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrTrigger : MonoBehaviour
{
    public UnityEvent OnEnter, OnExit;
    
    private BrPlayerTracker tracker;

    private void Start()
    {
        tracker = BrPlayerTracker.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsActivePlayer(other)) OnEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsActivePlayer(other)) OnExit.Invoke();
    }

    private bool IsActivePlayer(Collider other)
    {
        return true;
    }
}
