using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BrDeathReport : MonoBehaviour
{
    public Text rankText;
    public Text KillerText;
    public Text WeaponText;
    public PlayableDirector Director;
    public Button continueButton;
    public BrCharacterModel CharacterModel;
    public UnityEvent OnReport;
    public UnityEvent OnReportNoKiller;
    private void Awake()
    {
        BrPlayerTracker.Instance.OnPlayerDead += (victom, killer, weaponName) =>
        {
            if (victom.isMine)
            {
                rankText.text = "#"+BrPlayerTracker.Instance.PlayerCounter.ToString();
                
                if (killer == null)
                {   // no killer
                    KillerText.text = "?";
                    WeaponText.text = "?";
                    OnReportNoKiller.Invoke();
                }
                else // has killer
                {
                    KillerText.text = killer.profile.UserID;
                    WeaponText.text = weaponName;
                    CharacterModel.SetProfile(killer.profile);
                    OnReport.Invoke();
                }

                Director.Play();
            }
        };

        // show active player stat button
        continueButton.onClick.AddListener(() =>
        {
            Director.Resume();
        });

    }

    public void EndShow()
    {
        if (Application.isEditor)
            return;
        Director.Pause();
    }

    public void EndHide()
    {
        if (Application.isEditor)
            return;
        BrActivePlayerStatUI.Instance.Show();
    }

}