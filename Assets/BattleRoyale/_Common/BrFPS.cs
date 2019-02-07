using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrFPS : MonoBehaviour
{
    public Text text;

    private float _time = 0;
    private int _fcount = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        while (_time<1)
        {
            _time += Time.deltaTime;
            _fcount++;
            return;
        }

        text.text = _fcount.ToString("D");
        _time = 0;
        _fcount = 0;
    }
}
