using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrPlayerUI : MonoBehaviour
{
    public Image Health;
    public Image Shield;
    public Image WeaponIcon;
    public Text AmmoText;
    public Sprite MeleWeaponIcon;

    private BrCharacterController _player;

    void Start()
    {
        ActiveUI(false);

        BrPlayerTracker.instance.OnPlayerDead += PlayerDead;
        BrPlayerTracker.instance.OnPlayerRegisterd += PlayerRegister;
    }

    private void PlayerRegister(BrCharacterController player)
    {
        if (player.isMine)
        {
            _player = player;

            _player.OnStatChange += PlayerStatChange;

            _player.ParachuteState.OnLanding.AddListener(() => 
            {
                ActiveUI(true);
                PlayerStatChange(player);
                WeaponStatChange(_player.WeaponController);
            });

            _player.WeaponController.OnStatChange += WeaponStatChange;

           
        }
    }

    private void WeaponStatChange(BrWeaponController weapon)
    {
        if(weapon.CurrentWeaponIndex==-1)
        {
            WeaponIcon.sprite = MeleWeaponIcon;
            AmmoText.gameObject.SetActive(false);
        }
        else
        {
            if(weapon.CurrWeapon && weapon.CurrWeapon.Icon)
                WeaponIcon.sprite = weapon.CurrWeapon.Icon;
            AmmoText.gameObject.SetActive(true);
            AmmoText.text = weapon.BulletCount.ToString();

        }
    }

    private void PlayerStatChange(BrCharacterController player)
    {
        Health.fillAmount = player.Health / (float)player.MaxHealth;
        Shield.fillAmount = player.Shield / (float)player.MaxShield;
    }

    private void PlayerDead(BrCharacterController victom, BrCharacterController killer, string weaponName)
    {
        if (victom.isMine)
            ActiveUI(false);
    }

    private void ActiveUI(bool value)
    {
        gameObject.SetActive(value);
    }
 
}
