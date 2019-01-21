using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrLogManager : MonoBehaviour
{
    public Text text;
    private void Awake()
    {
        BrPlayerTracker.Instance.OnPlayerDead += (victom, killer, weaponName) =>
        {
            if (killer)
            {
                text.text += killer.UserID + " " + weaponName + " " + victom.UserID + "\n";
            }
            else
                text.text += victom.UserID + " is dead";
        };

    }
 
}
