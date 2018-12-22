﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrCamera : MonoBehaviour
{
    public static BrCamera Instance;

    public Camera MainCamera;

    #region Auxiliary Cameras

    [Header("Auxiliary Cameras")]
    public Camera FallingCamera;
    public Camera ParaCamera;
    public Camera GroundCamera;
    public Camera GroundAimCamera;

    #endregion

    [Header("Follow")]
    public float CharacterFollowSpeed=1;
    public float TransitionSpeed=1;

    private BrCharacterController _character;
    private Dictionary<CharacterStateEnum, Camera> _camStateDic;

    private void Awake()
    {
        Instance = this;

        #region Disable Auxiliary Cameras

        FallingCamera.gameObject.SetActive(false);
        ParaCamera.gameObject.SetActive(false);
        GroundCamera.gameObject.SetActive(false);
        GroundAimCamera.gameObject.SetActive(false);
        
        #endregion

        _camStateDic = new Dictionary<CharacterStateEnum, Camera>
        {
            {CharacterStateEnum.Falling,FallingCamera },
            {CharacterStateEnum.Parachute,ParaCamera },
            {CharacterStateEnum.Grounded,GroundCamera },
            {CharacterStateEnum.GroundedAim,GroundAimCamera },
        };
    }

	
	void Update ()
	{
		if(!_character)
            return;

        switch (_character.CurrentState)
        {
            case CharacterStateEnum.Falling:
                CameraLerp(FallingCamera, 1);
                break;
            case CharacterStateEnum.Parachute:
                CameraLerp(ParaCamera, TransitionSpeed * Time.deltaTime);
                break;
            case CharacterStateEnum.Grounded:
                CameraLerp(GroundCamera, TransitionSpeed * Time.deltaTime);
                break;
            case CharacterStateEnum.GroundedAim:
                CameraLerp(GroundAimCamera, TransitionSpeed * Time.deltaTime);
                break;
        }

	    //transform.position = Vector3.Lerp(transform.position, _character.transform.position, CharacterFollowSpeed * Time.deltaTime);
        transform.position = _character.transform.position;
    }

    private void CameraLerp(Camera camera, float value)
    {
        // Position
        MainCamera.transform.position = Vector3.Lerp(
            MainCamera.transform.position,
            camera.transform.position,
            value );

        // Rotation
        MainCamera.transform.rotation = Quaternion.Lerp(
            MainCamera.transform.rotation,
            camera.transform.rotation,
            value );

        // FieldOfView
        MainCamera.fieldOfView = Mathf.Lerp(
            MainCamera.fieldOfView,
            camera.fieldOfView,
            value );
    }

    public void SetCharacter(BrCharacterController characterController)
    {
        _character = characterController;
        transform.position = _character.transform.position;

    }

    public void ForceToState(CharacterStateEnum state)
    {
        CameraLerp(_camStateDic[state],1);
        transform.position = _character.transform.position;
    }
}
