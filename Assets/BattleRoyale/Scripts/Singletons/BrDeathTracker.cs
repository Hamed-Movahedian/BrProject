using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrDeathTracker : MonoBehaviourPunCallbacks
{
    public static BrDeathTracker instance;
    public BrCharacterController activePlayer;


    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void PlayerDead(int victomViewID, int killerViewID, string weaponName)    
    {
        var victomPhotonView = PhotonNetwork.GetPhotonView(victomViewID);
        var killerPhotonView = PhotonNetwork.GetPhotonView(killerViewID);

        var victomProfile = Profile.Deserialize((string)victomPhotonView.Owner.CustomProperties["Profile"]);
        var killerProfile = Profile.Deserialize((string)killerPhotonView.Owner.CustomProperties["Profile"]);

        var victomPlayer= victomPhotonView.GetComponent<BrCharacterController>();
        var killerPlayer = killerPhotonView.GetComponent<BrCharacterController>();

        // Log 
        BrLogManager.instance.LogKill(killerProfile.UserID, weaponName, victomProfile.UserID);

        // chage active player
        if (victomViewID == activePlayer.photonView.ViewID)
        {
            SetActivePlayer(killerPlayer);
            BrUIController.Instance.ActivePlayerIsDead(victomPlayer,killerPlayer,weaponName);
        }
    }

    internal void SetActivePlayer(BrCharacterController player)
    {
        activePlayer = player;

        BrCamera.Instance.SetCharacter(player);
    }

    
}
