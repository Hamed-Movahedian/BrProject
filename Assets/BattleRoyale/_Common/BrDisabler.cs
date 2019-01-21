using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrDisabler : MonoBehaviour
{
    public float Delay = 0.05f;

    private void OnEnable()
    {
        Invoke("Disable", Delay);
    }

    private void Disable()
    {
            gameObject.SetActive(false);
    }


}
