using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrPlayerCoin : MonoBehaviour
{
    private void Start()
    {
        PurchaseManager.Instance.OnCoinCountChanged+=SetTextValue;

    }

    private void OnEnable()
    {
        SetTextValue(ProfileManager.Instance().PlayerProfile.CoinCount);
    }

    private void SetTextValue(int coinCount)
    {
        GetComponent<Text>().text = PersianFixer.Fix(coinCount.ToString());
    }
}
