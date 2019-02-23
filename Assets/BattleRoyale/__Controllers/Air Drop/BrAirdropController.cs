using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float firstDropDelay = 2;
    public float nextDropDelay = 5;
    public PlayableDirector airDropDirector;
    public Transform airDropRoot;
    public List<BrPickupBase> pickup1Condidates;
    public List<BrPickupBase> pickup2Condidates;
    public List<BrPickupBase> pickup3Condidates;
    public UnityEvent OnNewAirDrop;
    public UnityEvent OnUnpackAirDrop;

    #region OnUnpack

    public delegate void OnUnpackDel(BrCharacterController player);

    public OnUnpackDel OnUnpack;

    #endregion

    private int activePlaceHolderIndex = -1;

    private void Awake()
    {
        BrPlayerTracker.Instance.OnMasterPlayerRegister += player =>
        {
            player.ParachuteState.OnLanding.AddListener((() => { Invoke(nameof(DropAirSupply), firstDropDelay); }));
        };
        
        //New area
        BrKillZone.Instance.OnNewCircleExtented += (curr, target, delay, shrinkTime) =>
        {
            int minIndex = -1;
            float minValue = float.MaxValue;
            
            List<int> deletedIndex=new List<int>();

            var center = (curr.transform.position + target.transform.position) / 2;
            var radious = (curr.radious + target.radious) / 2;

            radious *= radious;
            
            for (int i = 0; i < locations.Count; i++)
            {
                if(!curr.IsInside(locations[i].transform.position))
                    deletedIndex.Add(i);
                else
                {
                    var magnitude = Vector3.SqrMagnitude(center - locations[i].transform.position);
                    var distance = Mathf.Abs(magnitude - radious);

                    if (distance < minValue)
                    {
                        minValue = distance;
                        minIndex = i;
                    }
                }
            }

            if (minIndex==-1)
                return;
            
            
        };
    }

    private void DropAirSupply()
    {
        if(activePlaceHolderIndex!=-1)
            locations.RemoveAt(activePlaceHolderIndex);
        
        if (!PhotonNetwork.IsMasterClient)
            return;

        if(locations.Count==0)
            return;
        
        var i = Random.Range(0, locations.Count);
        photonView.RPC(nameof(DropAirSupplyRpc), RpcTarget.AllViaServer, i);
    }

    

    [PunRPC]
    private void DropAirSupplyRpc(int posIndex)
    {
        activePlaceHolderIndex = posIndex;
        airDropRoot.position = locations[posIndex].transform.position;
        airDropDirector.Play();
        OnNewAirDrop.Invoke();
    }

    public void Unpack(BrLocalPlayerTrigger playerTrigger)
    {
        var pIndex1 = Random.Range(0, pickup1Condidates.Count);
        var pIndex2 = Random.Range(0, pickup2Condidates.Count);
        var pIndex3 = Random.Range(0, pickup3Condidates.Count);
        photonView.RPC(nameof(UnpackRpc), RpcTarget.Others, pIndex1, pIndex2, pIndex3, playerTrigger.LastUserID);
        UnpackRpc(pIndex1, pIndex2, pIndex3, playerTrigger.LastUserID);
    }

    [PunRPC]
    private void UnpackRpc(int pIndex1, int pIndex2, int pIndex3, string userID)
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
        OnUnpack(BrPlayerTracker.Instance[userID]);
        Invoke(nameof(DropAirSupply), nextDropDelay);
    }

    private BrAirDropPlaceHolder activePlaceHolder => locations[activePlaceHolderIndex];
    public Vector3 DropPosition => airDropRoot.position;
}