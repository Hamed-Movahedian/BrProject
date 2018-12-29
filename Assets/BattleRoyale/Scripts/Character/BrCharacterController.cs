using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class BrCharacterController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Static fields

    public static BrCharacterController MasterCharacter;
    #endregion

    #region Public Fields

    public LayerMask EnviromentLayerMask;
    public RaycastHit GroundHitInfo;

    [Header("Look IK")]
    public bool LookIK = true;
    public Transform LookTarget;
    public float HeadRotationSpeed = 2;
    public bool LineOfSightGizmo = false;
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
    public Vector3 MovVector { get; set; }
    public Vector3 AimVector { get; set; }
    internal BrWeaponController WeaponController { get; set; }
    public bool IsAiming => CurrentState == CharacterStateEnum.GroundedAim;

    #endregion

    #region Privates

    private Dictionary<CharacterStateEnum, BrCharacterStateBase> _stateDic;
    #endregion

    // ********************** Methods

    #region Start/Awake

    private void Awake()
    {
        if (photonView.IsMine)
            MasterCharacter = this;

        var pos = JsonUtility.FromJson<Vector3>((string)photonView.Owner.CustomProperties["Pos"]);
        transform.position = new Vector3(pos.x * 17, 5, pos.y * 17);
    }

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
        if (photonView.IsMine)
            BrCamera.Instance.SetCharacter(this);

        // State Start
        if (photonView.IsMine)
            _stateDic.Values.ToList().ForEach(s => s.Start());
    }

    #endregion

    #region Updates
    void Update()
    {
        if (photonView.IsMine)
        {
            MovVector = BrUIController.Instance.MovementJoystick.Value3;
            AimVector = BrUIController.Instance.AimJoystick.Value3;

            GroundCheck();

            _stateDic[CurrentState].Update();
        }

    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
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

            //Move(moveSpeed, transform.forward * magnitude);
            //Move(moveSpeed, direction);
        }
        else
        {
            Rotate(rotationSpeed, transform.forward);

        }
    }

    public void MoveAndRotateToAim(float moveSpeed, float rotationSpeed)
    {
        Vector3 direction = Quaternion.Euler(0, 0 + BrCamera.Instance.MainCamera.transform.eulerAngles.y, 0) * MovVector;

        //Move(moveSpeed, direction);

        direction = Quaternion.Euler(0, 0 + BrCamera.Instance.MainCamera.transform.eulerAngles.y, 0) * AimVector;

        Rotate(rotationSpeed, direction);
    }

    private void Rotate(float rotationSpeed, Vector3 direction)
    {
        direction.y = 0;

        var lookRotation = Quaternion.LookRotation(direction);

        //Body rotation
        var bodyRotation = Quaternion.Lerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * rotationSpeed);

        var headRotation = Quaternion.Lerp(
            LookTarget.rotation,
            lookRotation,
            Time.deltaTime * HeadRotationSpeed);

        transform.rotation = bodyRotation;
        LookTarget.rotation = headRotation;

    }

    private void Move(float moveSpeed, Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }



    #endregion

    #region Gizmo
    private void OnDrawGizmos()
    {
        if (LineOfSightGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(LookTarget.position, LookTarget.position + LookTarget.forward * 3);

        }
    }
    #endregion

    #region FootStep
    private void FootStep() { }
    #endregion

    #region IK
    void OnAnimatorIK()
    {
        if (LookIK)
        {
            Animator.SetLookAtWeight(1);
            Animator.SetLookAtPosition(LookTarget.position + LookTarget.forward * 3);
        }
        else
        {
            Animator.SetLookAtWeight(0);
        }

    }

    #endregion

    #region Photon
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(MovVector);
            stream.SendNext(AimVector);
            stream.SendNext(CurrentState);
        }
        else
        {
            MovVector = (Vector3)stream.ReceiveNext();
            AimVector = (Vector3)stream.ReceiveNext();
            CurrentState = (CharacterStateEnum)stream.ReceiveNext();
/*            var state = (CharacterStateEnum)stream.ReceiveNext();
            if (state != CurrentState)
                SetState(state);*/
        }
    }
    #endregion
}