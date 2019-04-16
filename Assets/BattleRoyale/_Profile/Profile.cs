using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

[Serializable]
public class Profile
{
    public string UserID;
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

    public Statistics PlayerStat = new Statistics(0);

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
        
        var jPlayer = jInput["player"];

        var profile = new Profile()
        {
            ID=(int) jPlayer["Id"],
            Name = (string) jPlayer["Name"],
            AiBehaviorIndex=(int) jPlayer["AiBehaviorIndex"],
            
            CurrentCharacter = (int) jPlayer["CurrentCharacter"],
            CurrentFlag = (int) jPlayer["CurrentFlag"],
            CurrentPara = (int) jPlayer["CurrentPara"],
            CurrentEmote = (int) jPlayer["CurrentEmote"],
            
            AvalableCharacters= jInput["Characters"].Values<int>().ToList(),
            
            CoinCount=(int) jPlayer["CoinCount"],
            TicketCount=(int) jPlayer["TicketCount"],
            HasBattlePass=(bool) jPlayer["HasBattlePass"],

            
        };

        return profile;
    }
}