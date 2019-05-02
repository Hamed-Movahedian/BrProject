using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrPlayerCoin : MonoBehaviour
{
    private void Start()
    {

    }

    private void OnEnable()
    {
        PurchaseManager.Instance.OnCoinCountChanged+=SetTextValue;
        print("Added set Text");


        SetTextValue(ProfileManager.Instance().PlayerProfile.CoinCount);
    }

    private void SetTextValue(int coinCount)
    {
        print("setting Text");
        GetComponent<Text>().text = PersianFixer.Fix(coinCount.ToString());
    }
}
