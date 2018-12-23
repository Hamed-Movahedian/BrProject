using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrDisabler : MonoBehaviour
{
    public float Delay = 0.05f;

    private float _timeToDisable;

    private void OnEnable()
    {
        _timeToDisable = Delay;
    }

    void Update ()
    {
        _timeToDisable -= Time.deltaTime;

        if(_timeToDisable<=0)
            gameObject.SetActive(false);
    }
}
