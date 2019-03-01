using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BrCharacterController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Static fields

    public static BrCharacterController MasterCharacter;

    #endregion

    #region Public Fields

    [Header("Stat")] 
    public int MaxHealth = 100;
    public int MaxShield = 100;

    [Header("Look IK")] public bool LookIK = true;
    public Transform LookTarget;
    public float HeadRotationSpeed = 2;
    public bool LineOfSightGizmo = false;

    [Header("Camera")] public Transform CameraTarget;

    [HideInInspector] public Profile profile;
    [HideInInspector] public BrCharacterHitEffect hitEffect;

    #endregion

    #region States

    [Header("States")] 
    public BrFallingCharacterState FallingState;
    public BrParachuteCharacterState ParachuteState;
    public BrGroundedCharacterState GroundedState;
    public BrGroundedAimCharacterState GroundedAimState;

    #endregion

    #region Properties

    public CharacterStateEnum CurrentState { get; set; }

    public Animator Animator { get; set; }
    public NavMeshAgent NavMeshAgent { get; set; }

    public Rigidbody RigidBody { get; set; }
    public CapsuleCollider CapsuleCollider { get; set; }
    public BrCharacterModel characterModel { get; set; }
    public Vector3 MovVector { get; set; }
    public Vector3 AimVector { get; set; }
    public BrWeaponController WeaponController { get; set; }

    public bool isMine => photonView.IsMine;

    public bool IsAlive => Health > 0;

    #region Health

    public BrUIBar.UpdateBar OnHealthChange;
    private int health;
    public int Health
    {
        get => health;

        set
        {
            if (health == value) 
                return;

            health = value.Between(0, MaxHealth);
            
            OnHealthChange?.Invoke(health);
            OnStatChange?.Invoke(this);
        }
    }

    #endregion

    #region Shield 

    public BrUIBar.UpdateBar OnShieldChange;
    private int shield;

    public int Shield
    {
        get => shield;

        set
        {
            if (shield == value)
                return;

            shield = value.Between(0, MaxShield);
            
            OnStatChange?.Invoke(this);
            OnShieldChange?.Invoke(shield);
        }
    }

    #endregion

    public bool NeedHealth => Health < MaxHealth;
    public bool NeedShield => Shield < MaxShield;

    public string UserID => profile.UserID;

    #endregion

    #region Privates

    
    private Dictionary<CharacterStateEnum, BrCharacterStateBase> _stateDic;
    

    #endregion

    #region Events

    public delegate void PlayerStatChangeDelegate(BrCharacterController player);

    public PlayerStatChangeDelegate OnStatChange;

    public delegate void TakeDamageDelegate(int amount, int type);

    public TakeDamageDelegate OnTakeDamage;

    public UnityEvent OnDead;
    private bool _isInitialized;

    #endregion

    // ********************** Methods

    #region Initialize

   
    void OnEnable()
    {
        if (!_isInitialized) 
            Initialize();
    }

    void Start()
    {
        if (!_isInitialized) 
            Initialize();
    }

    private void Initialize()
    {
        _isInitialized = true;

        #region Get necessary components

        RigidBody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        Animator = GetComponent<Animator>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        WeaponController = GetComponent<BrWeaponController>();
        hitEffect = GetComponent<BrCharacterHitEffect>();
        characterModel = GetComponent<BrCharacterModel>();

        #endregion

        if (photonView.IsMine)
        {
            if(MasterCharacter==null)
                MasterCharacter = this;
            
        }
        else
            NavMeshAgent.enabled = false;

        #region Set Profile

        profile = Profile.Deserialize((string) photonView.Owner.CustomProperties["Profile"]);

        characterModel.SetProfile(profile);

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

        // set player stat
        Health = MaxHealth;
        Shield = 0;

        // State Start
        _stateDic.Values.ToList().ForEach(s => s.Start());

        CurrentState = CharacterStateEnum.Falling;
        _stateDic[CurrentState].OnEnter();

        // Register player
        BrPlayerTracker.Instance.RegisterPlayer(this);
    }

    #endregion

    #region Updates

    void Update()
    {
        _stateDic[CurrentState]?.Update();
    }

    private void FixedUpdate()
    {
        _stateDic[CurrentState]?.FixedUpdate();
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
            Rotate(rotationSpeed, MovVector);

            if (moveSpeed > 0)
                Move(moveSpeed, MovVector);
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

        Rotate(rotationSpeed, AimVector);
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
            stream.SendNext(Shield);
            stream.SendNext(CurrentState);
        }
        else
        {
            MovVector = (Vector3) stream.ReceiveNext();
            AimVector = (Vector3) stream.ReceiveNext();
            Health = (int) stream.ReceiveNext();
            Shield = (int) stream.ReceiveNext();
            var state = (CharacterStateEnum) stream.ReceiveNext();
            if (state != CurrentState)
                SetState(state);
        }
    }

    #endregion

    #region OnAnimatorMove

    private void OnAnimatorMove()
    {
        if(NavMeshAgent.updatePosition)
            NavMeshAgent.Move(Animator.deltaPosition);
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
    public void TakeDamageRpc(int damage, Vector3 bulletDir, int killerViewID, string weaponName)
    {
        if (Health <= 0)
            return;

        hitEffect.Hit();

        var killer = killerViewID == -1
            ? null
            : PhotonNetwork.GetPhotonView(killerViewID).GetComponent<BrCharacterController>();

        if (isMine || (killer && killer.isMine))
            OnTakeDamage(damage, Shield > 0 ? 0 : 1);

        if (Shield > 0)
            Shield -= damage;
        else
            Health -= damage;

        if (Health <= 0)
            Death(killerViewID, weaponName);
    }

    #endregion

    #region Death

    private void Death(int killerViewID, string weaponName)
    {
        enabled = false;
        WeaponController.enabled = false;
        Animator.SetTrigger("Dead");
        CapsuleCollider.enabled = false;
        RigidBody.useGravity = false;
        RigidBody.isKinematic = true;
        

        OnDead.Invoke();

        BrPlayerTracker.Instance.PlayerDead(photonView.ViewID, killerViewID, weaponName);

        if (killerViewID != -1)
            ShowFlag(killerViewID);
        
        
    }

    public void EndDieing()
    {
        gameObject.SetActive(false);
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

    #region OpenPara / Landing

    public void OpenPara()
    {
        OpenParaRpc();
        photonView.RPC("OpenParaRpc", RpcTarget.Others);
    }

    [PunRPC]
    public void OpenParaRpc()
    {
        SetState(CharacterStateEnum.Parachute);
    }

    public void Land()
    {
        LandRPC();
        photonView.RPC("LandRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void LandRPC()
    {
        SetState(CharacterStateEnum.Grounded);
        enabled = false;
        Invoke(nameof(EndLandig), 0.5f);
    }

    private void EndLandig()
    {
        enabled = true;
    }

    #endregion
}