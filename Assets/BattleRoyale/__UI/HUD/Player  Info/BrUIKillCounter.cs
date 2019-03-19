using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrUIKillCounter : MonoBehaviour
{
    public Text text;
    public Text textShadow;

    private int killCount=0;
    // Use this for initialization
    void Start()
    {
        BrPlayerTracker.Instance.OnPlayerDead += PlayerDead;
        UpdateTexts(0);
    }

    private void UpdateTexts(int count)
    {
        text.text =
            textShadow.text = count.ToString();
    }

    private void PlayerDead(BrCharacterController victom, BrCharacterController killer, string weaponName)
    {
        if (killer && killer.IsMaster)
            UpdateTexts(++killCount);

        if (victom.IsMaster)
            gameObject.SetActive(false);

    }
}
