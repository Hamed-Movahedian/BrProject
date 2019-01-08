using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]

public class PrTopDownCharController : MonoBehaviour
{

    public Renderer playerSelection;
    //Inputs
    [HideInInspector]
    public string[] playerCtrlMap = {"Horizontal", "Vertical", "LookX", "LookY","FireTrigger", "Reload",
        "EquipWeapon", "Sprint", "Aim", "ChangeWTrigger", "Roll", "Use", "Crouch", "ChangeWeapon", "Throw"  ,"Fire", "Mouse ScrollWheel"};

    [Header("Movement")]
    [SerializeField] float m_JumpPower = 12f;
    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [HideInInspector]
    public float m_MoveSpeedSpecialModifier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    private float m_GroundCheckDistance = 0.25f;

    public float PlayerRunSpeed = 1f;
    public float PlayerAimSpeed = 1f;
    public float PlayerCrouchSpeed = 0.75f;
    public float PlayerFallingSpeed = 0.75f;

    public float RunRotationSpeed = 100f;
    public float AnimatorSprintDampValue = 0.2f;
    public float AnimatorAimingDampValue = 0.1f;

    Rigidbody m_Rigidbody;
    Animator charAnimator;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    float m_TurnAmount;
    float m_ForwardAmount;
    bool m_Crouching;
    private bool crouch = false;

    public bool Falling = false;

    [HideInInspector] public bool m_isDead = false;
    [HideInInspector] public bool m_CanMove = true;

    [Header("Aiming")]
    public GameObject AimTargetVisual;
    public Transform AimFinalPos;
    public PrTopDownCamera CamScript;

    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    [HideInInspector]
    private Vector3 smoothMove;

    private PrTopDownCharInventory Inventory;
    private Vector3 _move;
    public float ParachuteUpForce=4;
    public float ParachuteStartUpForce=100;

    public UnityEvent OnStartFalling;
    public UnityEvent OnOpenPara;
    public UnityEvent OnLand;
    public bool IsParaOpen=false;
    public float ParaDistance=10;

    void Start()
    {
        Inventory = GetComponent<PrTopDownCharInventory>();

        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = CamScript.transform.GetComponentInChildren<Camera>().transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
        }

