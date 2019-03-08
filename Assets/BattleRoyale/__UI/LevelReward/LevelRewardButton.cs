using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewardButton : MonoBehaviour
{
    public RawImage ProbImage;


    public void SetButton(Reward reward, BrRewardProgress list)
    {
        switch (reward.type)
        {
            case Reward.RewardType.Character:
                ProbImage.texture = list.Characters[reward.Value].BodyIcon;
                break;
            case Reward.RewardType.Para:
                ProbImage.texture = list.Paras[reward.Value].Icon;

                break;
            case Reward.RewardType.Flag:
                ProbImage.texture = list.Flags[reward.Value].Icon;

                break;
            case Reward.RewardType.Emot:
                ProbImage.texture = list.Paras[reward.Value].Icon;

                break;
            case Reward.RewardType.Coin:
                ProbImage.texture = list.CoinIcon;

                break;
            default:
                ProbImage.gameObject.SetActive(false);
                break;
        }
    }
}