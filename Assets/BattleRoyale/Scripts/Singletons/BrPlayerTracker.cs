using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BrPlayerTracker : MonoBehaviourPunCallbacks
{
    public delegate void PlayerDeadDelegate(BrCharacterController victom, BrCharacterController killer, string weaponName);
    public delegate void PlayerRegisterDelegate(BrCharacterController player);
    public PlayerDeadDelegate OnPlayerDead;
    public PlayerRegisterDelegate OnPlayerRegisterd;

    [HideInInspector]
    public BrCharacterController activePlayer;

    public static BrPlayerTracker instance;

    private List<BrCharacterController> alivePlayers=new List<BrCharacterController>();

    public int PlayerCounter => alivePlayers.Count;

    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void PlayerDead(int victomViewID, int killerViewID, string weaponName)
    {
        var victomPlayer = GetPlayerByViewID(victomViewID);

        alivePlayers.Remove(victomPlayer);
        

        BrCharacterController killerPlayer = null;

        if (killerViewID != -1)
        {
            killerPlayer = GetPlayerByViewID(killerViewID);
            // Log 
            BrLogManager.instance.LogKill(killerPlayer.profile.UserID, weaponName, victomPlayer.profile.UserID);
        }
        else
        {
            BrLogManager.instance.LogKill(victomPlayer.profile.UserID);
        }

        OnPlayerDead(victomPlayer, killerPlayer, weaponName);

        // change active player
        if (victomViewID == activePlayer.photonView.ViewID)
        {
            if (killerViewID != -1)
            {
                SetActivePlayer(killerPlayer);
                BrUIController.Instance.ActivePlayerIsDead(victomPlayer, killerPlayer, weaponName);
            }
            else
            {
                SetActivePlayer(null);
                BrUIController.Instance.ActivePlayerIsDead(victomPlayer);
            }
        }
    }

    private static BrCharacterController GetPlayerByViewID(int victomViewID)
    {
        return PhotonNetwork
                    .GetPhotonView(victomViewID)
                    .GetComponent<BrCharacterController>();
    }

    internal void SetActivePlayer(BrCharacterController player)
    {
        if (player == null)
        {
            player = alivePlayers
                .Where(c => c.IsAlive && c != activePlayer)
                .OrderBy(c => Vector3.Distance(c.transform.position, activePlayer.transform.position))
                .FirstOrDefault();
        }
        activePlayer = player;

        if (player == null)
            return;

        if(activePlayer)
            BrCamera.Instance.SetCharacter(player);
    }


    public void RegisterPlayer(BrCharacterController player)
    {
        if(player.isMine)
            SetActivePlayer(player);
        alivePlayers.Add(player);
        OnPlayerRegisterd(player);
    }
}
