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

    public bool Armed { set
        {
            if(_armed!=value)
            {
                _controller.Animator.SetBool("Armed", value);
            }
            _armed = false;
        }
        get { return _armed; }
    }
    #endregion

    #region privates
    private BrCharacterController _controller;
    private BrWeapon[] _weaponList;

    #endregion

    public BrWeapon ActiveWeapon => CurrentWeaponIndex == -1 ? null : _weaponList[CurrentWeaponIndex];

    internal void PickWeapon(string weaponName)
    {
        for (int i = 0; i < _weaponList.Length; i++)
        {
            if (_weaponList[i].name == weaponName)
            {
                if (CurrentWeaponIndex == i)
                {
                    AddBullets(ActiveWeapon.InitialBullets);
                }
                else
                {
                    ChangeWeapon(i);
                }
            }
        }
    }

    private void ChangeWeapon(int index)
    {
        if (ActiveWeapon)
            ActiveWeapon.SetActive(false);

        CurrentWeaponIndex = index;

        ActiveWeapon.SetActive(true);

    }

    private void AddBullets(int amount)
    {
        BulletCount += amount;

        if (ActiveWeapon)
            if (BulletCount > ActiveWeapon.MaxBullets)
                BulletCount = ActiveWeapon.MaxBullets;
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