﻿using System;
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

    private string _filePath;

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadProfile();
    }

    [ContextMenu("Load")]
    private void LoadProfile()
    {
        _filePath = Application.streamingAssetsPath + "/settings.txt";
        string profileString = File.ReadAllText(_filePath);
        PlayerProfile = JsonUtility.FromJson<Profile>(profileString);
        StartCoroutine(LoadMainMenu());
    }

    public IEnumerator LoadMainMenu()
    {
        SceneManager.LoadScene(1);
        while (SceneManager.GetActiveScene().name == "Intro")
            yield return null;
        SetPlayerModel();
    }

    private void SetPlayerModel()
    {
        BrCharacterModel mainMenuCharacter = FindObjectsOfType<BrCharacterModel>().FirstOrDefault(c => c.gameObject.name.Contains("Main"));
        if (mainMenuCharacter)
        {
            CharactersList.Characters[PlayerProfile.CurrentCharacter].SetToCharacter(mainMenuCharacter);
            mainMenuCharacter.BodySkinnedMesh.gameObject.SetActive(true);
        }
    }

    [ContextMenu("Save")]
    public void SaveProfile()
    {
        string content = JsonUtility.ToJson(PlayerProfile);
        File.WriteAllText(_filePath, content);
    }

    public Texture2D GetProbIcon(int idnex, ProbType probsType)
    {
        switch (probsType)
        {
            case ProbType.Character:
                return CharactersList.Characters[idnex].BodyIcon;
            case ProbType.Para:
                return ParasList.Paras[idnex].Icon;
            case ProbType.Flag:
                return FlagsList.Flags[idnex].Icon;
            case ProbType.Emot:
                return CharactersList.Characters[idnex].BodyIcon;
        }
        return null;
    }
}

[Serializable]
public class Profile
{
    public string UserID;

    public List<int> AvalableCharacters;
    public int CurrentCharacter;
    public List<int> AvalableFlags;
    public int CurrentFlag;
    public List<int> AvalableEmotes;
    public int CurrentEmote;
    public List<int> AvalableParas;
    public int CurrentPara;
    public List<string> FriendsUserID;
    public List<string> RequestFrindUserID;

    public Statistics PlayerStat;

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
}