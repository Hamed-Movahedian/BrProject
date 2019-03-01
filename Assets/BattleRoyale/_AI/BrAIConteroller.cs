using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BrAIConteroller : MonoBehaviour
{
    #region Instance
    
    private static BrAIConteroller _instance;
    
    public static BrAIConteroller Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BrAIConteroller>();
            return _instance;
        }
    }
    
    #endregion

    #region PlayerCount

    private int playerCount = -1;

    public int PlayerCount
    {
        get
        {
            if (playerCount == -1)
            {
                playerCount = (int) PhotonNetwork.LocalPlayer.CustomProperties["AI"];
            }
            return playerCount;
        }
    }

    #endregion
    public bool IsActive => PlayerCount > 0;
   
}
