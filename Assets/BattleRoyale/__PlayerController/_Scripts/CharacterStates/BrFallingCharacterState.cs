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

    public override void OnEnter()
    {
        _controller.gameObject.SetActive(false);

        //if (isMine)
            _controller.NavMeshAgent.updatePosition = false;

        OnStartFalling.Invoke();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}