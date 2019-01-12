using System;
using UnityEngine;
using UnityEngine.UI;

public class BrKillAnnouncement : MonoBehaviour
{
    public Text text;
    public GameObject window;
    public Button continueButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(() =>
        {
            BrUIController.Instance.ShowActivePlayerStat();
            window.gameObject.SetActive(false);
        });
    }

    internal void Announce(BrCharacterController victomPlayer, BrCharacterController killerPlayer, string weponName)
    {
        text.text=killerPlayer.profile.UserID + " kill " + victomPlayer.profile.UserID + " by " + weponName;
        window.gameObject.SetActive(true);
    }

    internal void Announce(BrCharacterController victomPlayer)
    {
        text.text = victomPlayer.profile.UserID + " is dead!!";
        window.gameObject.SetActive(true);
    }
}