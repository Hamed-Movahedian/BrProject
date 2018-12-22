﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class BrPickupBase : MonoBehaviour
{
    public float Duration = 2;
    public Image Image;

    private BrCharacterController _currentPlayer=null;
    private float _timeToGetReward=0;

    // Use this for initialization
    void Start ()
    {
        if (Image)
            Image.fillAmount = 0;

    }

    // Update is called once per frame
    void Update ()
    {
		if(_timeToGetReward>0)
        {
            _timeToGetReward -= Time.deltaTime;


            if (_timeToGetReward <= 0)
                GetReward(_currentPlayer);
            else
                if (Image)
                    Image.fillAmount = (_timeToGetReward/Duration );
        }
    }

    protected virtual void GetReward(BrCharacterController currentPlayer)
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (_currentPlayer)
            return;

        var controller = other.GetComponent<BrCharacterController>();

        if (controller)
        {
            _currentPlayer = controller;
            _timeToGetReward = Duration;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!_currentPlayer)
            return;

        var controller = other.GetComponent<BrCharacterController>();

        if(_currentPlayer == controller)
        {
            _currentPlayer = null;
            _timeToGetReward = 0;
            if (Image)
                Image.fillAmount = 0;
        }
    }

}
