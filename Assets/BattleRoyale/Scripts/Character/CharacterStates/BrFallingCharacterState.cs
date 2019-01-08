using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BrFallingCharacterState : BrCharacterStateBase
{
    public bool FallingOnStart = true;
    public float FallingSpeed = 70;
    public float FallingHeight = 300;

    public UnityEvent OnStartFalling;
    public override void Start()
    {
        base.Start();

        // check current height
        _controller.GroundCheck();

        _controller.Animator.SetBool("FallingOnStart", FallingOnStart);
        _controller.Animator.SetBool("OnGround", !FallingOnStart);

        if (FallingOnStart)
        {
            // set falling state
            _controller.SetState(CharacterStateEnum.Falling);

            // Falling height
            _controller.transform.position = _controller.transform.position + Vector3.up * (FallingHeight - _controller.GroundDistance);

            // Start Falling speed
            _controller.RigidBody.velocity = Vector3.down * FallingSpeed;

            // Force camera to state
            if (isMine)
                BrCamera.Instance.ForceToState(CharacterStateEnum.Falling);

            // Disable joysticks
            if (isMine)
            {
                BrUIController.Instance.SetMovementJoyisticActive(false);
                BrUIController.Instance.SetAimJoyisticActive(false);
            }
        }
        else
        {
            // set grounded state
            _controller.SetState(CharacterStateEnum.Grounded);

            // put on ground
            _controller.RigidBody.MovePosition(_controller.transform.position + Vector3.down * _controller.GroundDistance);

            // set velocity to zero
            _controller.RigidBody.velocity = Vector3.zero;

            // Force camera to state
            if (isMine)
                BrCamera.Instance.ForceToState(CharacterStateEnum.Grounded);

            // Enable Joysticks
            if (isMine)
            {
                BrUIController.Instance.SetMovementJoyisticActive(true);
                BrUIController.Instance.SetAimJoyisticActive(true);
            }
        }
    }

    public override void Update()
    {
        if (_controller.GroundDistance < _controller.ParachuteState.OpenParaDistance)
            _controller.SetState(CharacterStateEnum.Parachute);
        else
        {
            if(_controller.isMine)
            _controller.transform.position += Vector3.down * FallingSpeed * Time.deltaTime;

            //FallingSpeed += 10 * Time.deltaTime;
        }
    }
    public override void OnEnter()
    {
        if (!_controller.isMine)
            _controller.gameObject.SetActive(false);
            OnStartFalling.Invoke();
    }
}
