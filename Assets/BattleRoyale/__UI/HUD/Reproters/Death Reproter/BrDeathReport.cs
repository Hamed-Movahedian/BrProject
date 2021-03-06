﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;
public class BrDeathReport : MonoBehaviour
{
    public string NoKillerText;
    public Text rankText;
    public Text KillText;
    public PlayableDirector Director;
    public Button continueButton;
    public BrCharacterModel CharacterModel;
    public UnityEvent OnReport;
    public UnityEvent OnReportNoKiller;
    private void Awake()
    {
        BrPlayerTracker.Instance.OnPlayerDead += (victom, killer, weaponName) =>
        {
            BrPlayerTracker.Instance.OnLastPlayerLeft += player => gameObject.SetActive(false);
            
            if (BrPlayerTracker.Instance.PlayerCounter <= 1)
            {
                gameObject.SetActive(false);
                return;
            }
            
            if (victom.IsMaster)
            {
                
                rankText.text = "# "+BrPlayerTracker.Instance.PlayerCounter+1;
                
                if (killer == null)
                {   // no killer
                    KillText.text = PersianFixer.Fix("شما کشته شدید");
                    OnReportNoKiller.Invoke();
                }
                else // has killer
                {
                    KillText.text = KillText.text
                        .Replace("***",killer.profile.UserID)
                        .Replace("###",weaponName);
                    CharacterModel.SetProfile(killer.profile);
                    OnReport.Invoke();
                }

                Director.Play();
            }
        };
    }

    public void EndShow()
    {
        if (!Application.isPlaying)
            return;
            
        Director.Pause();
    }

    public void EndHide()
    {
        if (!Application.isPlaying)
            return;
        BrActivePlayerStatUI.Instance.Show();
    }

}