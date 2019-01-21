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
        BrPlayerTracker.Instance.OnPlayerRegisterd += player =>
        {
            UpdateTexts();
        };

        BrPlayerTracker.Instance.OnPlayerDead += (victom, killer, weaponName) =>
        {
            UpdateTexts();
        };
	}

    public void UpdateTexts()
    {
        text.text =
            textShadow.text =
                BrPlayerTracker.Instance.PlayerCounter.ToString();
    }

}
