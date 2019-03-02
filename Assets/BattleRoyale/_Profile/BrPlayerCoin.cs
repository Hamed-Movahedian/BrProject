using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrPlayerCoin : MonoBehaviour
{
    private void OnEnable() => GetComponent<Text>().text=
        PersianFixer.Fix(
            ProfileManager.Instance().PlayerProfile.CoinCount.ToString());
}
