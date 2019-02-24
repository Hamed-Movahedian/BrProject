using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrJoystickController : MonoBehaviour
{
    public static BrJoystickController Instance;

    public BrJoystick MovementJoystick;
    public BrJoystick AimJoystick;

    private void Awake()
    {
        Instance = this;
        
        AimJoystick.SetActive(false);
        
        MovementJoystick.SetActive(false);
        
        BrPlayerTracker.Instance.OnMasterPlayerRegister += player =>
        {
            player.ParachuteState.OnOpenPara.AddListener(() =>
            {
                MovementJoystick.SetActive(true);
            });

            player.ParachuteState.OnLanding.AddListener(() =>
            {
                AimJoystick.SetActive(true);
            });
            
            player.OnDead.AddListener(() =>
            {
                AimJoystick.SetActive(false);
                MovementJoystick.SetActive(false);
            });
        };
        
    }

    private void Update()
    {
        var masterCharacter = BrCharacterController.MasterCharacter;
        
        if (masterCharacter == null) 
            return;

        var rotation = Quaternion.Euler(0, BrCamera.Instance.MainCamera.transform.eulerAngles.y, 0);
        
        masterCharacter.AimVector = rotation *AimJoystick.Value3;
        masterCharacter.MovVector = rotation *MovementJoystick.Value3;
    }
}
