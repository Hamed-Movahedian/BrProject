using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrTopDownCharInventory : MonoBehaviour
{

    [Header("Stats")]

    public int Health = 100;
    [HideInInspector] public int ActualHealth = 100;

    [HideInInspector] public bool isDead = false;
    public bool DestroyOnDead = false;

    private bool Damaged = false;
    private float DamagedTimer = 0.0f;


    [Header("Weapons")]

    public int playerWeaponLimit = 2;

    public PrWeapon[] InitialWeapons;

    private float lastWeaponChange = 0.0f;
    [HideInInspector]
    public bool Armed = true;
    [HideInInspector]
    public GameObject[] Weapon;
    [HideInInspector]
    public int ActiveWeapon = 0;
    private bool CanShoot = true;
    public PrWeaponList WeaponListObject;
    private GameObject[] WeaponList;
    public Transform WeaponR;
    public Transform WeaponL;

    public bool aimingIK = false;
    public bool useArmIK = true;

    [HideInInspector] public bool Aiming = false;

    private float FireRateTimer = 0.0f;
    private float LastFireTimer = 0.0f;

    private Transform AimTarget;

    [Header("VFX")]
    public GameObject DamageFX;
    private Vector3 LastHitPos = Vector3.zero;
    public Renderer[] MeshRenderers;
    [Space]
    public GameObject damageSplatVFX;
    private PrBloodSplatter actualSplatVFX;
    [Space]
    public GameObject deathVFX;
    private GameObject actualDeathVFX;

    [Space]
    //Explosive Death VFX
    public bool useExplosiveDeath = true;
    private bool explosiveDeath = false;
    public int damageThreshold = 50;
    public GameObject explosiveDeathVFX;
    private GameObject actualExplosiveDeathVFX;


    [Header("Sound FX")]

    public float FootStepsRate = 0.4f;
    public float GeneralFootStepsVolume = 1.0f;
    public AudioClip[] Footsteps;
    private float LastFootStepTime = 0.0f;
    private AudioSource Audio;
 
    //Private References
    [HideInInspector]
    public PrTopDownCharController charController;
    [HideInInspector]
    public Animator charAnimator;

    //ArmIK
    private Transform ArmIKTarget = null;

    private PrCharacterIK CharacterIKController;


    // Use this for initialization
    void Start()
    {
        //Creates weapon array
        Weapon = new GameObject[playerWeaponLimit];

        //Load Weapon List from Scriptable Object
        WeaponList = WeaponListObject.weapons;

        ActualHealth = Health;

        Audio = GetComponent<AudioSource>() as AudioSource;
        charAnimator = GetComponent<Animator>();

        charController = GetComponent<PrTopDownCharController>();
        AimTarget = charController.AimFinalPos;

        //Weapon Instantiate and initialization
        if (InitialWeapons.Length > 0)
        {
            InstantiateWeapons();

            Armed = false;
            EquipWeapon(false);
        }
        else
        {
            Armed = false;
        }

        if (useExplosiveDeath && explosiveDeathVFX)
        {
            actualExplosiveDeathVFX = Instantiate(explosiveDeathVFX, transform.position, transform.rotation) as GameObject;
            actualExplosiveDeathVFX.SetActive(false);

            if (GameObject.Find("VFXBloodParent"))
                actualExplosiveDeathVFX.transform.parent = GameObject.Find("VFXBloodParent").transform;
            else
            {
                GameObject VFXParent = new GameObject("VFXBloodParent") as GameObject;
                actualExplosiveDeathVFX.transform.parent = VFXParent.transform;
            }
        }

        if (deathVFX)
        {
            actualDeathVFX = Instantiate(deathVFX, transform.position, transform.rotation) as GameObject;
            actualDeathVFX.SetActive(false);

            if (GameObject.Find("VFXBloodParent"))
                actualDeathVFX.transform.parent = GameObject.Find("VFXBloodParent").transform;
            else
            {
                GameObject VFXParent = new GameObject("VFXBloodParent") as GameObject;
                actualDeathVFX.transform.parent = VFXParent.transform;
            }
        }

        if (damageSplatVFX)
        {
            GameObject GOactualSplatVFX = Instantiate(damageSplatVFX, transform.position, transform.rotation) as GameObject;
            GOactualSplatVFX.transform.position = transform.position;
            GOactualSplatVFX.transform.parent = transform;
            actualSplatVFX = GOactualSplatVFX.GetComponent<PrBloodSplatter>();
        }

    }

    void InstantiateWeapons()
    {

        foreach (PrWeapon Weap in InitialWeapons)
        {
            int weapInt = 0;
            //Debug.Log("Weapon to instance = " + Weap);

            foreach (GameObject weap in WeaponList)
            {

                if (Weap.gameObject.name == weap.name)
                {
                    //Debug.Log("Weapon to pickup = " + weap + " " + weapInt);
                    PickupWeapon(weapInt);
                }

                else
                    weapInt += 1;
            }

        }

    }

    public void FootStep()
    {
        if (Footsteps.Length > 0 && Time.time >= (LastFootStepTime + FootStepsRate))
        {
            int FootStepAudio = 0;

            if (Footsteps.Length > 1)
            {
                FootStepAudio = Random.Range(0, Footsteps.Length);
            }

            float FootStepVolume = charAnimator.GetFloat("Speed") * GeneralFootStepsVolume;
            if (Aiming)
                FootStepVolume *= 0.5f;

            Audio.PlayOneShot(Footsteps[FootStepAudio], FootStepVolume);

            LastFootStepTime = Time.time;
        }
    }
    
    void EnableArmIK(bool active)
    {
        if (CharacterIKController && useArmIK)
            if (Weapon[ActiveWeapon].GetComponent<PrWeapon>().useIK)
                CharacterIKController.ikActive = active;
            else
                CharacterIKController.ikActive = false;
    }

    void MeleeEvent()
    {
        //this event comes from animation, the exact moment of HIT
        Weapon[ActiveWeapon].GetComponent<PrWeapon>().AttackMelee();

    }
    // Update is called once per frame
    void Update()
    {
        if (charController.Falling)
            return;

        if (Damaged && MeshRenderers.Length > 0)
        {
            DamagedTimer = Mathf.Lerp(DamagedTimer, 0.0f, Time.deltaTime * 10);

            if (Mathf.Approximately(DamagedTimer, 0.0f))
            {
                DamagedTimer = 0.0f;
                Damaged = false;
            }

            foreach (Renderer Mesh in MeshRenderers)
            {
                if (Mesh.material.HasProperty("_DamageFX"))
                    Mesh.material.SetFloat("_DamageFX", DamagedTimer);
            }

            foreach (SkinnedMeshRenderer SkinnedMesh in MeshRenderers)
            {
                if (SkinnedMesh.material.HasProperty("_DamageFX"))
                    SkinnedMesh.material.SetFloat("_DamageFX", DamagedTimer);
            }

        }

        if (!isDead)
        {
            // Equip Weapon
            if (Input.GetButtonUp(charController.playerCtrlMap[6]) && Aiming == false)
            {
                Armed = !Armed;

                EquipWeapon(Armed);
            }
            // Shoot Weapons
            if (Input.GetAxis(charController.playerCtrlMap[4]) >= 0.5f || Input.GetButton(charController.playerCtrlMap[15]))
            {
                if (CanShoot && Weapon[ActiveWeapon] != null && Time.time >= (LastFireTimer + FireRateTimer))
                {
                    //Melee Weapon
                    if (Weapon[ActiveWeapon].GetComponent<PrWeapon>().Type == global::PrWeapon.WT.Melee)
                    {
                        LastFireTimer = Time.time;
                        charAnimator.SetTrigger("AttackMelee");
                        charAnimator.SetInteger("MeleeType", Random.Range(0, 2));
                    }
                    //Ranged Weapon
                    else
                    {
                        if (Aiming)
                        {
                            LastFireTimer = Time.time;
                            Weapon[ActiveWeapon].GetComponent<PrWeapon>().Shoot();

                            if (Weapon[ActiveWeapon].GetComponent<PrWeapon>().ActualBullets > 0)
                                charAnimator.SetTrigger("Shoot");
                            //else
                            //WeaponReload();
                        }
                    }
                }
            }

            // Aim
            if (Input.GetButton(charController.playerCtrlMap[8]) || Mathf.Abs(Input.GetAxis(charController.playerCtrlMap[2])) > 0.3f || Mathf.Abs(Input.GetAxis(charController.playerCtrlMap[3])) > 0.3f)
            {
                Aiming = true;

                Weapon[ActiveWeapon].GetComponent<PrWeapon>().LaserSight.enabled = true;

                charAnimator.SetBool("RunStop", false);
                charAnimator.SetBool("Aiming", true);
            }
            //Stop Aiming
            else if (Input.GetButtonUp(charController.playerCtrlMap[8]) || Mathf.Abs(Input.GetAxis(charController.playerCtrlMap[2])) < 0.3f || Mathf.Abs(Input.GetAxis(charController.playerCtrlMap[3])) < 0.3f)
            {
                Aiming = false;
                charAnimator.SetBool("Aiming", false);
                Weapon[ActiveWeapon].GetComponent<PrWeapon>().LaserSight.enabled = false;
            }


            // Change Weapon
            if (Input.GetButtonDown(charController.playerCtrlMap[13]) || Input.GetAxis(charController.playerCtrlMap[9]) >= 0.5f || Input.GetAxis(charController.playerCtrlMap[16]) != 0f)
            {

                if (Time.time >= lastWeaponChange + 0.25f && Armed)
                {
                    ChangeWeapon();
                }

            }

        }

    }

    void LateUpdate()
    {
        if (aimingIK)
        {
            if (Aiming)
            {
                WeaponR.parent.transform.LookAt(AimTarget.position, Vector3.up);
            }
        }
    }

    void EquipWeapon(bool bArmed)
    {
        charAnimator.SetBool("Armed", bArmed);
        Weapon[ActiveWeapon].SetActive(bArmed);

        if (!bArmed)
        {
            int PistolLayer = charAnimator.GetLayerIndex("PistolLyr");
            charAnimator.SetLayerWeight(PistolLayer, 0.0f);
            int PistoActlLayer = charAnimator.GetLayerIndex("PistolActions");
            charAnimator.SetLayerWeight(PistoActlLayer, 0.0f);
            int RifleActlLayer = charAnimator.GetLayerIndex("RifleActions");
            charAnimator.SetLayerWeight(RifleActlLayer, 0.0f);
            int PartialActions = charAnimator.GetLayerIndex("PartialActions");
            charAnimator.SetLayerWeight(PartialActions, 0.0f);

            if (CharacterIKController)
                CharacterIKController.enabled = false;
        }
        else
        {
            if (Weapon[ActiveWeapon].GetComponent<PrWeapon>().Type == global::PrWeapon.WT.Pistol)
            {
                int PistolLayer = charAnimator.GetLayerIndex("PistolLyr");
                charAnimator.SetLayerWeight(PistolLayer, 1.0f);
                int PistoActlLayer = charAnimator.GetLayerIndex("PistolActions");
                charAnimator.SetLayerWeight(PistoActlLayer, 1.0f);
                int RifleActlLayer = charAnimator.GetLayerIndex("RifleActions");
                charAnimator.SetLayerWeight(RifleActlLayer, 0.0f);
            }
            else if (Weapon[ActiveWeapon].GetComponent<PrWeapon>().Type == global::PrWeapon.WT.Rifle)
            {
                int PistolLayer = charAnimator.GetLayerIndex("PistolLyr");
                charAnimator.SetLayerWeight(PistolLayer, 0.0f);
                int PistoActlLayer = charAnimator.GetLayerIndex("PistolActions");
                charAnimator.SetLayerWeight(PistoActlLayer, 0.0f);
                int RifleActlLayer = charAnimator.GetLayerIndex("RifleActions");
                charAnimator.SetLayerWeight(RifleActlLayer, 1.0f);
            }

            int PartAct = charAnimator.GetLayerIndex("PartialActions");
            charAnimator.SetLayerWeight(PartAct, 1.0f);

            if (CharacterIKController)
                CharacterIKController.enabled = true;
        }

        EnableArmIK(bArmed);

    }

    public void PickupWeapon(int WeaponType)
    {
        GameObject NewWeapon = Instantiate(WeaponList[WeaponType], WeaponR.position, WeaponR.rotation) as GameObject;
        NewWeapon.transform.parent = WeaponR.transform;
        NewWeapon.transform.localRotation = Quaternion.Euler(90, 0, 0);
        NewWeapon.name = "Player_" + NewWeapon.GetComponent<PrWeapon>().WeaponName;

        //New multi weapon system
        bool replaceWeapon = true;

        for (int i = 0; i < playerWeaponLimit; i++)
        {
            if (Weapon[i] == null)
            {
                //Debug.Log(i + " " + NewWeapon.name);

                Weapon[i] = NewWeapon;
                replaceWeapon = false;

                if (ActiveWeapon != i)
                {
                    // ChangeWeapon();
                    ChangeToWeapon(i);
                }
                break;
            }

        }
        if (replaceWeapon)
        {
            //Debug.Log("Replacing weapon" + Weapon[ActiveWeapon].name + " using " + NewWeapon.name);
            DestroyImmediate(Weapon[ActiveWeapon]);
            Weapon[ActiveWeapon] = NewWeapon;

        }

        InitializeWeapons();
    }

    void InitializeWeapons()
    {
        PrWeapon ActualW = Weapon[ActiveWeapon].GetComponent<PrWeapon>();
        Weapon[ActiveWeapon].SetActive(true);
        ActualW.ShootTarget = AimTarget;
        ActualW.Player = this.gameObject;
        FireRateTimer = ActualW.FireRate;

        //ArmIK
        if (useArmIK)
        {
            if (ActualW.gameObject.transform.Find("ArmIK"))
            {
                ArmIKTarget = ActualW.gameObject.transform.Find("ArmIK");
                if (GetComponent<PrCharacterIK>() == null)
                {
                    gameObject.AddComponent<PrCharacterIK>();
                    CharacterIKController = GetComponent<PrCharacterIK>();
                }
                else if (GetComponent<PrCharacterIK>())
                {
                    CharacterIKController = GetComponent<PrCharacterIK>();
                }

                if (CharacterIKController)
                {
                    CharacterIKController.leftHandTarget = ArmIKTarget;
                    CharacterIKController.ikActive = true;
                }

            }
            else
            {
                if (CharacterIKController != null)
                    CharacterIKController.ikActive = false;
            }
        }


        ActualW.Audio = WeaponR.GetComponent<AudioSource>();

        if (ActualW.Type == global::PrWeapon.WT.Pistol)
        {
            int PistolLayer = charAnimator.GetLayerIndex("PistolLyr");
            charAnimator.SetLayerWeight(PistolLayer, 1.0f);
            int PistolActLayer = charAnimator.GetLayerIndex("PistolActions");
            charAnimator.SetLayerWeight(PistolActLayer, 1.0f);
            charAnimator.SetBool("Armed", true);
        }
        else if (ActualW.Type == global::PrWeapon.WT.Rifle)
        {
            int PistolLayer = charAnimator.GetLayerIndex("PistolLyr");
            charAnimator.SetLayerWeight(PistolLayer, 0.0f);
            int PistolActLayer = charAnimator.GetLayerIndex("PistolActions");
            charAnimator.SetLayerWeight(PistolActLayer, 0.0f);
            charAnimator.SetBool("Armed", true);
        }
        else if (ActualW.Type == global::PrWeapon.WT.Melee)
        {
            int PistolLayer = charAnimator.GetLayerIndex("PistolLyr");
            charAnimator.SetLayerWeight(PistolLayer, 0.0f);
            int PistolActLayer = charAnimator.GetLayerIndex("PistolActions");
            charAnimator.SetLayerWeight(PistolActLayer, 0.0f);
            charAnimator.SetBool("Armed", true);
            //charAnimator.SetBool("Armed", false);
        }
        else if (ActualW.Type == global::PrWeapon.WT.Laser)
        {
            int PistolLayer = charAnimator.GetLayerIndex("PistolLyr");
            charAnimator.SetLayerWeight(PistolLayer, 0.0f);
            int PistolActLayer = charAnimator.GetLayerIndex("PistolActions");
            charAnimator.SetLayerWeight(PistolActLayer, 0.0f);
            charAnimator.SetBool("Armed", true);
        }
    }

    void ChangeToWeapon(int weaponInt)
    {
        lastWeaponChange = Time.time;

        int nextWeapon = weaponInt;

        //Debug.Log("Changing Weapon " + Weapon[ActiveWeapon]);

        //New Multiple weapon system
        if (Weapon[nextWeapon] != null)
        {
            //Debug.Log("Testing");
            foreach (GameObject i in Weapon)
            {
                if (i != null)
                {
                    i.GetComponent<PrWeapon>().LaserSight.enabled = false;
                    i.SetActive(false);
                }
                //Debug.Log("Deactivating Weapon " + Weapon[ActiveWeapon]);
            }
            //Debug.Log(ActiveWeapon + " " + nextWeapon);

            ActiveWeapon = nextWeapon;
            Weapon[ActiveWeapon].SetActive(true);

            InitializeWeapons();

        }
    }

    void ChangeWeapon()
    {
        lastWeaponChange = Time.time;

        int nextWeapon = ActiveWeapon + 1;
        if (nextWeapon >= playerWeaponLimit)
            nextWeapon = 0;

        //New Multiple weapon system
        if (Weapon[nextWeapon] != null)
        {
            //Debug.Log(ActiveWeapon + " " + nextWeapon);
            foreach (GameObject i in Weapon)
            {
                if (i != null)
                {
                    i.GetComponent<PrWeapon>().LaserSight.enabled = false;
                    i.SetActive(false);
                }
                //Debug.Log("Deactivating Weapon " + Weapon[ActiveWeapon]);
            }

            ActiveWeapon = nextWeapon;

            Weapon[ActiveWeapon].SetActive(true);

            InitializeWeapons();

        }
        else
        {
            for (int i = nextWeapon; i < playerWeaponLimit; i++)
            {
                if (Weapon[i] != null)
                {
                    ActiveWeapon = i - 1;
                    ChangeWeapon();
                    break;
                }
            }

            ActiveWeapon = playerWeaponLimit - 1;
            //Debug.Log(playerWeaponLimit);
            ChangeWeapon();

        }
    }

    public void BulletPos(Vector3 BulletPosition)
    {
        LastHitPos = BulletPosition;
        LastHitPos.y = 0;
    }

    public void SetHealth(int HealthInt)
    {
        ActualHealth = HealthInt;
    }

    void ApplyDamagePassive(int damage)
    {
        if (!isDead)
        {
            SetHealth(ActualHealth - damage);

            if (ActualHealth <= 0)
            {
                if (actualSplatVFX)
                    actualSplatVFX.transform.parent = null;

                Die(true);
            }
        }
    }

    public void ApplyDamage(int Damage)
    {
        if (ActualHealth > 0)
        {
            //Here you can put some Damage Behaviour if you want
            SetHealth(ActualHealth - Damage);

            Damaged = true;
            DamagedTimer = 1.0f;

            if (actualSplatVFX)
            {
                actualSplatVFX.transform.LookAt(LastHitPos);
                actualSplatVFX.Splat();
            }

            if (ActualHealth <= 0)
            {
                if (actualSplatVFX)
                    actualSplatVFX.transform.parent = null;
                if (Damage >= damageThreshold)
                    explosiveDeath = true;
                Die(false);
            }

        }

    }

    public void ApplyDamageNoVFX(int Damage)
    {
        if (ActualHealth > 0)
        {
            //Here you can put some Damage Behaviour if you want
            SetHealth(ActualHealth - Damage);

            Damaged = true;
            DamagedTimer = 1.0f;

            if (ActualHealth <= 0)
            {
                if (actualSplatVFX)
                    actualSplatVFX.transform.parent = null;
                if (Damage >= damageThreshold)
                    explosiveDeath = true;
                Die(true);
            }

        }

    }


    public void Die(bool temperatureDeath)
    {
        EnableArmIK(false);
        isDead = true;
        charAnimator.SetBool("Dead", true);

        charController.m_isDead = true;
        Weapon[ActiveWeapon].GetComponent<PrWeapon>().TurnOffLaser();

        //Set invisible to Bullets
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;

        this.tag = "Untagged";
        charController.playerSelection.enabled = false;

        //Send Message to Game script to notify Dead
        SendMessageUpwards("PlayerDied", 1, SendMessageOptions.DontRequireReceiver);

        if (explosiveDeath && actualExplosiveDeathVFX)
        {
            actualExplosiveDeathVFX.transform.position = transform.position;
            actualExplosiveDeathVFX.transform.rotation = transform.rotation;
            actualExplosiveDeathVFX.SetActive(true);
            actualExplosiveDeathVFX.SendMessage("SetExplosiveForce", LastHitPos + new Vector3(0, 1.5f, 0), SendMessageOptions.DontRequireReceiver);

            Destroy(this.gameObject);
        }

        else
        {
            if (deathVFX && actualDeathVFX)
            {
                if (temperatureDeath)
                {
                    //Freezing of Burning Death VFX...
                }
                else
                {
                    actualDeathVFX.transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
                    actualDeathVFX.transform.LookAt(LastHitPos);
                    actualDeathVFX.transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);

                    actualDeathVFX.SetActive(true);

                    ParticleSystem[] particles = actualDeathVFX.GetComponentsInChildren<ParticleSystem>();

                    if (particles.Length > 0)
                    {
                        foreach (ParticleSystem p in particles)
                        {
                            p.Play();
                        }
                    }
                }
            }

        }

        Destroy(charController);
        Destroy(GetComponent<Collider>());
        //Destroy(this);
    }

}
