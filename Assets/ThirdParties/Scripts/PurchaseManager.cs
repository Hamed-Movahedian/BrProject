using System;
#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using BazaarPlugin;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    public delegate void PurchaseDone(string itemId);

    [HideInInspector] public PurchaseDone OnItemPurchased;

    public delegate void CoinCountChanged(int CountCount);

    [HideInInspector] public CoinCountChanged OnCoinCountChanged;

    public Text Debuger;


    #region Instance

    private static PurchaseManager instance;

    public static PurchaseManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PurchaseManager>();
            return instance;
        }
    }

    #endregion


    void Start()
    {
        OnItemPurchased += PurchaseSucceed;

#if UNITY_ANDROID
        BazaarIAB.init(
            "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwDd+IlpeNJRf25jGcQxyNnaM2F7ieP5q6yt3PGnizCrzWIbHoLmquM3ZQbPWXSYjpYEJ2dbIHROEvZHD4+oQOEm2skVGCRpbq2nFZm9p1QKstTwbYnWI1bRmeRil/G56VrMt1CvmN3pmgQ/CW6N4aZJawbn/58M+2c0Fwz9AUIECYhB6GHrmqtmBVq2u8zVE62hjK/Ri1QH6bSspvl73feIfDA+yQ4NWV1gwNT9KXECAwEAAQ==");

        IABEventManager.purchaseSucceededEvent += purchase => OnItemPurchased.Invoke(purchase.ProductId);
#if UNITY_ANDROID

        IABEventManager.purchaseSucceededEvent += NewMethod;
#endif
        IABEventManager.purchaseFailedEvent += NewMethod2;

#endif
    }

    private void NewMethod2(string obj)
    {
        Debug.Log("Purchase Failed");
    }

#if UNITY_ANDROID

    private void NewMethod(BazaarPurchase BP)
    {
        Debug.Log(BP.ProductId);
    }
#endif


    public void BuyItem(string itemId)
    {
#if UNITY_ANDROID

        BazaarIAB.purchaseProduct(itemId);

#endif
    }

    private void PurchaseSucceed(string itemId)
    {
        foreach (MarketItem item in BrStoreList.Instance.PurchaseItems)
        {
            if (item.ItemId == itemId)
            {
                ProfileManager.Instance().PlayerProfile.CoinCount += item.CoinCount;
                ProfileManager.Instance().SaveProfile();
                OnCoinCountChanged?.Invoke(ProfileManager.Instance().PlayerProfile.CoinCount);
                Consume(itemId);
                return;
            }
        }
    }


    private static void Consume(string itemId)
    {
#if UNITY_ANDROID
        BazaarIAB.consumeProduct(itemId);
#endif
    }

    public bool PayCoin(int price)
    {
        if (price > ProfileManager.Instance().PlayerProfile.CoinCount)
            return false;
        
        
        ProfileManager.Instance().PlayerProfile.CoinCount -= price;
        ProfileManager.Instance().SaveProfile();
        OnCoinCountChanged?.Invoke(ProfileManager.Instance().PlayerProfile.CoinCount);
        return true;
    }
}