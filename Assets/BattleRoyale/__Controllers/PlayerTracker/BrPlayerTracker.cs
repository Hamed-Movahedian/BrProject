﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BrPlayerTracker : MonoBehaviour
{
    public delegate void PlayerDeadDelegate(BrCharacterController victim, BrCharacterController killer, string weaponName);
    public delegate void PlayerRegisterDelegate(BrCharacterController player);
    public PlayerDeadDelegate OnPlayerDead;
    public PlayerRegisterDelegate OnPlayerRegisterd;

    #region OnLastPlayerLeft 

    public delegate void OnLastPlayerLeftDel(BrCharacterController player);

    public OnLastPlayerLeftDel OnLastPlayerLeft;

    #endregion
    
    #region Instance
    private static BrPlayerTracker instance;
    public static BrPlayerTracker Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BrPlayerTracker>();
            return instance;
        }
    }

    #endregion

    public List<BrCharacterController> alivePlayers = new List<BrCharacterController>();

    public int PlayerCounter => alivePlayers.Count;

    #region Active player
    public delegate void ActivePlayerChangeDelegate(BrCharacterController preActivePlayer, BrCharacterController nextActivePlayer);
    public ActivePlayerChangeDelegate OnActivePlayerChange;
    private BrCharacterController activePlayer;
    public BrCharacterController ActivePlayer
    {
        get => activePlayer;
        set
        {
            if (activePlayer != value)
            {
                var preAP = activePlayer;
                activePlayer = value;
                OnActivePlayerChange(preAP, activePlayer);
            }
        }
    }

    #endregion

    #region OnMasterPlayerRegister

    public delegate void OnMasterPlayerRegisterDel(BrCharacterController masterPlayer);

    public OnMasterPlayerRegisterDel OnMasterPlayerRegister;
    private Dictionary<string,BrCharacterController> UserIdDic=new Dictionary<string, BrCharacterController>();

    #endregion
    
    private void Awake()
    {
        instance = this;
    }

    internal void PlayerDead(int victimViewID, int killerViewID, string weaponName)
    {
        var victimPlayer = GetPlayerByViewID(victimViewID);
        var killerPlayer = GetPlayerByViewID(killerViewID);

        alivePlayers.Remove(victimPlayer);

        // change active player
        if (victimPlayer == ActivePlayer)
            SetActivePlayer(killerPlayer);
        
        OnPlayerDead(victimPlayer, killerPlayer, weaponName);
        
        if (alivePlayers.Count == 1)
            OnLastPlayerLeft(alivePlayers[0]);
    }

    private static BrCharacterController GetPlayerByViewID(int victomViewID)
    {

        return victomViewID==-1 ? 
            null :
            PhotonNetwork
                    .GetPhotonView(victomViewID)
                    .GetComponent<BrCharacterController>();
    }

    internal void SetActivePlayer(BrCharacterController player)
    {
        if (player == null)
        {
            player = alivePlayers
                .Where(c => c.IsAlive && c != ActivePlayer)
                .OrderBy(c => Vector3.Distance(c.transform.position, ActivePlayer.transform.position))
                .FirstOrDefault();
        }
        ActivePlayer = player;
       
    }


    public void RegisterPlayer(BrCharacterController player)
    {
        if (player.isMine)
        {
            SetActivePlayer(player);
            OnMasterPlayerRegister(player);
        }

        alivePlayers.Add(player);

        UserIdDic[player.UserID] = player;
        
        OnPlayerRegisterd(player);
        
        
    }

    public BrCharacterController this[string userID] => UserIdDic[userID];
}
