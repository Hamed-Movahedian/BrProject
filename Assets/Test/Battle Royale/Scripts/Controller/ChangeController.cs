using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeController : MonoBehaviour
{
    public GameObject Keyboard, MouseMovement, Aim;

    public void Change(Single mode)
    {
                Keyboard.SetActive(mode==1);
                MouseMovement.SetActive(mode!= 1);
    }
}
