using System;
using System.Collections;
using System.Collections.Generic;
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
    public float BulletDamage = 10;

    public BrWeaponController WeaponController { get; set; }

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

        BulletParticleSystem.Emit(30);

        ShotAudio.Play();

        for (int i = 0; i < BulletPerShot; i++)
        {
            InstantiateBullet();
        }
    }

    private void InstantiateBullet()
    {
        var muzzleRot = Quaternion.Euler(0, Muzzle.eulerAngles.y, 0);

        var bullet = Instantiate(
            BulletPrefab,
            Muzzle.transform.position,
            muzzleRot*Quaternion.Euler(ShotAngle * Random.Range(-1f, 1f)));

        bullet.Initialize(this);
    }
}
