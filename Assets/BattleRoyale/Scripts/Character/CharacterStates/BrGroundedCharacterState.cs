using System;
using UnityEngine;

[Serializable]
public class BrGroundedCharacterState : BrCharacterStateBase
{
    public float RotationSpeed = 90;
    public float MoveSpeed = 5;
    public float AnimatorDampValue = 0.2f;

    public override void OnExit()
    {
        _controller.Animator.SetFloat("Speed", 0);
    }

    public override void Update()
    {
        // Goto aim state if aiming
        if (_controller.AimVector.magnitude > 0.1)
        {
            _controller.SetState(CharacterStateEnum.GroundedAim);
            return;
        }

        _controller.Animator.SetFloat("Speed", _controller.MovVector.magnitude * 1.3f,AnimatorDampValue,Time.deltaTime);

        _controller.MoveAndRotate(MoveSpeed, RotationSpeed);
    }
}