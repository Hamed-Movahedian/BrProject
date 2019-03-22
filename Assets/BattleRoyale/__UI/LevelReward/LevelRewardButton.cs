using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewardButton : MonoBehaviour
{
    public RawImage ProbImage;
    public GameObject LockIcon;

    public void SetButton(Inventory inventory, BrRewardProgress list, bool battle)
    {
        if (battle)
            LockIcon.SetActive(ProfileManager.Instance().PlayerProfile.HasBattlePass == 0);

        switch (inventory.type)
        {
            case InventoryType.Character:
                ProbImage.texture = list.Characters[inventory.Value].BodyIcon;
                break;
            case InventoryType.Para:
                ProbImage.texture = list.Paras[inventory.Value].Icon;

                break;
            case InventoryType.Flag:
                ProbImage.texture = list.Flags[inventory.Value].Icon;

                break;
            case InventoryType.Emot:
                ProbImage.texture = list.Paras[inventory.Value].Icon;

                break;
            case InventoryType.Coin:
                ProbImage.texture = list.CoinIcon;

                break;
            default:
                ProbImage.gameObject.SetActive(false);
                break;
        }
        
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(()=>list.ShowProb(inventory,battle));
    }
}