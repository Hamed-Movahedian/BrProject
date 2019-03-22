using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward List", menuName = "BattleRoyal/Reward List", order = 5)]
public class BrLevelRewards : ScriptableObject
{
    [SerializeField]
    public List<LevelReward> LevelRewards;

    public LevelReward this[int index]
    {
        get => LevelRewards[index];
        set => LevelRewards[index] = value;
    }


}


[Serializable]
public struct Inventory
{

    public InventoryType type;
    public int Value;

    public ProbType GetProb()
    {
        switch (type)
        {
            case InventoryType.Character:
                return ProbType.Character;
            case InventoryType.Para:
                return ProbType.Para;
            case InventoryType.Flag:
                return ProbType.Flag;
            case InventoryType.Emot:
                return ProbType.Emot;
            default:
                return ProbType.NoProb;
        }
        
    }
}

[Serializable]
public struct LevelReward
{
    //[SerializeField]
    public List<Inventory> StandardReward;
    public List<Inventory> BattlePassReward;

}
public enum InventoryType
{
    Character,Para,Flag,Emot,Coin,Nothing
}
