using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BrLogItem : MonoBehaviour
{
    public PlayableDirector director;
    public Text victimText;
    public Text killerText;
    public Image weapon;

    public bool IsFree => director.state != PlayState.Playing;

    public void Show(BrCharacterController victim, BrCharacterController killer, string weaponName)
    {
        victimText.text = victim.UserID;
        if (killer)
        {
            killerText.gameObject.SetActive(true);
            
            killerText.text = killer.UserID;

            if (killer.WeaponController.CurrWeapon != null && killer.WeaponController.Armed)
                weapon.sprite = killer.WeaponController.CurrWeapon.Icon;
            else
                weapon.sprite = BrLogManager.Instance.melleIcon;
        }
        else
        {
            killerText.gameObject.SetActive(false);
            weapon.sprite = BrLogManager.Instance.killIcon;
        }
        
        transform.SetSiblingIndex(0);
        
        director.Play();
    }
}