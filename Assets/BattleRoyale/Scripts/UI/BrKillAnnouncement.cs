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
        BrPlayerTracker.Instance.OnPlayerDead += OnPlayerDead;
        
        // show active player stat button
        continueButton.onClick.AddListener(() =>
        {
            BrActivePlayerStatUI.instance.Show();
            window.gameObject.SetActive(false);
        });

        window.gameObject.SetActive(false);

    }

    private void OnPlayerDead(BrCharacterController victomPlayer, BrCharacterController killerPlayer, string weponName)
    {
        if (victomPlayer.isMine)
        {
            if (killerPlayer == null)
            {   // no killer
                text.text = victomPlayer.profile.UserID + " is dead!!";
            }
            else // has killer
            {
                text.text=killerPlayer.profile.UserID + " kill " + victomPlayer.profile.UserID + " by " + weponName;
            }

            window.gameObject.SetActive(true);

        }
    }
}