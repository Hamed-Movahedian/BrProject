using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrDeathTracker : MonoBehaviourPunCallbacks
{
    public BrCharacterController activePlayer;

    public static BrDeathTracker instance;


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
        var victomPhotonView = PhotonNetwork.GetPhotonView(victomViewID);
        var victomPlayer = victomPhotonView.GetComponent<BrCharacterController>();

        BrCharacterController killerPlayer = null;

        if (killerViewID != -1)
        {
            var killerPhotonView = PhotonNetwork.GetPhotonView(killerViewID);

            killerPlayer = killerPhotonView.GetComponent<BrCharacterController>();
            // Log 
            BrLogManager.instance.LogKill(killerPlayer.profile.UserID, weaponName, victomPlayer.profile.UserID);
        }
        else
        {
            BrLogManager.instance.LogKill(victomPlayer.profile.UserID);
        }



        // chage active player
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

    internal void SetActivePlayer(BrCharacterController player)
    {
        if (player == null)
        {
            player = FindObjectsOfType<BrCharacterController>()
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


}
