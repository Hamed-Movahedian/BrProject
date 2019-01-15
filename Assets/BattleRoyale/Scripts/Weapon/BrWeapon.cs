using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrWeapon : MonoBehaviour
{
    public Transform ArmIK;
    public int InitialBullets=50;
    public int MaxBullets=200;
    public float FireRate = 1;

    [Header("FX")]
    public Transform Muzzle;
    public ParticleSystem BulletParticleSystem;
    public AudioSource ShotAudio;

    [Header("Bullet")]
    public BrBullet BulletPrefab;
    public int BulletPerShot = 6;
    public Vector2 ShotAngle = new Vector2(5, 15);
    public float BulletRange = 6;
    public int BulletDamage = 10;
    public Sprite Icon;

    public BrWeaponController WeaponController { get; set; }

    public bool IsMine => WeaponController.IsMine;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
        Muzzle.gameObject.SetActive(true);

        BulletParticleSystem.Emit(1);

        ShotAudio.Play();

        //if(!IsMine)
        //    return;

        // Owner code

        for (int i = 0; i < BulletPerShot; i++)
        {
            InstantiateBullet();
        }
    }

    private void InstantiateBullet()
    {
        var muzzleRot = Quaternion.Euler(0, Muzzle.eulerAngles.y, 0);

        var bullet = BrPoolManager.insance.Instantiate(
            BulletPrefab.name,
            Muzzle.transform.position,
            muzzleRot*Quaternion.Euler(ShotAngle * Random.Range(-1f, 1f)));

        bullet.GetComponent<BrBullet>().Initialize(this);
    }
}
