using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BrKillZone : MonoBehaviourPunCallbacks
{
    public BrRing currCenter;
    public BrRing targetCenter;

    public float TimeToNextRing = 15;
    public float ChangeTime = 10;

    private float _shrinkTime = -1;
    void Start()
    {
        gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            targetCenter.gameObject.SetActive(true);
            CreateNextCircle();
        }
    }

    private void CreateNextCircle()
    {
        var nextR = currCenter.radious * .7f;
        Vector3 nextCenter = Random.insideUnitCircle * (currCenter.radious - nextR);
        nextCenter = new Vector3(nextCenter.x, 0, nextCenter.y) + currCenter.transform.localPosition;
        photonView.RPC("NewCircle", RpcTarget.AllViaServer, nextCenter, nextR);
    }

    [PunRPC]
    public void NewCircle(Vector3 center, float radious)
    {
        gameObject.SetActive(true);
        targetCenter.transform.localPosition = center;
        targetCenter.radious = radious;
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
            currCenter.transform.localPosition = targetCenter.transform.localPosition;
            currCenter.radious = targetCenter.radious;

            if (PhotonNetwork.IsMasterClient)
                CreateNextCircle();

        }

        else if (_shrinkTime <= ChangeTime)
        {
            currCenter.transform.localPosition = Vector3.Lerp(
                currCenter.transform.localPosition,
                targetCenter.transform.localPosition,
                (ChangeTime-_shrinkTime- Time.deltaTime) *Time.deltaTime /ChangeTime);

            currCenter.radious = Mathf.Lerp(
                currCenter.radious,
                targetCenter.radious,
                (ChangeTime - _shrinkTime- Time.deltaTime) * Time.deltaTime / ChangeTime);
        }
    }
}
