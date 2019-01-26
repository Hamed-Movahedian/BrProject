using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BrFallingCharacterState : BrCharacterStateBase
{
    public UnityEvent OnStartFalling;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
    }

    public override void OnEnter()
    {
        _controller.gameObject.SetActive(false);
        OnStartFalling.Invoke();
    }
}