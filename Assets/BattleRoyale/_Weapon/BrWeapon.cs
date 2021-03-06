﻿using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public class BrWeapon : MonoBehaviour
{
    public Transform ArmIK;
    public int InitialBullets=50;
    public int MaxBullets=200;
    public float FireRate = 1;
    public Transform FireLocation;
    
    [Header("FX")] 
    public List<PlayableDirector> FXDirectorList;

    [Header("Bullet")]
    public BrBullet BulletPrefab;
    public int BulletPerShot = 6;
    public float ShotAngle = 15;
    public float BulletRange = 6;
    public int BulletDamage = 10;
    public Sprite Icon;

    [Header("Shake")]
    public float ShakeFactor=3;
    public float ShakeDuration=0.2f;
    private int currentFXDir=0;

    public BrWeaponController WeaponController { get; set; }

    public bool IsMine => WeaponController.IsMine;



    internal void Initialize(BrWeaponController controller)
    {
        WeaponController = controller;
        gameObject.SetActive(false);

    }

    internal void SetActive(bool v)
    {
        gameObject.SetActive(v);
    }

    public void Fire()
    {
        FXDirectorList[currentFXDir++].Play();

        currentFXDir = currentFXDir % FXDirectorList.Count;
        
/*
        if (IsMine)
            BrCameraShaker.instance.Shake(ShakeFactor, ShakeDuration);
*/

        var angle = -ShotAngle / 2;
        var delta = ShotAngle / (BulletPerShot-1);

        for (int i = 0; i < BulletPerShot; i++)
        {
            InstantiateBullet(angle);
            angle += delta;
        }
    }

    private void InstantiateBullet(float angle)
    {
        var bullet = BrPoolManager.Instance.Instantiate(
            BulletPrefab.name,
            FireLocation.position,
            Quaternion.Euler(0, WeaponController.transform.eulerAngles.y + angle, 0)
            );

        bullet.GetComponent<BrBullet>().Initialize(this);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
