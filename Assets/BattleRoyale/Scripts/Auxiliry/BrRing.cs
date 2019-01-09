using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrRing : MonoBehaviour
{
    public Transform Outter;
    public Transform Inner;

    public float radious=3;
    public float thickness=.2f;
    // Use this for initialization
    void Start () {
		
	}

    private void OnValidate()
    {
        setup();
    }

    private void setup()
    {
        Inner.localScale = Vector3.one * radious;
        Outter.localScale = Vector3.one * (radious + thickness);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
