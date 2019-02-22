using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public static class BrRandom 
{
    public static Random CreateNew()
    {
        int seed = (int) PhotonNetwork.CurrentRoom.CustomProperties["Seed"];
                
        return new System.Random(seed);
    }
}
