using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BrExpManager
{
    private const int Multiplyer = 10;
    public static MatchXP CalculateXP(MatchStats match)
    {
        Statistics stat = playerStat;

        MatchXP matchXp = new MatchXP
        {
            K = (match.Kills - 2 * match.DoubleKills - 3 * match.TripleKills) * Multiplyer,
            Dk = match.DoubleKills * 2 * Multiplyer,
            Tk = match.TripleKills * 4 * Multiplyer,
            AP = match.SupplyDrop * 2 * Multiplyer,
            W = match.Wins * 5 * Multiplyer,
            P=(1+(int)(match.PlayTime/60))* Multiplyer
        };

        return matchXp;
    }


    public static int CalLevel(int xp)
    {
        int l = 0;
        
        while (xp >= 0)
        {
            l++;
            xp -= (int)Math.Round(10 * Mathf.Pow(1.122f, l - 1), 5) * Multiplyer;
        }
        
        return l-1;
    }
    public static int CalXp(int l)
    {
        int xp = 0;
        for (int i = 0; i <= l; i++) 
            xp += (int) Math.Round(10 * Mathf.Pow(1.122f, i), 5) * Multiplyer;
        
        return xp;
    }

    public static Statistics playerStat => ProfileManager.Instance().PlayerProfile.PlayerStat;
}
