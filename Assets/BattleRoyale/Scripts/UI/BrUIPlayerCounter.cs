using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrUIPlayerCounter : MonoBehaviour
{
    public Text text;
    public Text textShadow;

    // Use this for initialization
    void Start ()
    {
        BrPlayerTracker.instance.OnPlayerRegisterd += PlayerRegistered;
        BrPlayerTracker.instance.OnPlayerDead += PlayerDead;
	}

    private void PlayerRegistered(BrCharacterController player)
    {
        UpdateTexts();
    }

    private void PlayerDead(BrCharacterController victom, BrCharacterController killer, string weaponName)
    {
        UpdateTexts();
    }

    public void UpdateTexts()
    {
        text.text =
            textShadow.text =
                BrPlayerTracker.instance.PlayerCounter.ToString();
    }

}
