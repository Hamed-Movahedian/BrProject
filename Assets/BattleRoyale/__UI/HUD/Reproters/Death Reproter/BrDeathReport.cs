using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BrDeathReport : MonoBehaviour
{
    public Text text;
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
                if (killer == null)
                {   // no killer
                    text.text = victom.profile.UserID + " is dead!!";
                    OnReportNoKiller.Invoke();
                }
                else // has killer
                {
                    text.text = killer.profile.UserID + " kill " + victom.profile.UserID + " by " + weaponName;
                    CharacterModel.SetProfile(killer.profile);
                    OnReport.Invoke();
                }
            }
        };

        // show active player stat button
        continueButton.onClick.AddListener(() =>
        {
            BrActivePlayerStatUI.Instance.Show();
        });

    }

}