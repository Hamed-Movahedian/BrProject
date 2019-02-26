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
    
    #region OnDestory
    
    public delegate void OnDestoryDel();
    
    public OnDestoryDel OnDestory;
    
    #endregion
    
    #region OnInitialize
    
    public delegate void OnInitializeDel(BrCharacterController player);
    
    public OnInitializeDel OnInitialize;
    
    #endregion
    
    #region OnSetDestination
    
    public delegate void OnSetDestinationDel(Vector3 pos);
    
    public OnSetDestinationDel OnSetDestination;
    
    #endregion

    public Transform TestDestination;
    private void Awake()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["AI"].ToString() == "0")
        {
            gameObject.SetActive(false);
            OnInitialize(null);
            return;
        }
        
        //Register player
        BrPlayerTracker.Instance.OnMasterPlayerRegister += player =>
        {
            OnInitialize(player);
            player.OnDead.AddListener(() => OnDestory());
            
            // Test ...
            player.ParachuteState.OnOpenPara.AddListener(()=>Invoke(nameof(TestSetDest),Random.Range(0.5f,3)));
        };
    }

    private void TestSetDest()
    {
        SetDestination(TestDestination.position);
    }

    public void SetDestination(Vector3 pos)
    {
        Destination = pos;
        OnSetDestination(pos);
    }

    public Vector3 Destination { get; private set; }
}
