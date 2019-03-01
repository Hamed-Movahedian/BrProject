using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BrMatchstatRecorder : MonoBehaviour
{
    public BrCharacterModel Avatar;


    public Text KillCount;
    public Text KillPoint;
    public Text WinPoint;
    public Text MatchText;
    
    [FormerlySerializedAs("Rank")]
    public Text RankText;
    public GameObject WinText;

    public BrLevelupSlider levelSlider;


    private float lastKilTime;
    private float lastDoubleKilTime;
    private MatchStats thisMatchStat = new MatchStats();

    private bool win;
    private int rank = 1;
    private int _exp;
    private int _level;
    // Start is called before the first frame update



    #region Instance
    private static BrMatchstatRecorder instance;
    private float _startTime;
    private float _finishTime;

    public static BrMatchstatRecorder Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BrMatchstatRecorder>();
            return instance;
        }
    }

    #endregion


    #region Initialize

    void Start()
    {
        Statistics stat = ProfileManager.Instance().PlayerProfile.PlayerStat;
        _level = BrExpManager.CalLevel(stat.Experience);
        _exp = stat.Experience;
        BrGameManager.Instance.OnMatchFinished.AddListener(() =>
        {
            rank = Mathf.Max(BrPlayerTracker.Instance.PlayerCounter, rank);

            if (rank == 1)
                _finishTime = Time.time;

            ShowStats();
        });
        BrPlayerTracker.Instance.OnMasterPlayerRegister += (player) => _startTime = Time.time;
        // Kill counter
        BrPlayerTracker.Instance.OnPlayerDead += (victim, killer, weaponName) =>
        {
            _finishTime = Time.time;

            if (killer == BrCharacterController.MasterCharacter)
                RecordKill();

            if (victim == BrCharacterController.MasterCharacter)
                rank = BrPlayerTracker.Instance.PlayerCounter + 1;
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


    #endregion

    #region Recorders

    public void RecordKill()
    {
        float killTime = Time.time;

        if (killTime - lastKilTime < 4)
        {
            if (killTime - lastDoubleKilTime < 4)
            {
                thisMatchStat.TripleKills++;
                thisMatchStat.DoubleKills--;
                lastKilTime = 0;
                lastDoubleKilTime = 0;
            }
            else
            {
                thisMatchStat.DoubleKills++;
                lastDoubleKilTime = killTime;
                lastKilTime = killTime;
            }
        }
        else
            lastKilTime = killTime;

        thisMatchStat.Kills++;

    }

    public void RecordPickup(BrPickupBase pickUp)
    {
        switch (pickUp)
        {
            case BrChestPickup brChestPickup:
                thisMatchStat.SupplyCreates++;
                break;
            case BrWeaponPickup brWeaponPickup:
                thisMatchStat.GunsCollected++;
                break;
            default:
                thisMatchStat.ItemsCollected++;
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


    #endregion


    public void ShowStats()
    {
        if (Avatar != null) Avatar.SetProfile(ProfileManager.Instance().PlayerProfile);
        thisMatchStat.PlayTime = _finishTime - _startTime;
        win = rank == 1;
        thisMatchStat.Wins = win ? 1 : 0;
        WinText.SetActive(win);
        RankText.gameObject.SetActive(!win);
        RankText.text = "# " + rank;
        MatchXP xp = BrExpManager.CalculateXP(thisMatchStat);
        KillCount.text = thisMatchStat.Kills.ToString();
        KillPoint.text = (xp.K + xp.Dk + xp.Tk).ToString();
        WinPoint.text = xp.W.ToString();
        int total = xp.K + xp.Dk + xp.Tk + xp.W + xp.AP;
        MatchText.text = total.ToString();

        levelSlider.addedXp = total;

        GetComponent<PlayableDirector>().Play();
        SaveMatchRecordToProfile();
    }


    public void SaveMatchRecordToProfile()
    {
        Debug.Log(thisMatchStat.ToString());
        ProfileManager.Instance().SetMatchStat(thisMatchStat);
        thisMatchStat = new MatchStats();
    }

}

public struct MatchXP
{
    public int K;
    public int Dk;
    public int Tk;
    public int AP;
    public int W;
    public int P;
}