using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "Emote Data", menuName = "BattleRoyal/Emote Data", order = 5)]
public class EmoteData : ScriptableObject
{
    [HideInInspector]
    public int ID = -1;

    public string Name;

    public Texture2D Icon;

    public bool HasByDefault = false;
}