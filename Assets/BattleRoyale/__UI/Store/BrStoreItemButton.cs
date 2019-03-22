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
    public Image ItemIcon;
    public Text ItemPrice;


    public void InitializeButton(Sprite icon, int price, bool isMarketItem, Action listAction)
    {
        ItemIcon.sprite = icon;
        string p = PersianFixer.Fix(price.ToString());

        ItemPrice.text = (isMarketItem ? MarketPricePrefix : InGamePricePrefix) + " " + p;

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(listAction.Invoke);
    }
}