using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

[Serializable]
public class Profile
{
    public string UserID=>$"{Name}#{ID}";
    public int ID;
    public string Name;
    public int AiBehaviorIndex = -1;
    
    public List<int> AvalableCharacters = new List<int> {0, 1};
    public int CurrentCharacter = 0;
    public List<int> AvalableFlags = new List<int> {0};
    public int CurrentFlag = 0;
    public List<int> AvalableEmotes = new List<int> {0};
    public int CurrentEmote = 0;
    public List<int> AvalableParas = new List<int> {0};
    public int CurrentPara = 0;
    
    public List<string> FriendsUserID;
    public List<string> RequestFrindUserID;
    
    public int CoinCount = 100;
    public int TicketCount = 10;
    public bool HasBattlePass = false;

    public Statistics PlayerStat = new Statistics();

    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }

    public static Profile Deserialize(string text)
    {
        return JsonUtility.FromJson<Profile>(text);
    }

    public static Profile Create(string text)
    {
        var jInput = JToken.Parse(text);

        var profile = new Profile()
        {
            ID=(int) jInput["Id"],
            Name = ((string) jInput["Name"]).Replace(" ",""),
            AiBehaviorIndex=(int) jInput["AiBehaviorIndex"],
            
            CurrentCharacter = (int) jInput["CurrentCharacter"],
            CurrentFlag = (int) jInput["CurrentFlag"],
            CurrentPara = (int) jInput["CurrentPara"],
            CurrentEmote = (int) jInput["CurrentEmote"],
            
            AvalableCharacters= jInput["Characters"].Values<int>().ToList(),
            AvalableFlags= jInput["Flags"].Values<int>().ToList(),
            AvalableEmotes= jInput["Emotes"].Values<int>().ToList(),
            AvalableParas= jInput["Parachutes"].Values<int>().ToList(),
            
            CoinCount=(int) jInput["CoinCount"],
            TicketCount=(int) jInput["TicketCount"],
            HasBattlePass=(bool) jInput["HasBattlePass"],

            PlayerStat = new Statistics
            {
                TotalBattles = jInput["TotalBattles"].Value<int>(),
                TotalWins = jInput["TotalWins"].Value<int>(),
                TotalKills = jInput["TotalKills"].Value<int>(),
                DoubleKills = jInput["DoubleKills"].Value<int>(),
                TripleKills = jInput["TripleKills"].Value<int>(),
                ItemsCollected = jInput["ItemsCollected"].Value<int>(),
                GunsCollected = jInput["GunsCollected"].Value<int>(),
                SupplyDrop = jInput["SupplyDrop"].Value<int>(),
                SupplyCreates = jInput["SupplyCreates"].Value<int>(),
                Experience = jInput["Experience"].Value<int>(),
            }
        };

        return profile;
    }
}