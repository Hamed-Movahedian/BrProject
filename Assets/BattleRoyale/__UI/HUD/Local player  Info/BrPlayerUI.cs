using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BrPlayerUI : MonoBehaviour
{
    public Image Health;
    public Image Shield;
    public Image WeaponIcon;
    public Text AmmoText;
    public Sprite MeleWeaponIcon;

    public UnityEvent OnShow;
    public UnityEvent OnHide;
    
    private BrCharacterController _player;

    void Awake()
    {
        // master player enter
        BrPlayerTracker.Instance.OnMasterPlayerRegister += player =>
        {
            _player = player;

            _player.OnStatChange += PlayerStatChange;
            _player.WeaponController.OnStatChange += WeaponStatChange;

            _player.ParachuteState.OnLanding.AddListener(() =>
            {
                OnShow.Invoke();
                PlayerStatChange(player);
                WeaponStatChange(_player.WeaponController);
            });
            
            _player.OnDead.AddListener((() => OnHide.Invoke()));
        };
        
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

    
}
