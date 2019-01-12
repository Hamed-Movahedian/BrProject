using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BrKillZone : MonoBehaviourPunCallbacks
{
    public BrRing currRing;
    public BrRing targetRing;

    public float StartTime = 10;
    public float TimeToNextRing = 15;
    public float ChangeTime = 10;
    public int DamageAmount = 10;
    public float DamageRate = 1;
    private float _shrinkTime = -1;
    void Start()
    {
        gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
            Invoke("CreateNextCircle", StartTime);

        InvokeRepeating("DamageToPlayer", StartTime, DamageRate);
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
        photonView.RPC("NewCircle", RpcTarget.AllViaServer, nextCenter, nextR);
    }

    [PunRPC]
    public void NewCircle(Vector3 center, float radious)
    {
        targetRing.gameObject.SetActive(true);
        gameObject.SetActive(true);
        targetRing.transform.localPosition = center;
        targetRing.radious = radious;
        _shrinkTime = TimeToNextRing + ChangeTime;
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

            if (PhotonNetwork.IsMasterClient)
                CreateNextCircle();

        }

        else if (_shrinkTime <= ChangeTime)
        {
            currRing.transform.localPosition = Vector3.Lerp(
                currRing.transform.localPosition,
                targetRing.transform.localPosition,
                (ChangeTime - _shrinkTime) * Time.deltaTime / ChangeTime);

            currRing.radious = Mathf.Lerp(
                currRing.radious,
                targetRing.radious,
                (ChangeTime - _shrinkTime) * Time.deltaTime / ChangeTime);
        }
    }
}