        charAnimator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
        StartFalling();

    }

    // Update is called once per frame
    void Update()
    {
        MouseTargetPos();

        if (!m_isDead && m_CanMove)
        {
            float h = Input.GetAxis(playerCtrlMap[0]);
            float v = Input.GetAxis(playerCtrlMap[1]);

            if (Falling && !IsParaOpen)
                h = v = 0;

            _move = new Vector3(h, 0, v);


            if (Inventory.Aiming)
                MouseAim(AimFinalPos.position);
            else
                RunningLook(_move);

            _move = _move.normalized * m_MoveSpeedSpecialModifier;
            //Rotate move in camera space
            _move = Quaternion.Euler(0, 0 - transform.eulerAngles.y + m_Cam.transform.parent.transform.eulerAngles.y,
                       0) * _move;
            //Move Player
            Move(_move, crouch, Falling);

        }
        else
        {
            m_ForwardAmount = 0.0f;
            m_TurnAmount = 0.0f;
            Inventory.Aiming = false;
            UpdateAnimator(Vector3.zero);
        }

    }

    public void Crounch(bool isCounching)
    {
        crouch = isCounching;
    }

    private void StartFalling()
    {
        IsParaOpen = false;
        Falling = true;
        IsParaOpen = false;
        charAnimator.SetTrigger("StartFalling");
        OnStartFalling.Invoke();
    }
    private void OpenPara()
    {
        if (!Falling)
            return;
        IsParaOpen = true;
        charAnimator.SetTrigger("OpenPara");
        OnOpenPara.Invoke();
        m_Rigidbody.AddForce(Vector3.up * ParachuteStartUpForce, ForceMode.Acceleration);

    }
    private void EndJump()
    {
        IsParaOpen = false;
        Falling = false;
        CamScript.SetTargetFollowSpeed(CamScript.FollowSpeed * 4);
        OnLand.Invoke();
    }

    private void RunningLook(Vector3 Direction)
    {
        if (Falling && !IsParaOpen)
            return;
        if (Direction.magnitude >= 0.25f)
        {
            Direction = Quaternion.Euler(0, 0 + m_Cam.transform.parent.transform.eulerAngles.y, 0) * Direction;

            if (Falling)
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(Direction),
                    Time.deltaTime * (RunRotationSpeed * 0.03f));
            else
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(Direction),
                    Time.deltaTime * (RunRotationSpeed * 0.1f));

            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }

    }

    private void MouseTargetPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2000f, 9))
        {
            Vector3 FinalPos = new Vector3(hit.point.x, 0, hit.point.z);

            AimTargetVisual.transform.position = FinalPos;
            AimTargetVisual.transform.LookAt(transform.position);

        }
    }

    private void MouseAim(Vector3 FinalPos)
    {
        if (Falling && !IsParaOpen)
            return;

        transform.transform.LookAt(FinalPos);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    public void Move(Vector3 move, bool crouch, bool jump)
    {

        CheckGroundStatus();

        
        m_TurnAmount = move.x;
        m_ForwardAmount = move.z;
        
        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }
    
    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        charAnimator.SetFloat("Y", m_ForwardAmount, AnimatorAimingDampValue, Time.deltaTime);
        charAnimator.SetFloat("X", m_TurnAmount, AnimatorAimingDampValue, Time.deltaTime);

        charAnimator.SetFloat("Speed", move.magnitude, AnimatorSprintDampValue, Time.deltaTime);

        charAnimator.SetBool("Crouch", m_Crouching);
        charAnimator.SetBool("OnGround", m_IsGrounded);

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (move.magnitude > 0 && !Falling)
        {

            if (Inventory.Aiming)
            {
                move *= PlayerAimSpeed;
                transform.Translate(move * Time.deltaTime);
                //charAnimator.applyRootMotion = false;
            }
            else
            {
                if (crouch)
                {
                    move *= PlayerCrouchSpeed;
                }
                else
                {
                    move *= PlayerRunSpeed;
                }

                transform.Translate(move * Time.deltaTime);
                //charAnimator.applyRootMotion = false;
            }

            charAnimator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            charAnimator.speed = 1;
        }
    }

    private void FixedUpdate()
    {
        if(m_IsGrounded)
            return;

        _move = transform.TransformVector(_move*PlayerFallingSpeed);
        
        m_Rigidbody.MovePosition(m_Rigidbody.position+_move);

        if(IsParaOpen)
            m_Rigidbody.velocity=Vector3.down*ParachuteUpForce;
            //m_Rigidbody.AddForce(Vector3.up*ParachuteUpForce,ForceMode.Acceleration);
    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }
    
    //This function it´s used only for Aiming and Jumping states. Those anims doesn´t have root motion so we move the player by script
    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        //if (m_IsGrounded && Time.deltaTime > 0)
        if (Time.deltaTime > 0)
        {
            Vector3 v = (charAnimator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 1f), Vector3.down, out hitInfo))
        {
            if(hitInfo.distance<1.2f)
            {
                if (!m_IsGrounded)
                {
                    m_Rigidbody.position = hitInfo.point;
                    m_Rigidbody.velocity = Vector3.zero;

                }

                m_IsGrounded = true;
            }
            else
            {
                if (hitInfo.distance < ParaDistance)
                {
                    if(!IsParaOpen)
                        OpenPara();
                }
                m_IsGrounded = false;
            }

            playerSelection.transform.position = hitInfo.point+Vector3.up*0.1f;
            if(IsParaOpen)
                CamScript.TargetToFollow.position = (transform.position + hitInfo.point) / 2;
            else
            {
                CamScript.TargetToFollow.position = transform.position;
            }
        }
        else
        {
            m_IsGrounded = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnvZone"))
        {
            CamScript.TargetHeight = other.GetComponent<PrEnvironmentZone>().CameraHeight;
        }
    }
}
