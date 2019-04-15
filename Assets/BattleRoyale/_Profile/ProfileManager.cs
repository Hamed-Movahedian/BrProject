using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ProfileManager : MonoBehaviour
{
    #region Singleton

    static ProfileManager _instane;

    public static ProfileManager Instance()
    {
        return _instane ?? (_instane = FindObjectOfType<ProfileManager>());
    }

    #endregion

    public Profile PlayerProfile;
    public CharactersList CharactersList;
    public ParasList ParasList;
    public FlagsList FlagsList;

    public BrLevelRewards levelRewards;
    
    private string _filePath;

    public void Start()
    {
        SceneManager.sceneLoaded += OnSceneChange;
        DontDestroyOnLoad(this.gameObject);
        
        //LoadProfile();
        LoadMainMenu();
    }

    private void OnSceneChange(Scene scene, LoadSceneMode loadMode)
    {
        if (scene.buildIndex==3) TakeTicket();
    }

    private void TakeTicket()
    {
        if (PlayerProfile.HasBattlePass>0)return;
        PlayerProfile.TicketCount--;
        SaveProfile();
    }

    [ContextMenu("Load")]
    private void LoadProfile()
    {
        string fileName = "settings.txt";

#if UNITY_EDITOR
        var filepath = string.Format(@"Assets/StreamingAssets/{0}", fileName);
        if (!File.Exists(filepath))
        {
            PlayerProfile = new Profile();

            File.WriteAllText(filepath, PlayerProfile.Serialize());

            while (!File.Exists(filepath))
            {
            }
        }
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, fileName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb =
 new WWW("jar:file://" + Application.dataPath + "!/assets/" + fileName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb =
 Application.dataPath + "/Raw/" + fileName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb =
 Application.dataPath + "/StreamingAssets/" + fileName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb =
 Application.dataPath + "/StreamingAssets/" + fileName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb =
 Application.dataPath + "/StreamingAssets/" + fileName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

#endif
        _filePath = filepath;
        string profileString = File.ReadAllText(_filePath);
        PlayerProfile = Profile.Deserialize(profileString);
        //LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
        
    }

    
    
    public void SetMatchStat(MatchStats thisMatchStats)
    {
        PlayerProfile.PlayerStat.ChangeStats(thisMatchStats);
        SaveProfile();
    }

    [ContextMenu("Save")]
    public void SaveProfile()
    {
        File.WriteAllText(_filePath, PlayerProfile.Serialize());
    }

    public Texture2D GetProbIcon(int index, ProbType probType)
    {
        switch (probType)
        {
            case ProbType.Character:
                return CharactersList.Characters[index].BodyIcon;
            case ProbType.Para:
                return ParasList.Paras[index].Icon;
            case ProbType.Flag:
                return FlagsList.Flags[index].Icon;
            case ProbType.Emot:
                return CharactersList.Characters[index].BodyIcon;
        }

        return null;
    }

    public void AddProb(ProbType type, int index)
    {
        switch (type)
        {
            case ProbType.Character:
                PlayerProfile.AvalableCharacters.Add(index);
                break;
            case ProbType.Para:
                PlayerProfile.AvalableParas.Add(index);
                break;
            case ProbType.Flag:
                PlayerProfile.AvalableFlags.Add(index);
                break;
            case ProbType.Emot:
                PlayerProfile.AvalableEmotes.Add(index);
                break;
        }

        SaveProfile();
    }

    public bool HaveItem(ProbType probType, int index)
    {
        switch (probType)
        {
            case ProbType.Character:
                return PlayerProfile.AvalableCharacters.Contains(index);
            case ProbType.Para:
                return PlayerProfile.AvalableParas.Contains(index);
            case ProbType.Flag:
                return PlayerProfile.AvalableFlags.Contains(index);
            case ProbType.Emot:
                return PlayerProfile.AvalableEmotes.Contains(index);
        }
        return false;
    }

    public void GiveBattlePass()
    {
        PlayerProfile.HasBattlePass=1;
        int level = BrExpManager.CalLevel(PlayerProfile.PlayerStat.Experience);

        for (var i = 0; i < level; i++)
        {
            LevelReward reward = levelRewards.LevelRewards[i];
            foreach (Inventory inventory in reward.BattlePassReward)
            {
                if (inventory.GetProb() == ProbType.NoProb)
                    continue;

                if (!HaveItem(inventory.GetProb(), inventory.Value))
                {
                    AddProb(inventory.GetProb(), inventory.Value);
                }
            }
        }
    }
}

[Serializable]
public class Profile
{
    public string UserID;

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
    public int HasBattlePass = 0;

    public Statistics PlayerStat = new Statistics(0);

    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }

    public static Profile Deserialize(string text)
    {
        return JsonUtility.FromJson<Profile>(text);
    }
}

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

public class MatchStats
{
    public float PlayTime;
    public int Wins;
    public int Kills;
    public int DoubleKills;
    public int TripleKills;
    public int ItemsCollected;
    public int GunsCollected;
    public int SupplyDrop;
    public int SupplyCreates;
    public int Experience;
}