using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BrCharacterController : MonoBehaviour
{
    #region Public Fields

    public LayerMask EnviromentLayerMask;
    public RaycastHit GroundHitInfo;

    #endregion

    #region States
    public BrFallingCharacterState FallingState;
    public BrParachuteCharacterState ParachuteState;
    public BrGroundedCharacterState GroundedState;
    public BrGroundedAimCharacterState GroundedAimState;
    #endregion

    #region Properties
    public CharacterStateEnum CurrentState { get; private set; }

    public Animator Animator { get; set; }
    public Rigidbody RigidBody { get; set; }
    public CapsuleCollider CapsuleCollider { get; set; }
    public bool IsGrounded { get; set; }
    public float GroundDistance { get; set; }
    public Vector3 MovVector => BrUIController.Instance.MovementJoystick.Value3;
    public Vector3 AimVector => BrUIController.Instance.AimJoystick.Value3;
    internal BrWeaponController WeaponController { get; set; }

    #endregion

    #region Privates

    private Dictionary<CharacterStateEnum, BrCharacterStateBase> _stateDic;

    #endregion

    // ********************** Methods

    #region Start
    void Start()
    {
        #region Get necessary components
        RigidBody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        Animator = GetComponent<Animator>();
        WeaponController = GetComponent<BrWeaponController>();
        #endregion

        #region Create state dictionary
        _stateDic = new Dictionary<CharacterStateEnum, BrCharacterStateBase>()
        {
            {CharacterStateEnum.Falling, FallingState},
            {CharacterStateEnum.Parachute, ParachuteState},
            {CharacterStateEnum.Grounded, GroundedState},
            {CharacterStateEnum.GroundedAim, GroundedAimState}
        };
        #endregion

        // Initialize
        _stateDic.Values.ToList().ForEach(s => s.Initialize(this));

        // Register to camera
        BrCamera.Instance.SetCharacter(this);

        // State Start
        _stateDic.Values.ToList().ForEach(s => s.Start());
    }

    #endregion

    #region Updates
    void Update()
    {
        GroundCheck();

        _stateDic[CurrentState].Update();
    }

    private void FixedUpdate()
    {
        _stateDic[CurrentState].FixedUpdate();
    }

    #endregion

    #region GroundCheck

    public void GroundCheck()
    {
        IsGrounded = false;


        if (Physics.Raycast(
            transform.position + Vector3.up * CapsuleCollider.height,
            Vector3.down,
            out GroundHitInfo,
            Mathf.Infinity,
            EnviromentLayerMask))
        {
            GroundDistance = GroundHitInfo.distance - CapsuleCollider.height;

            if (GroundDistance < 0.2f)
            {
                IsGrounded = true;
                if (GroundDistance < 0)
                    RigidBody.MovePosition(GroundHitInfo.point);
            }
        }
        else
            Debug.LogError("No Ground Detected!!!");
    }

    #endregion

    #region SetState
    public void SetState(CharacterStateEnum state)
    {
        _stateDic[CurrentState].OnExit();
        CurrentState = state;
        _stateDic[CurrentState].OnEnter();
    }

    #endregion

    #region Move and rotate
    public void MoveAndRotate(float moveSpeed, float rotationSpeed)
    {

        var magnitude = MovVector.magnitude;

        if (magnitude > 0)

        {
            Vector3 direction = Quaternion.Euler(0, 0 + BrCamera.Instance.MainCamera.transform.eulerAngles.y, 0) * MovVector;

            Rotate(rotationSpeed, direction);

            Move(moveSpeed, transform.forward * magnitude);
        }
    }

    public void MoveAndRotateToAim(float moveSpeed, float rotationSpeed)
    {
        Vector3 direction = Quaternion.Euler(0, 0 + BrCamera.Instance.MainCamera.transform.eulerAngles.y, 0) * MovVector;

        Move(moveSpeed, direction);

        direction = Quaternion.Euler(0, 0 + BrCamera.Instance.MainCamera.transform.eulerAngles.y, 0) * AimVector;

        Rotate(rotationSpeed, direction);
    }

    private void Rotate(float rotationSpeed, Vector3 direction)
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * rotationSpeed);

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    private void Move(float moveSpeed, Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    #endregion

    #region FootStep
    private void FootStep() { }
    #endregion

}