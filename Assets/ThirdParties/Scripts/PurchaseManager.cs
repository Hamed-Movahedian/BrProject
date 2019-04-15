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
#if UNITY_ANDROID
        BazaarIAB.init(
            "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwDd+IlpeNJRf25jGcQxyNnaM2F7ieP5q6yt3PGnizCrzWIbHoLmquM3ZQbPWXSYjpYEJ2dbIHROEvZHD4+oQOEm2skVGCRpbq2nFZm9p1QKstTwbYnWI1bRmeRil/G56VrMt1CvmN3pmgQ/CW6N4aZJawbn/58M+2c0Fwz9AUIECYhB6GHrmqtmBVq2u8zVE62hjK/Ri1QH6bSspvl73feIfDA+yQ4NWV1gwNT9KXECAwEAAQ==");

        IABEventManager.purchaseSucceededEvent += purchase => OnItemPurchased.Invoke(purchase.ProductId);
        IABEventManager.purchaseSucceededEvent += NewMethod;
        IABEventManager.purchaseFailedEvent += NewMethod2;

#endif
    }

    private void NewMethod2(string obj)
    {
        Debug.Log("Purchase Failed");
    }

/*
    private void NewMethod(BazaarPurchase BP)
    {
        Debug.Log("Purchase Done");
        Debug.Log(BP.ProductId);
    }
*/

    public void BuyItem(string itemId)
    {
#if UNITY_ANDROID
        Debug.Log("Purchase started in Bazaar");

        BazaarIAB.purchaseProduct(itemId);

        Debug.Log("Purchase Done in Bazaar");

#endif
    }

    public static void Consume(string itemId)
    {
        //BazaarIAB.consumeProduct(itemId);
    }
}