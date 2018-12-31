using System;
using UnityEngine;

[Serializable]
public class BrGroundedAimCharacterState : BrCharacterStateBase
{
    public float RotationSpeed = 10;
    public float MoveSpeed = 5;
    public float AnimatorDampValue = 0.2f;

    public override void OnEnter()
    {
        _controller.Animator.SetBool("Aiming", true);
    }

    public override void OnExit()
    {
        _controller.Animator.SetBool("Aiming", false);
    }
    public override void Update()
    {
        // Goto not aim state if not aiming
        if (_controller.AimVector.magnitude <=0.1)
        {
            _controller.SetState(CharacterStateEnum.Grounded);
            return;
        }

        var direction = 
            Quaternion.Euler(0, BrCamera.Instance.MainCamera.transform.eulerAngles.y - _controller.transform.eulerAngles.y, 0) 
            * _controller.MovVector;
        //var direction = _controller.MovVector;

        _controller.Animator.SetFloat("X", direction.x, AnimatorDampValue, Time.deltaTime);
        _controller.Animator.SetFloat("Y", direction.z, AnimatorDampValue, Time.deltaTime);

        _controller.MoveAndRotateToAim(MoveSpeed, RotationSpeed);
    }
}