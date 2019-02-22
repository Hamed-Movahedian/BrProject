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
    public System.Random random;
    public List<BrPickupBase> scenePickups;

    [ContextMenu("Collect")]
    public void CollectAllPickups()
    {
        placeHolders = FindObjectsOfType<BrPickupPlaceHolder>().ToList();
        scenePickups = FindObjectsOfType<BrPickupBase>().ToList();
    }
   

    // Use this for initialization
    IEnumerator Start()
    {
        random = BrRandom.CreateNew();

        scenePickups.ForEach(sp=>sp.Evaluate(random));
        
        var delta =Mathf.Max(placeHolders.Count / 60,2);

        for (int i = 0; i < placeHolders.Count;)
        {
            for (int j = 0; j < delta && i < placeHolders.Count; j++,i++) 
                placeHolders[i].Evaluate(random);

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