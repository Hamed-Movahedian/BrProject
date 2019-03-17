using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BrCamera : MonoBehaviour
{
    #region Instance

    private static BrCamera _instance;

    public static BrCamera Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BrCamera>();
            return _instance;
        }
    }

    #endregion

    public Camera MainCamera;
    public CinemachineBrain Brain;
    
}
