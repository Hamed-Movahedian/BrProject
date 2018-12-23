using System;
using System.Collections.Generic;
using UnityEngine;

public class BrWeaponController : MonoBehaviour
{
    #region Publics
    public Transform WeaponSlot;
    public Transform HolsterWeaponSlot;

    public float ArmIK = 1;
    public float AimingShotTreshould = 0.16f;
    #endregion

    #region properties

    #region Armed
    private bool _armed = false;
    public bool Armed
    {
        set
        {
            if (_armed != value)
            {
                CharacterController.Animator.SetBool("Armed", value);
                _targetArmIk = value ? 1 : 0;
            }
            _armed = value;
        }
        get { return _armed; }
    }
    #endregion

    public int CurrentWeaponIndex { get; set; } = -1;
    public int BulletCount { get; set; } = 0;

    public BrCharacterController CharacterController { get; set; }

    public BrWeapon CurrWeapon => CurrentWeaponIndex == -1 ? null : _weaponList[CurrentWeaponIndex];

    #endregion

    #region privates
    private BrWeapon[] _weaponList;
    private float _timeToNextShot = 0;
    private float _targetArmIk;

    #endregion

    // ************** Methods

    #region Pickup/Change/Holster weapon
    internal void PickWeapon(string weaponName)
    {
        if (CurrWeapon && CurrWeapon.name == weaponName)
        {
            AddBullets(CurrWeapon.InitialBullets);
            return;
        }

        for (int i = 0; i < _weaponList.Length; i++)
        {
            if (_weaponList[i].name == weaponName)
            {
                ChangeWeapon(i);
                BulletCount = CurrWeapon.InitialBullets;
                return;
            }
        }
    }

    private void ChangeWeapon(int index)
    {
        if (CurrWeapon)
            CurrWeapon.SetActive(false);

        CurrentWeaponIndex = index;

        if (CurrWeapon)
            CurrWeapon.SetActive(true);

        Armed = CurrWeapon != null;

    }

    private void HolsterWeapon()
    {
        Armed = false;
     
        CharacterController.Animator.SetTrigger("PutBackWeapon");
    }

    private void DetachWeapon()
    {
        CurrWeapon.transform.position = HolsterWeaponSlot.transform.position;
        CurrWeapon.transform.rotation = HolsterWeaponSlot.transform.rotation;
        CurrWeapon.transform.SetParent(HolsterWeaponSlot);
    }

    private void UnholsterWeapon()
    {
        CharacterController.Animator.SetTrigger("GrabWeapon");
    }

    private void AttachWeapon()
    {
        CurrWeapon.transform.position = WeaponSlot.transform.position;
        CurrWeapon.transform.rotation = WeaponSlot.transform.rotation;
        CurrWeapon.transform.SetParent(WeaponSlot);
    }
    private void EndGrabWeapon()
    {
        Armed = true;
    }
    #endregion

    #region Bullet
    private void AddBullets(int amount)
    {
        BulletCount += amount;

        if (CurrWeapon)
            if (BulletCount > CurrWeapon.MaxBullets)
                BulletCount = CurrWeapon.MaxBullets;
    }
    #endregion

    #region Start
    void Start()
    {
        CharacterController = GetComponent<BrCharacterController>();

        // Get weapon list
        _weaponList = WeaponSlot.GetComponentsInChildren<BrWeapon>();

        // Initialize weapons
        foreach (var weapon in _weaponList)
        {
            weapon.Initialize(this);
        }

        _targetArmIk = ArmIK;
    }
    #endregion

    #region Update
    void Update()
    {
        if (_timeToNextShot > 0)
            _timeToNextShot -= Time.deltaTime;

        if (CharacterController.AimVector.sqrMagnitude > AimingShotTreshould && Armed && BulletCount > 0)
        {
            if (_timeToNextShot <= 0f)
            {
                Fire();
            }
        }

        ArmIK = Mathf.Lerp(ArmIK, _targetArmIk, 5*Time.deltaTime);
    }

    #endregion

    #region Fire
    private void Fire()
    {
        BulletCount--;

        CurrWeapon.Fire();

        CharacterController.Animator.SetTrigger("Shoot");

        _timeToNextShot = CurrWeapon.FireRate;

        if (BulletCount <= 0)
            HolsterWeapon();
    }
    #endregion

    #region IK
    void OnAnimatorIK()
    {
        //if the IK is active, set the position and rotation directly to the goal. 
        if (CurrWeapon && CurrWeapon.ArmIK != null)
        {
            // Set the right hand target position and rotation, if one has been assigned
            CharacterController.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ArmIK);
            CharacterController.Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ArmIK);
            CharacterController.Animator.SetIKPosition(AvatarIKGoal.LeftHand, CurrWeapon.ArmIK.position);
            CharacterController.Animator.SetIKRotation(AvatarIKGoal.LeftHand, CurrWeapon.ArmIK.rotation);

        }
        else
        {
            CharacterController.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            CharacterController.Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            //_controller.Animator.SetLookAtWeight(0);
        }
    }

    #endregion

    #region Ammo
    public void PickupAmmo(int bulletCount)
    {
        if (BulletCount <= 0)
        {
            BulletCount += bulletCount;

            if (CurrWeapon != null)
                UnholsterWeapon();

        }
        else
            BulletCount += bulletCount;

        if (BulletCount > CurrWeapon.MaxBullets)
            BulletCount = CurrWeapon.MaxBullets;

    }

    public bool CanPickupAmmo()
    {
        return CurrWeapon != null;
    }

    #endregion
}