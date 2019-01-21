﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BrKillZone : MonoBehaviourPunCallbacks
{
    #region Instance
    private static BrKillZone instance;
    public static BrKillZone Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BrKillZone>();
            return instance;
        }
    }

    #endregion

    public BrRing currRing;
    public BrRing targetRing;

    public float StartNewCircleDelay = 10;
    public float TimeToNextRing = 15;
    public float ChangeTime = 10;
    public int DamageAmount = 10;
    public float DamageRate = 1;
    private float _shrinkTime = -1;
    private Vector3 currCenter;
    private float currRadious;



    #region Events
    public delegate void NewCircle(Vector3 center, float Radious);
    public delegate void WaitForShrink(int time);

    public delegate void Shirinking(int time);
    public NewCircle OnNewCircle;
    public Shirinking Shrinking;
    public WaitForShrink OnWaitForShrink; 
    #endregion

    void Start()
    {
        gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
            Invoke("CreateNextCircle", StartNewCircleDelay);

        InvokeRepeating("DamageToPlayer", StartNewCircleDelay, DamageRate);
    }

    private void DamageToPlayer()
    {
        if (BrCharacterController.MasterCharacter != null && BrCharacterController.MasterCharacter.IsAlive)
        {
            if (Vector3.Distance(BrCharacterController.MasterCharacter.transform.position, currRing.transform.position) > currRing.radious)
                BrCharacterController.MasterCharacter.TakeDamage(DamageAmount, Vector3.back, null, null);
        }
    }

    private void CreateNextCircle()
    {
        var nextR = currRing.radious * .7f;
        Vector3 nextCenter = Random.insideUnitCircle * (currRing.radious - nextR);
        nextCenter = new Vector3(nextCenter.x, 0, nextCenter.y) + currRing.transform.localPosition;
        photonView.RPC("NewCircleRPC", RpcTarget.AllViaServer, nextCenter, nextR);
    }

    [PunRPC]
    public void NewCircleRPC(Vector3 center, float radious)
    {
        targetRing.gameObject.SetActive(true);
        gameObject.SetActive(true);
        targetRing.transform.localPosition = center;
        targetRing.radious = radious;
        _shrinkTime = TimeToNextRing + ChangeTime;

        currCenter = currRing.transform.localPosition;
        currRadious = currRing.radious;
        OnNewCircle(targetRing.transform.position, radious);
    }

    // Update is called once per frame
    void Update()
    {

        if (_shrinkTime < 0)
            return;

        _shrinkTime -= Time.deltaTime;

        if (_shrinkTime <= 0)
        {
            currRing.transform.localPosition = targetRing.transform.localPosition;
            currRing.radious = targetRing.radious;

            Shrinking(0);
            if (PhotonNetwork.IsMasterClient)
                Invoke("CreateNextCircle", StartNewCircleDelay);

        }

        else if (_shrinkTime <= ChangeTime)
        {
            currRing.transform.localPosition = Vector3.Lerp(
                currCenter,
                targetRing.transform.localPosition,
                (ChangeTime - _shrinkTime) / ChangeTime);

            currRing.radious = Mathf.Lerp(
                currRadious,
                targetRing.radious,
                (ChangeTime - _shrinkTime) / ChangeTime);
            Shrinking((int)_shrinkTime);
        }
        else
            OnWaitForShrink((int)(_shrinkTime - ChangeTime));
    }
}