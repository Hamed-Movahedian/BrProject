﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrRewardProgress : MonoBehaviour
{
    public LevelRewardButton ButtonPrefab;
    public GameObject Sectors;
    
    public RectTransform BattlePassRewards;
    public RectTransform StandardRewards;

    public BrLevelRewards RewardsList;

    public CharactersList Characters;
    public ParasList Paras;
    public FlagsList Flags;
    public Texture CoinIcon;

    private void OnEnable()
    {
        foreach (LevelReward level in RewardsList.LevelRewards)
        {
            foreach (Reward reward in level.BattlePassReward)
            {
                var button = Instantiate(ButtonPrefab);
                button.transform.parent = BattlePassRewards;
                button.transform.localScale=Vector3.one;
                button.SetButton(reward,this);
            }
            
            var o = Instantiate(Sectors);
            o.transform.parent=BattlePassRewards;
            o.transform.localScale=Vector3.one;
            
            foreach (Reward reward in level.StandardReward)
            {
                var button = Instantiate(ButtonPrefab);
                button.transform.parent = StandardRewards;
                button.transform.localScale=Vector3.one;
                button.SetButton(reward,this);
            }
            
            o = Instantiate(Sectors);
            o.transform.parent=StandardRewards;
            o.transform.localScale=Vector3.one;

        }
    }


}
