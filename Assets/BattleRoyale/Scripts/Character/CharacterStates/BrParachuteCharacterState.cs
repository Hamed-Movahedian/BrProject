using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BrParachuteCharacterState : BrCharacterStateBase
{
    public float OpenParaDistance = 50;
    public float Velocity = 1;
    public float ParaUpForce = 8;
    public float Speed = 5;
    public float RotationSpeed = 90;

    [Header("Events")]
    public UnityEvent OnOpenPara;
    public UnityEvent OnLanding;
    private bool _canMove = true;


    public override void Update()
    {
        if (_controller.IsGrounded)
        {
            _controller.StartCoroutine(Landing());
        }
        else
        {
            if (_canMove)
                _controller.MoveAndRotate(Speed, RotationSpeed);

        }
    }

    private IEnumerator Landing()
    {
        _controller.Animator.SetBool("OnGround", true);
        OnLanding.Invoke();

        _canMove = false;

        yield return new WaitForSeconds(.4f);

        _canMove = true;

        _controller.SetState(CharacterStateEnum.Grounded);
        // Enable Joyistics
        BrUIController.Instance.SetMovementJoyisticActive(true);
        BrUIController.Instance.SetAimJoyisticActive(true);
    }

    public override void FixedUpdate()
    {
        //_controller.RigidBody.velocity = Vector3.down*Velocity;
        _controller.RigidBody.AddForce(Vector3.up * ParaUpForce, ForceMode.Acceleration);
    }

    public override void OnEnter()
    {
        OnOpenPara.Invoke();
        _controller.Animator.SetTrigger("OpenPara");
        _controller.RigidBody.velocity = Vector3.down * Velocity;

        // Enable Joysticks
        BrUIController.Instance.SetMovementJoyisticActive(true);
        BrUIController.Instance.SetAimJoyisticActive(false);
    }


}