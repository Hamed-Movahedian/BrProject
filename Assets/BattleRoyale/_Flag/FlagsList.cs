using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flags List", menuName = "BattleRoyal/Flags List", order = 5)]
public class FlagsList : ScriptableObject
{
    public FlagData[] Flags;

    public FlagData this[int index]
    {
        get { return Flags[index]; }
        set { Flags[index] = value; }
    }
}
