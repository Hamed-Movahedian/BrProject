using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BrParachuteCharacterState : BrCharacterStateBase
{
    public float Speed = 5;
    public float RotationSpeed = 90;

    public GameObject Shadow;
    
    [Header("Events")] public UnityEvent OnOpenPara;
    public UnityEvent OnLanding;

    public override void Update()
    {
        _controller.MoveAndRotate(Speed, RotationSpeed);
    }

    public override void OnEnter()
    {
        OnOpenPara.Invoke();
        _controller.Animator.SetTrigger("OpenPara");
        
        Shadow.SetActive(false);
        _controller.gameObject.SetActive(true);
        //if (!isMine)
            //_controller.characterModel.Show();
    }

    public override void OnExit()
    {
/*
        if (isMine)
            _controller.characterModel.Show();
*/
        Shadow.SetActive(true);

        _controller.Animator.SetBool("OnGround", true);
        OnLanding.Invoke();
        _controller.RigidBody.isKinematic = !isMine;
    }
}