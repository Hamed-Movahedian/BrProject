using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersList", menuName = "BattleRoyal/CharactersList", order = 1)]
public class CharactersList : ScriptableObject
{
    public CharacterData[] Characters;
    public CharacterData this[int index]
    {
        get { return Characters[index]; }
        set { Characters[index] = value; }
    }
}