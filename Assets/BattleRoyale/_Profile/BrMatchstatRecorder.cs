using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BrMatchstatRecorder : MonoBehaviour
{
    public Text KillCount;
    public Text KillPoint;
    public Text WinPoint;
    [FormerlySerializedAs("Rank")] public Text RankText;
    public GameObject WinText;
    
    private float lastKilTime;
    private float lastDoubleKilTime;
    private MatchStats thisMatchStat=new MatchStats();

    private bool win;
    private int rank = 1;

    // Start is called before the first frame update
    void Start()
    {
        
        BrGameManager.Instance.OnMatchFinished.AddListener(()=> ShowStats());
        
        // Kill counter
        BrPlayerTracker.Instance.OnPlayerDead += (victim, killer, weaponName) =>
        {
            if (killer == BrCharacterController.MasterCharacter)
                RecordKill();

            if (victim == BrCharacterController.MasterCharacter)
                rank = BrPlayerTracker.Instance.PlayerCounter+1;
        };
        
        //Pickup counter
        BrPickupManager.Instance.OnPickedup += (player, pickup) =>
        {
            if (player == BrCharacterController.MasterCharacter)
                RecordPickup(pickup);
        };
        
        // AirDrop counter
        BrAirdropController.Instance.OnUnpack += player =>
        {
            if (player == BrCharacterController.MasterCharacter)
                RecordOpenSupplyDrop();
        };
    }

    public void RecordKill()
    {
        float killTime=Time.time;
        
        if (killTime-lastKilTime<1)
        {
            if (killTime-lastDoubleKilTime<1)
            {
                thisMatchStat.TripleKills++;
                thisMatchStat.DoubleKills--;
                lastKilTime = 0;
                lastDoubleKilTime=0;
            }
            else
            {
                thisMatchStat.DoubleKills++;
                lastDoubleKilTime=killTime;
                lastKilTime=killTime;
            }
        }
        else
            lastKilTime=killTime;
        
        thisMatchStat.Kills++;

    }

    public void RecordPickup(BrPickupBase pickUp)
    {
        switch (pickUp)
        {
            case BrChestPickup brChestPickup:
                thisMatchStat.SupplyCreates ++;
                break;
            case BrWeaponPickup brWeaponPickup:
                thisMatchStat.GunsCollected++;
                break;
            default:
                thisMatchStat.ItemsCollected ++;
                break;
        }
    }

    public void RecordOpenSupplyDrop()
    {
        thisMatchStat.SupplyDrop++;
    }

    public void RecordOpenChest()
    {
        thisMatchStat.SupplyCreates++;
    }

    public void ShowStats()
    {
        win = rank==1;
        thisMatchStat.Wins =win ?1:0;
        WinText.SetActive(win);
        RankText.gameObject.SetActive(!win);
        RankText.text = "# "+rank;
        KillCount.text = thisMatchStat.Kills.ToString();
        KillPoint.text = (thisMatchStat.Kills * 5).ToString();
        WinPoint.text = (thisMatchStat.Wins * 20).ToString();
        GetComponent<PlayableDirector>().Play();
        SaveMatchRecordToProfile();
    }
    
    public void SaveMatchRecordToProfile()
    {
        Debug.Log(thisMatchStat.ToString());
        ProfileManager.Instance().SetMatchStat(thisMatchStat);
        thisMatchStat=new MatchStats();
    }
    
}