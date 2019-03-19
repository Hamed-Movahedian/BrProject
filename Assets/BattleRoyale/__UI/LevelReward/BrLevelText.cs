using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrLevelText : MonoBehaviour
{
    private void OnEnable()
    {
        string l =
            PersianFixer.Fix(
                BrExpManager.CalLevel(ProfileManager.Instance().PlayerProfile.PlayerStat.Experience)
                    .ToString());
        GetComponent<Text>().text = GetComponent<Text>().text.Replace("*", l); 
    }
}
