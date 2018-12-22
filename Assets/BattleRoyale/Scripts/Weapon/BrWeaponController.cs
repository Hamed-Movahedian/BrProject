using System;
using System.Collections.Generic;
using UnityEngine;

public class BrWeaponController : MonoBehaviour
{
    #region Publics
    public Transform WeaponSlot;
    public int CurrentWeaponIndex = -1;
    public int BulletCount = 0;
    #endregion

    #region properties

    private bool _armed = false;

    public bool Armed
    {
        set
        {
            if (_armed != value)
            {
                _controller.Animator.SetBool("Armed", value);
            }
            _armed = value;
        }
        get { return _armed; }
    }
    #endregion

    #region privates
    private BrCharacterController _controller;
    private BrWeapon[] _weaponList;

    #endregion

    public BrWeapon CurrWeapon => CurrentWeaponIndex == -1 ? null : _weaponList[CurrentWeaponIndex];

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
                BulletCount = Mathf.Min(BulletCount, CurrWeapon.InitialBullets);
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

    private void AddBullets(int amount)
    {
        BulletCount += amount;

        if (CurrWeapon)
            if (BulletCount > CurrWeapon.MaxBullets)
                BulletCount = CurrWeapon.MaxBullets;
    }

    void Start()
    {
        _controller = GetComponent<BrCharacterController>();

        // Get weapon list
        _weaponList = WeaponSlot.GetComponentsInChildren<BrWeapon>();

        // Initialize weapons
        foreach (var weapon in _weaponList)
        {
            weapon.Initialize(this);
        }
    }

    void Update()
    {
    }

}