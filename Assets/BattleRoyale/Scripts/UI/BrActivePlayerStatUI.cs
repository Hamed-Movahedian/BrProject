using System;
using UnityEngine;
using UnityEngine.UI;

public class BrActivePlayerStatUI : MonoBehaviour
{
    public GameObject panle;
    public Text text;
    internal void Show()
    {
        panle.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        text.text = BrPlayerTracker.instance.activePlayer ? BrPlayerTracker.instance.activePlayer.profile.UserID : "";
    }
}