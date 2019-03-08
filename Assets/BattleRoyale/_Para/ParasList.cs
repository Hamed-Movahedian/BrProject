using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Paras List", menuName = "BattleRoyal/Paras List", order = 3)]
public class ParasList : ScriptableObject
{
    public ParaData[] Paras;
    
    public ParaData this[int index]
    {
        get { return Paras[index]; }
        set { Paras[index] = value; }
    }
}
