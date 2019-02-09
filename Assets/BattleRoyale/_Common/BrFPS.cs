using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrFPS : MonoBehaviour
{
    public Text text;

    private float _time = 0;
    private int _fcount = 0;


    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        _fcount++;

        if (_time >= 1)
        {
            text.text = (_fcount / _time).ToString("F0");
            _time = 0;
            _fcount = 0;
        }
    }
}