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

    public Action<ProbType, int, bool> OnProbSelected;


    
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

        for (var i = 0; i < RewardsList.LevelRewards.Count; i++)
        {
            LevelReward level = RewardsList.LevelRewards[i];
            foreach (Reward reward in level.BattlePassReward)
            {
                var button = Instantiate(ButtonPrefab, BattlePassRewards, true);
                button.transform.localScale = Vector3.one;
                button.SetButton(reward, this,true);
            }

            var o = Instantiate(Sectors, BattlePassRewards, true);
            o.transform.localScale = Vector3.one;
            o.GetComponentInChildren<Text>().text=
                o.GetComponentInChildren<Text>().
                    text.Replace("*",
                        PersianFixer.Fix((i + 1).ToString()));
            
            foreach (Reward reward in level.StandardReward)
            {
                var button = Instantiate(ButtonPrefab, StandardRewards, true);
                button.transform.localScale = Vector3.one;
                button.SetButton(reward, this,false);
            }

            o = Instantiate(Sectors, StandardRewards, true);
            o.transform.localScale = Vector3.one;
            o.GetComponentInChildren<Text>().text=
                o.GetComponentInChildren<Text>().
                    text.Replace("*", PersianFixer.Fix((i + 1).ToString()));
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

        float levelLenth = sl2 - sl;
        
        sl +=((float)(xp - cuExp) / (nEXP - cuExp))*(levelLenth);
        //sl += (level)/ (2f * count);
        
        Debug.Log(sl);
        Debug.Log(sl2);
        Debug.Log(levelLenth);
        slider.value = sl;
    }

    public void ShowProb(Reward reward, bool battle)
    {
        ProbType probType = reward.GetProb();
        
        if (probType==ProbType.NoProb)
            return;
        
        OnProbSelected(
            probType,
            reward.Value,
            (battle&&ProfileManager.Instance().PlayerProfile.HasBattlePass==0));
    }


}
