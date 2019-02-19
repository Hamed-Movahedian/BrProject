using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class BrPickupBase : MonoBehaviour
{
    [Range(0,100)]
    public int Chance=50;
    public float Duration = 2;
    public Image Image;

    private BrCharacterController _currentPlayer=null;
    private float _timeToGetReward=0;
    public int Index { get; set; }

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
        BrPickupManager.Instance.DisablePickup(this);
    }

    public void DisablePickup()
    {
        gameObject.SetActive(false);
    }
 
    private void OnTriggerStay(Collider other)
    {
        if (_currentPlayer)
            return;

        var controller = other.GetComponent<BrCharacterController>();

        if (controller && controller.isMine && CanPickup(controller))
        {
            _currentPlayer = controller;
            _timeToGetReward = Duration;
        }

    }

    protected virtual bool CanPickup(BrCharacterController controller)
    {
        return true;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}