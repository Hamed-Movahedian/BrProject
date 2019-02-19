using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BrPickupManager : MonoBehaviourPunCallbacks
{
    #region Instance

    private static BrPickupManager instance;

    public static BrPickupManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BrPickupManager>();
            return instance;
        }
    }

    #endregion

    public int chanceFactor = 1;
    public List<BrPickupPlaceHolder> placeHolders;
    private List<BrPickupBase> allPickups=new List<BrPickupBase>();

    [ContextMenu("Collect")]
    public void CollectAllPickups()
    {
        placeHolders = FindObjectsOfType<BrPickupPlaceHolder>().ToList();
    }

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int seed = Random.Range(0, 9999);
            photonView.RPC(nameof(InitializeRpc),RpcTarget.All,seed);
        }
    }

    [PunRPC]
    private void InitializeRpc(int seed)
    {
        StartCoroutine(InitializeCoroutine(seed));
        
    }

    private IEnumerator InitializeCoroutine(int seed)
    {
        var random = new System.Random(seed);

        var delta =Mathf.Max(placeHolders.Count / 60,2);

        for (int i = 0; i < placeHolders.Count;)
        {
            for (int j = 0; j < delta && i < placeHolders.Count; j++,i++)
            {
                var pickup = placeHolders[i].Evaluate(random);

                if (pickup != null) 
                    AddPickup(pickup);
            }

            yield return null;
        }
        
    }

    public void DisablePickup(BrPickupBase pickup)
    {
        if (pickup.Index >= 0)
            photonView.RPC(nameof(DisablePickupRpc), RpcTarget.All, pickup.Index);
    }

    [PunRPC]
    public void DisablePickupRpc(int index)
    {
        allPickups[index].DisablePickup();
    }
    public void AddPickup(BrPickupBase pickup)
    {
        pickup.Index = allPickups.Count;
        allPickups.Add(pickup);
    }

}