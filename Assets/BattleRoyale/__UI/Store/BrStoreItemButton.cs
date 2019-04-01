using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BrStoreItemButton : MonoBehaviour
{
    public string MarketPricePrefix;
    public string InGamePricePrefix;
    public string CoinDes;
    public string TicketDes;

    public Image ItemIcon;
    public Text ItemPrice;
    public Text itemDescribe;
    


    public void InitializeButton(Sprite icon, int price, int itemCount, MarketItemType type, Action listAction)
    {
        ItemIcon.sprite = icon;
        
        string p = PersianFixer.Fix(price.ToString());
        ItemPrice.text = (type==MarketItemType.Coin?MarketPricePrefix:InGamePricePrefix) + " " + p;
        p = PersianFixer.Fix(itemCount.ToString());

        itemDescribe.text = (type == MarketItemType.Coin ? CoinDes : type == MarketItemType.Ticket ? TicketDes:"" )+ " " + p;
        itemDescribe.gameObject.SetActive(type != MarketItemType.Prob);
        
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(listAction.Invoke);
    }
}

public enum MarketItemType
{
    Prob,
    Coin,
    Ticket
}