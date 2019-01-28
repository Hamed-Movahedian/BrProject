using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BrWeaponController : MonoBehaviourPunCallbacks, IPunObservable
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
                if (IsMine)
                    CharacterController.Animator.SetBool("Armed", value);

                _targetArmIk = value ? 1 : 0;
            }
            _armed = value;
        }
        get { return _armed; }
    }
    #endregion

    private int _currentWeaponIndex=-1;
    public int CurrentWeaponIndex
    {
        get
        {
            return _currentWeaponIndex;
        }
        set
        {
            _currentWeaponIndex = value;
            OnStatChange?.Invoke(this);
        }
    }
    private int _bulletCount;
    public int BulletCount
    {
        get
        {
            return _bulletCount;
        }
        set
        {
            _bulletCount = value;
            OnStatChange?.Invoke(this);
        }
    }
    public BrCharacterController CharacterController { get; set; }

    public BrWeapon CurrWeapon
    {
        get
        {
            return CurrentWeaponIndex == -1 ? 
                null :
                _weaponList!=null ? _weaponList[CurrentWeaponIndex] : null;
        }
    }

    public bool IsMine => photonView.IsMine;
    #endregion

    #region privates
    private BrWeapon[] _weaponList;
    private float _timeToNextShot = 0;
    private float _targetArmIk;

    #endregion

    #region Events
    public delegate void WeaponStatChangeDelegate(BrWeaponController weapon);
    public WeaponStatChangeDelegate OnStatChange;
    #endregion
    // ************** Methods

    #region Pickup/Change/Holster weapon
    internal void PickupWeapon(string weaponName)
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

        CurrentWeaponIndex = -1;

        // Initialize weapons
        foreach (var weapon in _weaponList)
        {
            weapon.Initialize(this);
        }

        _targetArmIk = ArmIK;

        CharacterController.OnDead.AddListener((() =>
        {
            enabled = false;
            if (Armed && CurrWeapon != null)
                CurrWeapon.Hide();

        }));

    }
    #endregion

    #region Update
    void Update()
    {
        ArmIK = Mathf.Lerp(ArmIK, _targetArmIk, 5 * Time.deltaTime);

        /*        if (!IsMine)
                    return;*/

        // Owner code ...

        if (_timeToNextShot > 0)
            _timeToNextShot -= Time.deltaTime;

        if (CharacterController.AimVector.sqrMagnitude > AimingShotTreshould && Armed && BulletCount > 0)
        {
            if (_timeToNextShot <= 0f)
            {
                Fire();
            }
        }
    }

    #endregion

    #region Fire
    private void Fire()
    {
        CurrWeapon.Fire();

        _timeToNextShot = CurrWeapon.FireRate;

        if (!IsMine)
            return;

        // Owner

        BulletCount--;

        CharacterController.Animator.SetTrigger("Shoot");

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

    #region Photon
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CurrentWeaponIndex);
            stream.SendNext(BulletCount);
            stream.SendNext(Armed);
        }
        else
        {
            var wIndex = (int)stream.ReceiveNext();
            if (wIndex != CurrentWeaponIndex)
                ChangeWeapon(wIndex);

            BulletCount = (int)stream.ReceiveNext();
            Armed = (bool)stream.ReceiveNext();
        }
    }
    #endregion
}