using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrMatchstatRecorder : MonoBehaviour
{
    public Text KillCount;
    public Text KillPoint;
    public Text WinPoint;
    public Text Rank;
    public GameObject WinText;
    
    private DateTime lastKilTime;
    private DateTime lastDoubleKilTime;
    private MatchStats thisMatchStat=new MatchStats();

    private bool win;

    // Start is called before the first frame update
    void Start()
    {
        lastKilTime = DateTime.MinValue;
    }

    public void RecordKill()
    {
        DateTime killTime=DateTime.Now;
        if (DateTime.Compare(lastKilTime,killTime)<1)
        {
            if (DateTime.Compare(lastDoubleKilTime,killTime)<1)
            {
                thisMatchStat.TripleKills++;
                thisMatchStat.DoubleKills--;
                lastKilTime = DateTime.MinValue;
                lastDoubleKilTime=DateTime.MinValue;
            }
            else
            {
                thisMatchStat.DoubleKills++;
                lastDoubleKilTime=DateTime.Now;
                lastKilTime=DateTime.Now;
            }
        }
        else
            lastKilTime=DateTime.Now;
        
        thisMatchStat.Kills++;

    }

    public void RecordPickup(BrPickupBase pickUp)
    {
        if (pickUp as BrWeaponPickup != null)
            thisMatchStat.GunsCollected++;
        else
            thisMatchStat.ItemsCollected ++;
    }

    public void RecordOpenSupplyDrop()
    {
        thisMatchStat.SupplyDrop++;
    }

    public void RecordOpenChest()
    {
        thisMatchStat.SupplyCreates++;
    }

    public void ShowStats(int rank)
    {
        win = rank==1;
        thisMatchStat.Wins =win ?1:0;
        WinText.SetActive(win);
        Rank.gameObject.SetActive(!win);
        Rank.text = "# "+rank;
        KillCount.text = thisMatchStat.Kills.ToString();
        KillPoint.text = (thisMatchStat.Kills * 5).ToString();
        WinPoint.text = (thisMatchStat.Wins * 20).ToString();
        SaveMatchRecordToProfile();
    }
    
    public void SaveMatchRecordToProfile()
    {
        ProfileManager.Instance().SetMatchStat(thisMatchStat);
        thisMatchStat=new MatchStats();
    }
    
    
    
}