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
        SetAimJoyisticActive(false);
        SetMovementJoyisticActive(false);
        BrPlayerTracker.Instance.OnPlayerRegisterd += OnPlayerRegisterd;
        BrPlayerTracker.Instance.OnPlayerDead += OnPlayerDead;
    }

    private void OnPlayerDead(BrCharacterController victom, BrCharacterController killer, string weaponName)
    {
        if (victom.isMine)
        {
            SetAimJoyisticActive(false);
            SetMovementJoyisticActive(false);

        }
    }

    private void OnPlayerRegisterd(BrCharacterController player)
    {
        if (player.isMine)
        {
            player.ParachuteState.OnOpenPara.AddListener(() =>
            {
                SetMovementJoyisticActive(true);
            });

            player.ParachuteState.OnLanding.AddListener(() =>
            {
                SetAimJoyisticActive(true);
            });
        }
    }

    public void SetMovementJoyisticActive(bool b)
    {
        MovementJoystick.gameObject.SetActive(b);
    }
    public void SetAimJoyisticActive(bool b)
    {
        AimJoystick.gameObject.SetActive(b);
    } 
}
