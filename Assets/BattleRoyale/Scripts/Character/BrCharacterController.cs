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

    [Header("Stat")]
    public int MaxHealth = 100;
    public int MaxShield = 100;

    [Header("Look IK")]
    public bool LookIK = true;
    public Transform LookTarget;
    public float HeadRotationSpeed = 2;
    public bool LineOfSightGizmo = false;

    [Header("Camera")]
    public Transform CameraTarget;

    [HideInInspector]
    public Profile profile;

    #endregion

    #region States

    [Header("States")]

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
    public bool isMine => photonView.IsMine;

    public Vector3 CameraTargetPos => CameraTarget.position;
    public bool IsAlive => Health > 0;

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
            OnStatChange(this);
        }
    }

    public int Shield
    {
        get
        {
            return shield;
        }

        set
        {
            shield = value;
            OnStatChange(this);
        }
    }

    public bool NeedHealth => Health < MaxHealth;

    #endregion

    #region Privates

    private Dictionary<CharacterStateEnum, BrCharacterStateBase> _stateDic;
    private BrCharacterModel _characterModel;
    private BrCharacterHitEffect hitEffect;
    private int health;
    private int shield;



    #endregion

    #region Events
    public delegate void PlayerStatChangeDelegate(BrCharacterController player);
    public PlayerStatChangeDelegate OnStatChange;
    #endregion
    // ********************** Methods

    #region Start/Awake

    private void Awake()
    { 
        if (photonView.IsMine)
            MasterCharacter = this;


    }

    void Start()
    {
        #region Get necessary components
        RigidBody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        Animator = GetComponent<Animator>();
        WeaponController = GetComponent<BrWeaponController>();
        hitEffect = GetComponent<BrCharacterHitEffect>();
        #endregion

        #region Get custom properties
        var pos = JsonUtility.FromJson<Vector3>((string)photonView.Owner.CustomProperties["Pos"]);

        profile = Profile.Deserialize((string)photonView.Owner.CustomProperties["Profile"]);

        _characterModel = GetComponent<BrCharacterModel>();
        _characterModel.SetProfile(profile);

        transform.position = BrLevelCoordinator.instance.GetLevelPos(pos);
        #endregion

        // root motion off for non locals
        Animator.applyRootMotion = photonView.IsMine;

        #region Create state dictionary
        _stateDic = new Dictionary<CharacterStateEnum, BrCharacterStateBase>()
        {
            {CharacterStateEnum.Falling, FallingState},
            {CharacterStateEnum.Parachute, ParachuteState},
            {CharacterStateEnum.Grounded, GroundedState},
            {CharacterStateEnum.GroundedAim, GroundedAimState}
        };
        #endregion

        // Initialize states
        _stateDic.Values.ToList().ForEach(s => s.Initialize(this));

        // Register to camera
        BrPlayerTracker.instance.RegisterPlayer(this);

        // set player stat
        Health = MaxHealth;
        Shield = 0;

        // State Start
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
        }

        GroundCheck();

        _stateDic[CurrentState].Update();
    }

    private void FixedUpdate()
    {
        //if (photonView.IsMine)
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
                    transform.position = GroundHitInfo.point;
                GroundDistance = 0;
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
        if (!isMine)
            return;
        var magnitude = MovVector.magnitude;

        if (magnitude > 0.1)

        {
            Vector3 direction = Quaternion.Euler(0, 0 + BrCamera.Instance.MainCamera.transform.eulerAngles.y, 0) * MovVector;


            Rotate(rotationSpeed, direction);

            if (moveSpeed > 0)
                Move(moveSpeed, direction);
        }
        else
        {
            Rotate(rotationSpeed, transform.forward);

        }
    }

    public void MoveAndRotateToAim(float moveSpeed, float rotationSpeed)
    {
        if (!isMine)
            return;
        Vector3 direction = Quaternion.Euler(0, 0 + BrCamera.Instance.MainCamera.transform.eulerAngles.y, 0) * MovVector;

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
            stream.SendNext(Health);
            stream.SendNext(CurrentState);
        }
        else
        {
            MovVector = (Vector3)stream.ReceiveNext();
            AimVector = (Vector3)stream.ReceiveNext();
            Health = (int) stream.ReceiveNext();
            var state = (CharacterStateEnum)stream.ReceiveNext();
            if (state != CurrentState)
                SetState(state);
        }
    }
    #endregion

    #region TakeDamage
    internal void TakeDamage(int bulletDamage, Vector3 bulletDir, BrCharacterController killer, string weaponName)
    {
        photonView.RPC(
            "TakeDamageRpc",
            RpcTarget.All,
            bulletDamage,
            bulletDir,
            killer ? killer.photonView.ViewID : -1,
            weaponName
            );
    }

    [PunRPC]
    public void TakeDamageRpc(int damage, Vector3 bulletDir,int killerViewID, string weaponName)
    {
        if (Health <= 0)
            return;

        hitEffect.Hit();
        if(Shield>0)
        {
            Shield -= damage;
            if(Shield<0)
            {
                Health += Shield;
                Shield = 0;
            }
        }
        else
            Health -= damage;

        if (Health <= 0)
            Death(killerViewID,weaponName);
    }

    #endregion

    #region Death
    private void Death(int killerViewID, string weaponName)
    {
        enabled = false;
        WeaponController.enabled = false;
        Animator.SetTrigger("Dead");
        CapsuleCollider.enabled = false;

        BrPlayerTracker.instance.PlayerDead(photonView.ViewID, killerViewID, weaponName);

        if(killerViewID!=-1)
            ShowFlag(killerViewID);

        if (isMine)
        {

            BrUIController.Instance.SetMovementJoyisticActive(false);
            BrUIController.Instance.SetAimJoyisticActive(false);
        }
    }

    private void ShowFlag(int killerViewID)
    {
        var flag = GetComponentInChildren<Flag>(true);

        var killerProfile = PhotonNetwork.GetPhotonView(killerViewID).GetComponent<BrCharacterController>().profile;
        flag.flagsList.Flags[killerProfile.CurrentFlag].SetToFlag(flag);
        flag.transform.SetParent(null);
        flag.transform.eulerAngles = new Vector3(-90, 0, 45);
        flag.gameObject.SetActive(true);
    }
    #endregion

    #region Health/shield
    public void AddHealth(int health)
    {
        Health += health;
        if (Health > MaxHealth)
            Health = MaxHealth;
    }
    internal void AddShield(int sheild)
    {
        Shield += sheild;

        if (Shield > MaxShield)
            Shield = MaxShield;
    }
    #endregion
}