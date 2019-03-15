using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public SelectProb OnProbSelected;


    
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

    private void OnEnable()
    {
        SetSlider();


        if (rew)
            return;
        
        rew = true;

        foreach (LevelReward level in RewardsList.LevelRewards)
        {
            foreach (Reward reward in level.BattlePassReward)
            {
                var button = Instantiate(ButtonPrefab,BattlePassRewards,true);
                button.transform.localScale=Vector3.one;
                button.SetButton(reward,this);
            }
            
            var o = Instantiate(Sectors, BattlePassRewards, true);
            o.transform.localScale=Vector3.one;
            
            foreach (Reward reward in level.StandardReward)
            {
                var button = Instantiate(ButtonPrefab, StandardRewards, true);
                button.transform.localScale=Vector3.one;
                button.SetButton(reward,this);
            }
            
            o = Instantiate(Sectors,StandardRewards,true);
            o.transform.localScale=Vector3.one;

        }
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
        
        for (int i = 0; i < RewardsList.LevelRewards.Count; i++)
        {
            count += RewardsList.LevelRewards[i].BattlePassReward.Count;
        }

        for (int i = 0; i < level; i++)
        {
            sl += (float)RewardsList.LevelRewards[i].BattlePassReward.Count / count;
        }
        float sl2 = 0;

        for (int i = 0; i < level+1; i++)
        {
            sl2 += (float) RewardsList.LevelRewards[i].BattlePassReward.Count / count;
        }

        float levelLenth = sl2 - sl;
        
        sl +=((float)(xp - cuExp) / (nEXP - cuExp))*(levelLenth);
        
        Debug.Log(sl);
        Debug.Log(sl2);
        Debug.Log(levelLenth);
        slider.value = sl;
    }

    public void ShowProb(Reward reward)
    {
        ProbType probType = reward.GetProb();
        
        if (probType==ProbType.NoProb)
            return;
        
        OnProbSelected(probType, reward.Value);
    }


}
