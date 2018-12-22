using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrWeapon : MonoBehaviour
{
    public Transform ArmIK;
    public int InitialBullets=50;
    public int MaxBullets=200;


    private BrWeaponController _controller;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void Initialize(BrWeaponController controller)
    {
        _controller = controller;
        gameObject.SetActive(false);

    }

    internal void SetActive(bool v)
    {
        gameObject.SetActive(v);
    }
}
