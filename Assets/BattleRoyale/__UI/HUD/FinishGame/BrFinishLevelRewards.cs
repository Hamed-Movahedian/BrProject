using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BrFinishLevelRewards : MonoBehaviour
{
    public List<GameObject> battleRewards;
    public List<GameObject> standardRewards;

    public BrLevelRewards rewardsList;

    public FlagsList FlagsList;
    public ParasList ParasList;
    public CharactersList CharactersList;


    private int _newLevel;
    private Profile profile;
    private int _currLevel;


    public void ShowRewards(int exp)
    {
        profile = ProfileManager.Instance().PlayerProfile;
        int experience = profile.PlayerStat.Experience;
        _newLevel = BrExpManager.CalLevel(experience);
        _currLevel = BrExpManager.CalLevel(experience-exp);
        if (_currLevel == _newLevel)
            return;
        
        List<Reward> allRewards = new List<Reward>();
        LevelReward rewards = new LevelReward();
        for (int i = _currLevel; i < _newLevel; i++)
        {
            rewards.StandardReward.AddRange(rewardsList[i].StandardReward);
            rewards.BattlePassReward.AddRange(rewardsList[i].BattlePassReward);
            allRewards.AddRange(rewardsList[i].StandardReward);
            allRewards.AddRange(rewardsList[i].BattlePassReward);
        }
        

        foreach (Reward reward in allRewards)
        {
            switch (reward.type)
            {
                case Reward.RewardType.Character:
                    profile.AvalableCharacters.Add(reward.Value);
                    break;
                case Reward.RewardType.Para:
                    profile.AvalableParas.Add(reward.Value);
                    break;
                case Reward.RewardType.Flag:
                    profile.AvalableFlags.Add(reward.Value);
                    break;
                case Reward.RewardType.Emot:
                    profile.AvalableEmotes.Add(reward.Value);
                    break;
                case Reward.RewardType.Coin:
                    profile.CoinCount += (reward.Value);
                    break;
                case Reward.RewardType.Nothing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        ProfileManager.Instance().SaveProfile();

        StartCoroutine(ShowRewardImage(rewards));
    }

    private IEnumerator ShowRewardImage(LevelReward rewards)
    {
        int i = 0;
        foreach (Reward reward in rewards.BattlePassReward)
        {
            Sprite probimage=null;
            
            switch (reward.type)
            {
                case Reward.RewardType.Character:
                    probimage = CharactersList[reward.Value].BodySprite;
                    break;
                case Reward.RewardType.Para:
                    probimage = ParasList[reward.Value].Sprite;
                    
                    break;
                case Reward.RewardType.Flag:
                    probimage = FlagsList[reward.Value].Sprite;

                    break;
                case Reward.RewardType.Emot:
                    probimage = CharactersList[reward.Value].BodySprite;
                    break;
                case Reward.RewardType.Coin:
                    break;
                case Reward.RewardType.Nothing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            battleRewards[i].transform.GetChild(0).GetComponent<Image>().sprite = probimage;
            battleRewards[i].SetActive(true);
            i++;
            yield return new WaitForSeconds(0.5f);

        }
        
        i = 0;
        foreach (Reward reward in rewards.StandardReward)
        {
            Sprite probimage=null;
            
            switch (reward.type)
            {
                case Reward.RewardType.Character:
                    probimage = CharactersList[reward.Value].BodySprite;
                    break;
                case Reward.RewardType.Para:
                    probimage = ParasList[reward.Value].Sprite;
                    
                    break;
                case Reward.RewardType.Flag:
                    probimage = FlagsList[reward.Value].Sprite;

                    break;
                case Reward.RewardType.Emot:
                    probimage = CharactersList[reward.Value].BodySprite;
                    break;
                case Reward.RewardType.Coin:
                    break;
                case Reward.RewardType.Nothing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            standardRewards[i].transform.GetChild(0).GetComponent<Image>().sprite = probimage;
            standardRewards[i].SetActive(true);
            i++;
            yield return new WaitForSeconds(0.5f);

        }

        yield return null;
    }
}