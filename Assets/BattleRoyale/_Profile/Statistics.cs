using System;

[Serializable]
public struct Statistics
{
    //public int Level;
    public int TotalBattles;
    public int TotalWins;
    public int TotalKills;
    public int DoubleKills;
    public int TripleKills;
    public int ItemsCollected;
    public int GunsCollected;
    public int SupplyDrop;
    public int SupplyCreates;
    public int Experience;

/*
    public Statistics(int i)
    {
        TotalBattles = i;
        TotalWins = i;
        TotalKills = i;
        DoubleKills = i;
        TripleKills = i;
        ItemsCollected = i;
        GunsCollected = i;
        SupplyDrop = i;
        SupplyCreates = i;
        Experience = i;
    }
*/

    public void ChangeStats(MatchStats thisMatchStats)
    {
        SupplyDrop += thisMatchStats.SupplyDrop;
        Experience += thisMatchStats.Experience;
        SupplyCreates += thisMatchStats.SupplyCreates;
        GunsCollected += thisMatchStats.GunsCollected;
        ItemsCollected += thisMatchStats.ItemsCollected;
        TotalKills += thisMatchStats.Kills;
        DoubleKills += thisMatchStats.DoubleKills;
        TripleKills += thisMatchStats.TripleKills;
        TotalWins += thisMatchStats.Wins;
        TotalBattles++;
    }
}