﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BrPlayerShowStat : MonoBehaviour
{
    public Text LevelText;
    public Text WinCounText;
    public Text KillCountText;
    public Text DoubleKillText;
    public Text TripleKillText;
    public Text WeaponCountText;
    public Text ItemCountText;
    public Text DropCountText;
    public Text CrateCountText;
    
    
    public Text LevelProgressText;
    public Image LevelProgressImage;

    private ProfileManager _profileManager;

    private Statistics _stats;

    // Start is called before the first frame update
    void Start()
    {
        _profileManager = ProfileManager.Instance();
        if (_profileManager)
        {
            _stats = _profileManager.PlayerProfile.PlayerStat;
            ShowMainStats(_stats);
        }
    }

    public void ShowMainStats(Statistics stats)
    {
        int level = BrExpManager.CalLevel(stats.Experience);

        if (LevelText != null) LevelText.text = PersianFixer.Fix((level+1).ToString(), true, true);
        if (WinCounText != null) WinCounText.text = stats.TotalWins.ToString();
        if (KillCountText != null) KillCountText.text = stats.TotalKills.ToString();
        int preXp = BrExpManager.CalXp(level );
        int nextXp = BrExpManager.CalXp(level+1);
        LevelProgressImage.fillAmount =
            (float)
            (stats.Experience - preXp) /
            (nextXp - preXp);

        LevelProgressText.text = string.Format(
            "{0}/{1}",
            PersianFixer.Fix((stats.Experience - preXp).ToString(), true, true),
            PersianFixer.Fix((nextXp - preXp).ToString(), true, true));

    }

    public void ShowStatDetails()
    {
        if (LevelText != null) LevelText.text = BrExpManager.CalLevel(_stats.Experience).ToString();
        if (WinCounText != null) WinCounText.text = _stats.TotalWins.ToString();
        if (KillCountText != null) KillCountText.text = _stats.TotalKills.ToString();
        if (DoubleKillText != null) DoubleKillText.text = _stats.DoubleKills.ToString();
        if (TripleKillText != null) TripleKillText.text = _stats.TripleKills.ToString();
        if (WeaponCountText != null) WeaponCountText.text = _stats.GunsCollected.ToString();
        if (ItemCountText != null) ItemCountText.text = _stats.ItemsCollected.ToString();
        if (DropCountText != null) DropCountText.text = _stats.SupplyDrop.ToString();
        if (CrateCountText != null) CrateCountText.text = _stats.SupplyCreates.ToString();
    }
}