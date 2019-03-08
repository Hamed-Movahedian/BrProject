using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward List", menuName = "BattleRoyal/Reward List", order = 5)]
public class BrLevelRewards : ScriptableObject
{
    [SerializeField]
    public List<LevelReward> LevelRewards;

}


[Serializable]
public struct Reward
{
    public enum RewardType
    {
        Character,Para,Flag,Emot,Coin,Nothing
    }

    public RewardType type;
    public int Value;

}

[Serializable]
public struct LevelReward
{
    //[SerializeField]
    public List<Reward> StandardReward;
    public List<Reward> BattlePassReward;

}
