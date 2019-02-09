using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BrLogManager : MonoBehaviour
{
    #region Instance

    private static BrLogManager _instance;

    public static BrLogManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BrLogManager>();
            return _instance;
        }
    }

    #endregion

    public Sprite melleIcon;
    public Sprite killIcon;
    public List<BrLogItem> items;

    private void Awake()
    {
        BrPlayerTracker.Instance.OnPlayerDead += (victim, killer, weaponName) =>
        {
            foreach (var item in items)
                if (item.IsFree)
                {
                    item.Show(victim, killer, weaponName);
                    return;
                }

        };

    }
 
}
