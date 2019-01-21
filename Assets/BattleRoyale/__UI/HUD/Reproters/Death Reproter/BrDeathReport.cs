using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BrDeathReport : MonoBehaviour
{
    public Text text;
    public Button continueButton;
    public UnityEvent OnReport;
    private void Awake()
    {
        BrPlayerTracker.Instance.OnPlayerDead += (victom, killer, weaponName) =>
        {
            if (victom.isMine)
            {
                if (killer == null)
                {   // no killer
                    text.text = victom.profile.UserID + " is dead!!";
                }
                else // has killer
                {
                    text.text = killer.profile.UserID + " kill " + victom.profile.UserID + " by " + weaponName;
                }
                OnReport.Invoke();
            }
        };
        
        // show active player stat button
        continueButton.onClick.AddListener(() =>
        {
            BrActivePlayerStatUI.Instance.Show();
        });

    }

}