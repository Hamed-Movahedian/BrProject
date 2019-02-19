using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class BrAirdropController : MonoBehaviourPun
{
    #region Instance

    private static BrAirdropController _instance;

    public static BrAirdropController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BrAirdropController>();
            return _instance;
        }
    }

    #endregion


    public List<BrAirDropPlaceHolder> locations;
    public float firstDropDelay=2;
    public float nextDropDelay=5;
    public PlayableDirector airDropDirector;
    public Transform airDropRoot;
    public List<BrPickupBase> pickup1Condidates;
    public List<BrPickupBase> pickup2Condidates;
    public List<BrPickupBase> pickup3Condidates;
    public UnityEvent OnNewAirDrop;
    public UnityEvent OnUnpackAirDrop;
    private int activePlaceHolderIndex=-1;

    private void Awake()
    {
        BrPlayerTracker.Instance.OnMasterPlayerRegister += player =>
        {
            player.ParachuteState.OnLanding.AddListener((() =>
            {
                Invoke(nameof(DropAirSupply), firstDropDelay);
            }));
        };
    }

    private void DropAirSupply()
    {
        if(PhotonNetwork.IsMasterClient)
            photonView.RPC(nameof(DropAirSupplyRpc),RpcTarget.AllViaServer,GetDropPosition());
        
    }

    private int GetDropPosition()
    {
        return Random.Range(0, locations.Count);
    }

    [PunRPC]
    private void DropAirSupplyRpc(int posIndex)
    {
        
        activePlaceHolderIndex = posIndex;
        airDropRoot.position = locations[posIndex].transform.position;
        airDropDirector.Play();
        OnNewAirDrop.Invoke();
    }

    public void Unpack()
    {
        var pIndex1 = Random.Range(0, pickup1Condidates.Count);
        var pIndex2 = Random.Range(0, pickup2Condidates.Count);
        var pIndex3 = Random.Range(0, pickup3Condidates.Count);
        photonView.RPC(nameof(UnpackRpc),RpcTarget.Others,pIndex1,pIndex2,pIndex3);
        UnpackRpc(pIndex1,pIndex2,pIndex3);
    }
    
    [PunRPC]
    private void UnpackRpc(int pIndex1, int pIndex2, int pIndex3)
    {
        var pickup = Instantiate<BrPickupBase>(pickup1Condidates[pIndex1]);
        pickup.transform.position = activePlaceHolder.GetPickupPos(0);
        BrPickupManager.Instance.AddPickup(pickup);
        
        pickup = Instantiate<BrPickupBase>(pickup1Condidates[pIndex2]);
        pickup.transform.position = activePlaceHolder.GetPickupPos(1);
        BrPickupManager.Instance.AddPickup(pickup);

        pickup = Instantiate<BrPickupBase>(pickup1Condidates[pIndex3]);
        pickup.transform.position = activePlaceHolder.GetPickupPos(2);
        BrPickupManager.Instance.AddPickup(pickup);

        OnUnpackAirDrop.Invoke();
        Invoke(nameof(DropAirSupply), nextDropDelay);

    }

    private BrAirDropPlaceHolder activePlaceHolder => locations[activePlaceHolderIndex];
}