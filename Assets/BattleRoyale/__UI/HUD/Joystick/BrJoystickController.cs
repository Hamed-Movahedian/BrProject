using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BrJoystickController : MonoBehaviour
{
    #region Instance
    
    private static BrJoystickController _instance;
    
    public static BrJoystickController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BrJoystickController>();
            return _instance;
        }
    }
    
    #endregion

    public BrJoystick MovementJoystick;
    public BrJoystick AimJoystick;
    private BrCharacterController masterCharacter=null;

    private void Awake()
    {
        gameObject.SetActive(false);
        
        AimJoystick.SetActive(false);
        
        MovementJoystick.SetActive(false);
        
        BrPlayerTracker.Instance.OnMasterPlayerRegister += player =>
        {
            if(player.IsAi)
                return;

            masterCharacter = player;
            
            gameObject.SetActive(true);

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
        if (masterCharacter == null) 
            return;

        var rotation = Quaternion.Euler(0, BrCamera.Instance.MainCamera.transform.eulerAngles.y, 0);
        
        masterCharacter.AimVector = rotation *AimJoystick.Value3;
        masterCharacter.MovVector = rotation *MovementJoystick.Value3;
    }
}
