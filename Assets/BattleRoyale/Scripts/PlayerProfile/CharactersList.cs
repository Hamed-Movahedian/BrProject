using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersList", menuName = "BattleRoyal/CharactersList", order = 1)]
public class CharactersList : ScriptableObject
{
    public CharacterData[] Characters;

}