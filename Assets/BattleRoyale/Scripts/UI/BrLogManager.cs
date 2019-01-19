using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrLogManager : MonoBehaviour
{
    public static BrLogManager instance;
    public Text text;
    private void Awake()
    {
        instance = this;
        BrPlayerTracker.Instance.OnPlayerDead += OnPlayerDead;

    }

    private void OnPlayerDead(BrCharacterController victom, BrCharacterController killer, string weaponName)
    {
        if(killer)
        {
            text.text += killer.UserID + " " + weaponName + " " + victom.UserID + "\n";
        }
        else
            text.text += victom.UserID + " is dead";

    }
 
}
