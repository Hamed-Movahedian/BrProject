﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BrObjectRotator : MonoBehaviour
{
    public GameObject ObjectToRotate;
    public float Speed;
    
    private float _currentPos;
    private float _startPos;
    private int _width;

    private void Start()
    {
        _width = Screen.width;
    }

    public void OnDrag(BaseEventData data)
    {
        _currentPos=(Application.platform == RuntimePlatform.Android)
            ? data.currentInputModule.input.GetTouch(0).position.x
            : data.currentInputModule.input.mousePosition.x;

        float angle = _currentPos-_startPos;
        ObjectToRotate.transform.Rotate(Vector3.up,angle*Speed/_width);

        _startPos = _currentPos;

    }

    public void OnStartDrag(BaseEventData data)
    {
        _startPos=(Application.platform == RuntimePlatform.Android)
            ? data.currentInputModule.input.GetTouch(0).position.x
            : data.currentInputModule.input.mousePosition.x;
    }
}