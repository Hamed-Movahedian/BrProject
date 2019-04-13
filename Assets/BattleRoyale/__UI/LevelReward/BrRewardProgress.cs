using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class BrRewardProgress : MonoBehaviour
{
    
    #region Instance

    private static BrRewardProgress instance;

    public static BrRewardProgress Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BrRewardProgress>();
            return instance;
        }
    }


    #endregion
    
    public delegate void SelectProb(ProbType type, int index);

    //public Action<ProbType, int, bool> OnProbSelected;
    public Action<ProbType, int, bool> OnProbSelected;
    public Action<ProbType, int,string,string, bool> OnProbSelectednew;


    
    public LevelRewardButton ButtonPrefab;
    public GameObject Sectors;
    
    public RectTransform BattlePassRewards;
    public RectTransform StandardRewards;

    public BrLevelRewards RewardsList;

    public CharactersList Characters;
    public ParasList Paras;
    public FlagsList Flags;
    public Texture CoinIcon;
    public Slider slider;
    private bool rew=false;
    private float sessionProgress;

    private void OnEnable()
    {
        SetSlider();


        if (rew)
            return;
        
        rew = true;

        for (var i = 0; i < RewardsList.LevelRewards.Count; i++)
        {
            LevelReward level = RewardsList.LevelRewards[i];
            foreach (Inventory reward in level.BattlePassReward)
            {
                var button = Instantiate(ButtonPrefab, BattlePassRewards, true);
                button.transform.localScale = Vector3.one;
                button.SetButton(reward, this,true,i+1);
            }

            var o = Instantiate(Sectors, BattlePassRewards, true);
            o.transform.localScale = Vector3.one;
            o.GetComponentInChildren<Text>().text=
                o.GetComponentInChildren<Text>().
                    text.Replace("*",
                        PersianFixer.Fix((i + 1).ToString()));
            
            foreach (Inventory reward in level.StandardReward)
            {
                var button = Instantiate(ButtonPrefab, StandardRewards, true);
                button.transform.localScale = Vector3.one;
                button.SetButton(reward, this,false,i+1);
            }

            o = Instantiate(Sectors, StandardRewards, true);
            o.transform.localScale = Vector3.one;
            o.GetComponentInChildren<Text>().text=
                o.GetComponentInChildren<Text>().
                    text.Replace("*", PersianFixer.Fix((i + 1).ToString()));
        }
        ScrollRect scrollRect = GetComponentInChildren<ScrollRect>();
        Debug.Log(scrollRect.horizontalNormalizedPosition);
        scrollRect.horizontalNormalizedPosition=sessionProgress;
    }



    private void SetSlider()
    {
        int xp = ProfileManager.Instance().PlayerProfile.PlayerStat.Experience;
        int level = BrExpManager.CalLevel(xp);
        int cuExp = BrExpManager.CalXp(level);
        int nEXP = BrExpManager.CalXp(level + 1);
        int last = BrExpManager.CalXp(20);
        float sl = 0;
        int count = 0;
        slider.maxValue = 0;
        for (int i = 0; i < RewardsList.LevelRewards.Count; i++)
        {
            count += RewardsList.LevelRewards[i].BattlePassReward.Count;
            slider.maxValue += 0.5f;
        }

        slider.maxValue += count;

        for (int i = 0; i < level; i++)
        {
            sl += RewardsList.LevelRewards[i].BattlePassReward.Count+0.5f ;
        }
        float sl2 = 0;

        for (int i = 0; i < level+1; i++)
        {
            sl2 += RewardsList.LevelRewards[i].BattlePassReward.Count + 0.5f;
        }

        float levelLength = sl2 - sl;
        
        sl +=((float)(xp - cuExp) / (nEXP - cuExp))*(levelLength);
        //sl += (level)/ (2f * count);

        slider.value = sl;
        sessionProgress = sl;
    }

/*
    public void ShowProb(Inventory inventory, bool battle)
    {
        ProbType probType = inventory.GetProb();
        
        if (probType==ProbType.NoProb)
            return;
        
        OnProbSelected(
            probType,
            inventory.Value,
            (battle&&ProfileManager.Instance().PlayerProfile.HasBattlePass==0));
    }
    
    */

    public void PreviewProb(Inventory inventory, bool battle,int level)
    {
        ProbType probType = inventory.GetProb();
        
        if (probType==ProbType.NoProb)
            return;
        
        OnProbSelectednew(
            probType,
            inventory.Value,
            PersianFixer.Fix("جایزه سطح " + level.ToString()) ,
            "",
            battle&&ProfileManager.Instance().PlayerProfile.HasBattlePass==0);
    }


}
