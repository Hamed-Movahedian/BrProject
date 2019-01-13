using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrPickupManager : MonoBehaviourPunCallbacks
{
    public static BrPickupManager instance;
    public int chanceFactor = 1;
    public List<BrPickupBase> allPickups;

    [ContextMenu("Collect")]
    public void CollectAllPickups()
    {
        allPickups = FindObjectsOfType<BrPickupBase>().ToList();
    }

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        allPickups.ForEach(p => p.gameObject.SetActive(false));

        if (!PhotonNetwork.IsMasterClient)
            return;

        BitArray bitArray = new BitArray(allPickups.Count);

        for (int i = 0; i < allPickups.Count; i++)
            bitArray[i] = Random.value * 100 / chanceFactor < allPickups[i].Chance;

        Byte[] byteArray = new byte[allPickups.Count / 8 + 1];

        bitArray.CopyTo(byteArray, 0);
        photonView.RPC("SetPickups", RpcTarget.All, byteArray);
    }

    [PunRPC]
    public void SetPickups(byte[] byteArray)
    {
        var bitArray = new BitArray(byteArray);

        for (int i = 0; i < allPickups.Count; i++)
            allPickups[i].gameObject.SetActive(bitArray[i]);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisablePickup(BrPickupBase pickup)
    {
        var indexOf = allPickups.IndexOf(pickup);
        if (indexOf >= 0)
        {
            photonView.RPC("DisablePickup", RpcTarget.All, indexOf);
        }
    }

    [PunRPC]
    public void DisablePickup(int index)
    {
        allPickups[index].DisablePickup();
    }
}
