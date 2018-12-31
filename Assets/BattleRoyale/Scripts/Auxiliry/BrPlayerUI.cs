using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrPlayerUI : MonoBehaviour
{
    public BrCharacterController Character;

    public Slider HealthSlider;
    public Text AmmoText;

    #region Start
    // Use this for initialization
    void Start()
    {

    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        if (Character)
        {
            HealthSlider.value = Character.Health / (float)Character.MaxHealth;
            AmmoText.text=Character.WeaponController.BulletCount.ToString();
        }
    } 
    #endregion
}
