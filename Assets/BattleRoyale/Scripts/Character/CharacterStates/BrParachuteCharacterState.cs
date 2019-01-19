using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BrParachuteCharacterState : BrCharacterStateBase
{
    public float OpenParaDistance = 50;
    public float FallingSpeed = 2.5f;
    public float Speed = 5;
    public float RotationSpeed = 90;

    [Header("Events")]
    public UnityEvent OnOpenPara;
    public UnityEvent OnLanding;


    public override void Update()
    {
        if (_controller.IsGrounded)
        {
            _controller.SetState(CharacterStateEnum.Grounded);
        }
        else
        {
            _controller.MoveAndRotate(Speed, RotationSpeed);

        }
    }

    public override void FixedUpdate()
    {
        if (!_controller.isMine)
            return;

        _controller.transform.position += Vector3.down * FallingSpeed * Time.deltaTime;

        FallingSpeed += 1 * Time.deltaTime;
    }

    public override void OnEnter()
    {
        OnOpenPara.Invoke();
        _controller.Animator.SetTrigger("OpenPara");

        _controller.gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        _controller.Animator.SetBool("OnGround", true);
        OnLanding.Invoke();
        _controller.RigidBody.isKinematic = !isMine;
    }
}